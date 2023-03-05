
using luftborn.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace luftborn.Service.Features.Employees.Interfaces.Repositories
{
    public interface IEmployeeRepository
    {
        Task Create(Employee employee);
        Task DeleteEmployee(int id);
        Task<Employee> GetEmployee(int id);
        Task<List<Employee>> GetEmployeeList();
        Task<int> GetLastIdEmployee();
        Task<bool> IsThereAnyEmployee();
    }
}
