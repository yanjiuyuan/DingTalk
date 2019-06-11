﻿using Common.ClassChange;
using Common.DTChange;
using Common.Flie;
using Common.Ionic;
using Common.PDF;
using DingTalk.EF;
using DingTalk.Models;
using DingTalk.Models.DingModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DingTalk.Controllers
{
    /// <summary>
    /// 技术支持单
    /// </summary>
    [RoutePrefix("TechnicalSupport")]
    public class TechnicalSupportController : ApiController
    {
        /// <summary>
        /// 图纸BOM变更表单保存
        /// </summary>
        /// <returns></returns>
        [Route("Save")]
        [HttpPost]
        public NewErrorModel Save(TechnicalSupport technicalSupport)
        {
            try
            {
                EFHelper<TechnicalSupport> eFHelper = new EFHelper<TechnicalSupport>();
                technicalSupport.CompanyName = "泉州华中科技大学智能制造研究院";
                technicalSupport.ProjectState = "在研";
                technicalSupport.ProjectType = "测试项目";
                eFHelper.Add(technicalSupport);

                return new NewErrorModel()
                {
                    data = "",
                    error = new Error(0, "保存成功！", "") { },
                };
            }
            catch (Exception ex)
            {
                return new NewErrorModel()
                {
                    error = new Error(1, ex.Message, "") { },
                };
            }
        }

        /// <summary>
        /// 图纸BOM变更表单读取
        /// </summary>
        /// <param name="TaskId"></param>
        /// <returns></returns>
        [Route("Read")]
        [HttpGet]
        public NewErrorModel Read(string TaskId)
        {
            try
            {
                using (DDContext context = new DDContext())
                {
                    TechnicalSupport technicalSupport = context.TechnicalSupport.Where(c => c.TaskId == TaskId).FirstOrDefault();
                    
                    return new NewErrorModel()
                    {
                        data = technicalSupport,
                        error = new Error(0, "读取成功！", "") { },
                    };
                }
            }
            catch (Exception ex)
            {
                return new NewErrorModel()
                {
                    error = new Error(1, ex.Message, "") { },
                };
            }
        }


        /// <summary>
        /// 图纸BOM变更表单修改
        /// </summary>
        /// <returns></returns>
        [Route("Modify")]
        [HttpPost]
        public NewErrorModel Modify(TechnicalSupport technicalSupport)
        {
            try
            {
             
                EFHelper<TechnicalSupport> eFHelper = new EFHelper<TechnicalSupport>();
                eFHelper.Modify(technicalSupport);
                if (technicalSupport.IsCreateProject)
                {
                    using (DDContext context = new DDContext())
                    {
                        ProjectInfo projectInfo = new ProjectInfo();
                        projectInfo.ProjectName = technicalSupport.ProjectName;
                        projectInfo.CreateTime = DateTime.Now.ToString("yyyy-dd-mm HH:mm:ss");
                        projectInfo.IsEnable = true;
                        projectInfo.ProjectState = technicalSupport.ProjectState;
                        projectInfo.DeptName = technicalSupport.DeptName;
                        projectInfo.StartTime = technicalSupport.StartTime;
                        projectInfo.EndTime = technicalSupport.EndTime;
                        projectInfo.CompanyName = technicalSupport.CompanyName;
                     
                        //建立项目文件夹及其子文件
                        string path = string.Format("\\UploadFile\\ProjectFile\\{0}\\{1}\\{2}",
                            projectInfo.CompanyName, technicalSupport.ProjectType, technicalSupport.ProjectName);
                        projectInfo.FilePath = path;
                        projectInfo.ResponsibleMan = technicalSupport.ResponsibleMan;
                        projectInfo.ResponsibleManId = technicalSupport.ResponsibleManId;
                       
                        projectInfo.TeamMembers = technicalSupport.TeamMembers;
                        projectInfo.TeamMembersId = technicalSupport.TeamMembersId;

                        context.ProjectInfo.Add(projectInfo);
                        context.SaveChanges();
                        path = System.Web.Hosting.HostingEnvironment.MapPath(path);
                        FileHelper.CreateDirectory(path);
                    }
                }
             

                return new NewErrorModel()
                {
                    data = "",
                    error = new Error(0, "修改成功！", "") { },
                };
            }
            catch (Exception ex)
            {
                return new NewErrorModel()
                {
                    error = new Error(1, ex.Message, "") { },
                };
            }
        }



        /// <summary>
        /// 打印表单数据、盖章、推送
        /// </summary>
        /// 测试数据   /Purchase/PrintAndSend
        /// data: { "UserId":"083452125733424957","TaskId":"20"}
        [HttpPost]
        [Route("PrintAndSend")]
        public async Task<NewErrorModel> PrintAndSend([FromBody]PrintAndSendModel printAndSendModel)
        {
            try
            {
                string TaskId = printAndSendModel.TaskId;
                string UserId = printAndSendModel.UserId;
                PDFHelper pdfHelper = new PDFHelper();
                using (DDContext context = new DDContext())
                {
                    //获取表单信息
                    Tasks tasks = context.Tasks.Where(t => t.TaskId.ToString() == TaskId && t.NodeId == 0).First();
                    string FlowId = tasks.FlowId.ToString();
                    string ProjectId = tasks.ProjectId;
                    //判断流程是否已结束
                    List<Tasks> tasksList = context.Tasks.Where(t => t.TaskId.ToString() == TaskId && t.State == 0 && t.IsSend == false).ToList();
                    if (tasksList.Count > 0)
                    {
                        return new NewErrorModel()
                        {
                            error = new Error(1, "流程未结束！", "") { },
                        };
                    }

                   TechnicalSupport technicalSupport = context.TechnicalSupport.Where(u => u.TaskId == TaskId).First();

                   List<NodeInfo> NodeInfoList = context.NodeInfo.Where(u => u.FlowId == FlowId && u.NodeId != 0 && u.IsSend != true && u.NodeName != "结束").ToList();
                    foreach (NodeInfo nodeInfo in NodeInfoList)
                    {
                        if (string.IsNullOrEmpty(nodeInfo.NodePeople))
                        {
                            string strNodePeople = context.Tasks.Where(q => q.TaskId.ToString() == TaskId && q.NodeId == nodeInfo.NodeId).First().ApplyMan;
                            string ApplyTime = context.Tasks.Where(q => q.TaskId.ToString() == TaskId && q.NodeId == nodeInfo.NodeId).First().ApplyTime;
                            nodeInfo.NodePeople = strNodePeople + "  " + ApplyTime;
                        }
                        else
                        {
                            string ApplyTime = context.Tasks.Where(q => q.TaskId.ToString() == TaskId && q.NodeId == nodeInfo.NodeId).First().ApplyTime;
                            nodeInfo.NodePeople = nodeInfo.NodePeople + "  " + ApplyTime;
                        }
                    }
                    DataTable dtApproveView = ClassChangeHelper.ToDataTable(NodeInfoList);
                    string FlowName = context.Flows.Where(f => f.FlowId.ToString() == FlowId).First().FlowName.ToString();
                    ProjectInfo projectInfo = context.ProjectInfo.Where(p => p.ProjectId == ProjectId).First();
                    string ProjectName = projectInfo.ProjectName;
                    string ProjectNo = projectInfo.ProjectId;

                   

                 
                    string path = pdfHelper.GeneratePDF(FlowName, TaskId, tasks.ApplyMan, tasks.Dept, tasks.ApplyTime,
                    ProjectName, ProjectNo, "2", 300, 650, null, null, null, dtApproveView, null);
                    string RelativePath = "~/UploadFile/PDF/" + Path.GetFileName(path);

                    List<string> newPaths = new List<string>();
                    RelativePath = AppDomain.CurrentDomain.BaseDirectory + RelativePath.Substring(2, RelativePath.Length - 2).Replace('/', '\\');
                    newPaths.Add(RelativePath);
                    string SavePath = string.Format(@"{0}\UploadFile\Ionic\{1}.zip", AppDomain.CurrentDomain.BaseDirectory, FlowName + DateTime.Now.ToString("yyyyMMddHHmmss"));
                    //文件压缩打包
                    IonicHelper.CompressMulti(newPaths, SavePath, false);

                    //上传盯盘获取MediaId
                    SavePath = string.Format(@"~\UploadFile\Ionic\{0}", Path.GetFileName(SavePath));
                    DingTalkServersController dingTalkServersController = new DingTalkServersController();
                    var resultUploadMedia = await dingTalkServersController.UploadMedia(SavePath);
                    //推送用户
                    FileSendModel fileSendModel = JsonConvert.DeserializeObject<FileSendModel>(resultUploadMedia);
                    fileSendModel.UserId = UserId;
                    var result = await dingTalkServersController.SendFileMessage(fileSendModel);

                    return new NewErrorModel()
                    {
                        error = new Error(0, result, "") { },
                    };
                }
            }
            catch (Exception ex)
            {
                return new NewErrorModel()
                {
                    error = new Error(2, ex.Message, "") { },
                };
            }
        }

    }
}
