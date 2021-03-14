using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebToanHoc.EF.Model
{
    public class list_doc
    {
        public int id_doc { get; set; }
        public string file_name { get; set; }

        public string cate_name { get; set; }

        public string time_up { get; set; }

       public list_doc(int id_doc,string file_name, string cate_name,string time_up )
        {
            this.id_doc = id_doc;this.file_name = file_name;this.cate_name = cate_name; this.time_up = time_up;
        }
    }
}