using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using luftborn.Data.Entities;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
namespace luftborn.Service.Features.Common.Interfaces
{
    public interface IluftbornEntities : IDisposable
    {
        DbSet<T> Set<T>() where T : class;
        bool SaveChanges(bool enableAuditLog = true);
        Task<bool> SaveChangesAsync(bool enableAuditLog = true, CancellationToken cancellationToken = default);
        bool GetLazyLoadingEnabledFlag();
        Task ExecuteQuery(string queryString);
        IDbConnection GetDbContextConnection();
        DatabaseFacade Database { get; }

        #region Tables
        DbSet<Employee> Employees { get; set; }
        #endregion

    }
}
