namespace DingTalk.Models.DingModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NodeInfo")]
    public partial class NodeInfo
    {
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal Id { get; set; }

        public int? NodeId { get; set; }

        [StringLength(100)]
        public string FlowId { get; set; }

        [StringLength(200)]
        public string NodeName { get; set; }

        [StringLength(500)]
        public string NodePeople { get; set; }

        [StringLength(500)]
        public string PeopleId { get; set; }

        [StringLength(200)]
        public string PreNodeId { get; set; }

        public bool? IsAllAllow { get; set; }

        [StringLength(500)]
        public string Condition { get; set; }

        public bool? IsBack { get; set; }

        public bool? IsNeedChose { get; set; }

        public bool? IsSend { get; set; }

        [StringLength(100)]
        public string BackNodeId { get; set; }

        [StringLength(100)]
        public string ChoseNodeId { get; set; }

        /// <summary>
        /// ѡ�˿ؼ���ѡ���߶�ѡ(��ѡ 0 ��ѡ 1  ��ʽ  0,1,0,1 )
        /// </summary>
        [StringLength(200)]
        public string IsSelectMore { get; set; }

        /// <summary>
        /// �Ƿ��ѡ (0 ����ѡ 1 ��ѡ  ��ʽ  0,1,0,1)
        /// </summary>
        [StringLength(200)]
        public string IsMandatory { get; set; }

        /// <summary>
        /// ѡ�˿ؼ�����(0 ����ѡ�˿ؼ� 1 ��ɫѡ�˿ؼ�)
        /// </summary>
        [StringLength(50)]
        public string ChoseType { get; set; }

        /// <summary>
        /// ��ɫѡ�����õĽ�ɫ��Ϣ(��������Ա,ͼ�����Ա)
        /// </summary>
        [StringLength(500)]
        public string RoleNames { get; set; }

        /// <summary>
        /// ѡ�˽�ɫ�б�
        /// </summary>
        [NotMapped]
        public Dictionary<string, List<Roles>> RolesList { get; set; }
        //public List<List<Roles>> Roles { get; set; }


    }
}
