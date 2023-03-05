using luftborn.DTO.Models;
using luftborn.Service.Features.Employees.Interfaces.Factories;
using luftborn.Service.Features.Employees.Interfaces.Helpers;
using System;

namespace luftborn.Presistance.Features.Employee.Implementation.Factories
{
    public class EmployeeFactory : IEmployeeFactory
    {
        private readonly Lazy<IEmployeesHelper> _employeeHelper;
        public EmployeeFactory(Lazy<IEmployeesHelper> employeeHelper)
        {
            _employeeHelper = employeeHelper;
        }
        private IEmployeesHelper EmployeeHelper => _employeeHelper.Value;
        public Data.Entities.Employee CreateEmployeeDTO(DTOEmployee employee, int lastId)
        {
            return new Data.Entities.Employee()
            {
                Id = lastId + 1,
                Title = employee.Title,
                Description = employee.Description,
                Tags = employee.Tags,
                Category = employee.Category
            };
        }
        public DTOEmployee CreateEmployeeListDTO(Data.Entities.Employee employee)
        {
            return new DTOEmployee
            {
                Id = employee.Id.ToString(),
                Title = employee.Title,
                Description = employee.Description,
                Tags = employee.Tags,
                Category = employee.Category
            };
        }
        public void UpdateEmployee(Data.Entities.Employee employee, DTOEmployee model)
        {
            employee.Title = model.Title;
            employee.Description = model.Description;
            employee.Tags = model.Tags;
            employee.Category = model.Category;
        }
    }
}