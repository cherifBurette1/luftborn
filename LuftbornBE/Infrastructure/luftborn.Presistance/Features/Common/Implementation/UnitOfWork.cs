using System;
using System.Threading.Tasks;
using luftborn.Presistance.Features.Common.Implementation;
using luftborn.Service.Features.Common.Interfaces;
using luftborn.Service.Features.Employees.Interfaces.Repositories;

namespace luftborn.Presistance.Features.Common.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        #region private fields
        private readonly IluftbornEntities _context;
        private readonly Lazy<IEmployeeRepository> employeeRepository;
        #endregion

        #region constructor
        public UnitOfWork(IluftbornEntities context,
            Lazy<IEmployeeRepository> employeeRepository
            )
        {
            _context = context;
            this.employeeRepository = employeeRepository;
        }

        #endregion

        #region Properties
        private IluftbornEntities LuftbornEntities => _context;
        public IEmployeeRepository EmployeeRepository => employeeRepository.Value;

        #endregion
        #region methods

        public async Task<bool> SaveChangesAsync(bool enableAuditLog = true)
        {
            return await LuftbornEntities.SaveChangesAsync(enableAuditLog);
        }
        public IDatabaseTransaction BeginTransaction()
        {
            return new DatabaseTransaction(LuftbornEntities);
        }
        #endregion
    }
}
