namespace WebToanHoc.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_file
    {
        [Key]
        public int id_doc { get; set; }

        [StringLength(200)]
        public string file_name { get; set; }

        public int? id_cate { get; set; }

        public string link_drive { get; set; }

        public int? status { get; set; }

        [StringLength(50)]
        public string time_up { get; set; }

        public string file_name_sha1 { get; set; }

        public int? num_view { get; set; }

        public int? num_down { get; set; }

        public int? id_user { get; set; }

        public string description { get; set; }

        public virtual tbl_category tbl_category { get; set; }
    }
}
