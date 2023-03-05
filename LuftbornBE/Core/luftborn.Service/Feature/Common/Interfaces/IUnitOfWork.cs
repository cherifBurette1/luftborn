using System.Threading.Tasks;
using luftborn.Service.Features.Common.Interfaces;
using luftborn.Service.Features.Employees.Interfaces.Repositories;
namespace luftborn.Service.Features.Common.Interfaces
{
    public interface IUnitOfWork
    {
        Task<bool> SaveChangesAsync(bool enableAuditLog = true);
        IDatabaseTransaction BeginTransaction();
        #region Repositories
        IEmployeeRepository EmployeeRepository { get; }
        #endregion
    }
}