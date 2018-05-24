namespace DingTalk.Models.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PurchaseProcedureInfo")]
    public partial class PurchaseProcedureInfo
    {
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal Id { get; set; }

        [StringLength(300)]
        public string DrawingNo { get; set; }

        [StringLength(300)]
        public string ProcedureInfoId { get; set; }

        [StringLength(300)]
        public string CreateManId { get; set; }

        [StringLength(300)]
        public string CreateTime { get; set; }

        [StringLength(300)]
        public string TaskId { get; set; }

        public bool? IsDown { get; set; }
    }
}
