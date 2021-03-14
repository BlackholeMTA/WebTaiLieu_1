namespace WebToanHoc.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_category
    {
        [Key]
        public int id_cate { get; set; }

        [StringLength(50)]
        public string cate_name { get; set; }

        public int? id_subject { get; set; }

        public virtual tbl_subject tbl_subject { get; set; }
    }
}
