﻿using DingTalk.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DingTalk.Controllers
{
    public class DrawingUploadController : Controller
    {
        // GET: DrawingUpload
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string Upload(FormCollection form)
        {
            if (Request.Files.Count == 0)
            {
                //Request.Files.Count 文件数为0上传不成功
                return JsonConvert.SerializeObject(new ErrorModel
                {
                    errorCode = 1,
                    errorMessage = "文件数为0"
                });
            }

            var file = Request.Files[0];
            if (file.ContentLength == 0)
            {
                //文件大小大（以字节为单位）为0时，做一些操作
                return JsonConvert.SerializeObject(new ErrorModel
                {
                    errorCode = 2,
                    errorMessage = "文件大小大（以字节为单位）为0"
                });
            }
            else
            {
                //文件大小不为0
                HttpPostedFileBase files = Request.Files[0];
                string newFile = DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
                files.SaveAs(Server.MapPath(@"~\UploadFile\Excel\"+ newFile));
            }
            return JsonConvert.SerializeObject(new ErrorModel
            {
                errorCode = 0,
                errorMessage = "上传成功"
            });
        }

    }
}