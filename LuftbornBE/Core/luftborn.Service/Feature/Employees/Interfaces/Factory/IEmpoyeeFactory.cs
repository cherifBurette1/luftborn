using luftborn.Data.Entities;
using luftborn.DTO.Models;

namespace luftborn.Service.Features.Employees.Interfaces.Factories
{
    public interface IEmployeeFactory
    {
        public Employee CreateEmployeeDTO(DTOEmployee employee, int lastId);
        DTOEmployee CreateEmployeeListDTO(Employee employee);
        void UpdateEmployee(Employee employee, DTOEmployee model);
    }
}
