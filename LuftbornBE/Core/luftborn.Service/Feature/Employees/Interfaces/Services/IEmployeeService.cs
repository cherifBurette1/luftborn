using luftborn.Service.Models;
using System.Threading.Tasks;
using luftborn.DTO.Models;
using System.Collections.Generic;

namespace luftborn.Service.Feature.Employees.Interfaces.Services
{
    public interface IEmployeeService
    {
        public Task<ServiceResultList<DTOEmployee>> GetEmployees();
        public Task<ServiceResultDetail<string>> CreateEmployee(DTOEmployee model);
        Task<ServiceResultDetail<string>> EditEmployee(DTOEmployee model);
        Task<ServiceResultDetail<string>> DeleteEmployee(int id);
    }
}