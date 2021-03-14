
using WebToanHoc.Models;
using System;

namespace WebToanHoc.EF.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
          DbContext_WebTaiLieu Init();
    }
}