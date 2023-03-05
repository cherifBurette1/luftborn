using luftborn.Service.Features.Common.Interfaces;
using luftborn.Service.Features.Employees.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace luftborn.Presistance.Features.Employee.Implementation.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IluftbornEntities _luftbornEntities;
        public EmployeeRepository(IluftbornEntities luftbornEntities)
        {
            _luftbornEntities = luftbornEntities;
        }

        public async Task<List<Data.Entities.Employee>> GetEmployeeList()
        {
            return await _luftbornEntities.Employees.ToListAsync();
        }
        public async Task<int> GetLastIdEmployee()
        {
            return await _luftbornEntities.Employees.CountAsync();
        }
        public async Task<bool> IsThereAnyEmployee()
        {
            return await _luftbornEntities.Employees.AnyAsync();
        }
        public async Task DeleteEmployee(int id)
        {
            string query = $"DELETE FROM Employees where Id = {id} ;";
            await _luftbornEntities.ExecuteQuery(query);
        }
        public async Task Create(Data.Entities.Employee employee)
        {
            await _luftbornEntities.Set<Data.Entities.Employee>().AddAsync(employee);
        }
        public async Task<Data.Entities.Employee> GetEmployee(int id)
        {
           return await _luftbornEntities.Employees.Where(x => x.Id == id).FirstOrDefaultAsync();
        }
    }
}
