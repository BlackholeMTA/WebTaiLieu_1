using WebToanHoc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebToanHoc.EF.Infrastructure
{
     public class DbFactory : IDbFactory
     {
          private DbContext_WebTaiLieu dbContext;

          public void Dispose()
          {
               if (dbContext != null)
                    dbContext.Dispose();
          }

          public DbContext_WebTaiLieu Init()
          {
               return dbContext ?? (dbContext = new DbContext_WebTaiLieu());
          }
     }
}