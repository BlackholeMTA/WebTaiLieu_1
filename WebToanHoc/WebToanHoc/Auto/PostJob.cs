using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Quartz;
using WebToanHoc.Models;


namespace WebToanHoc.Auto
{
    public class PostJob : IJob
    {
        public PostJob()
        {

        }
        //public async Task Execute(IJobExecutionContext context)
        public async Task Execute(IJobExecutionContext context)
        {
            //Lấy thời gian
            DateTime aDateTime = DateTime.Now;
            string date = "";
            string day = aDateTime.Day.ToString();
            string month = aDateTime.Month.ToString();
            string year = aDateTime.Year.ToString();
            date = day + '-' + month + '-' + year;

            // job thay đổi status của tài liệu thành active và thêm ngày
            DbContext_WebTaiLieu db = new DbContext_WebTaiLieu();
            var list_docu = db.tbl_file.Where(x => x.status == 0).ToList();
            int number_docu = list_docu.Count();
            Random number_page = new Random();
            int num_page= number_page.Next(2, 3);
            for(int i=0;i<num_page;i++)
            {
                Random list_number = new Random();
               int num= list_number.Next(0, number_docu-1);
                tbl_file new_file = list_docu[num];
                new_file.status = 1;
                new_file.time_up = date;
                db.SaveChanges();
                
            }

            
            
        }

        //Task IJob.Execute(IJobExecutionContext context)
        //{
        //    throw new NotImplementedException();
        //}
    }
}