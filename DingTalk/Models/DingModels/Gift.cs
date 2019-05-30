﻿namespace DingTalk.Models.DingModels{    using System;    using System.Collections.Generic;    using System.ComponentModel.DataAnnotations;    using System.ComponentModel.DataAnnotations.Schema;    using System.Data.Entity.Spatial;    [Table("Gift")]    public partial class Gift    {        [Column(TypeName = "numeric")]        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]        public decimal Id { get; set; }

        /// <summary>        /// 类别        /// </summary>        [StringLength(100)]        public string Type { get; set; }
        
        /// <summary>        /// 项目        /// </summary>        [StringLength(100)]        public string ProjectName { get; set; }
        
        /// <summary>        /// 名称        /// </summary>        [StringLength(100)]        public string GiftName { get; set; }
        
        /// <summary>        /// 库存        /// </summary>        [StringLength(100)]        public string Stock { get; set; }

        /// <summary>
        /// 单位
        /// </summary>        [StringLength(100)]        public string Unit { get; set; }

        /// <summary>
        /// 单价
        /// </summary>        [StringLength(100)]        public string Price { get; set; }    }}