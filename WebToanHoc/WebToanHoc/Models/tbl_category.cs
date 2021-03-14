namespace WebToanHoc.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_category
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_category()
        {
            tbl_file = new HashSet<tbl_file>();
        }

        [Key]
        public int id_cate { get; set; }

        [StringLength(50)]
        public string cate_name { get; set; }

        public int? id_subject { get; set; }

        public virtual tbl_subject tbl_subject { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_file> tbl_file { get; set; }
    }
}
