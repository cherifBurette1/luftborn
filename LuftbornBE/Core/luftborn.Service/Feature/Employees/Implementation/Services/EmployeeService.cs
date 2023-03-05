using luftborn.Service.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using luftborn.DTO.Models;
using luftborn.Service.Features.Employees.Interfaces.Factories;
using luftborn.Service.Feature.Employees.Interfaces.Services;
using luftborn.Service.Features.Common.Interfaces;
using System.Collections.Generic;

namespace Qorrect.Service.Feature.Employee.Implementation.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly Lazy<IEmployeeFactory> _EmployeeFactory;
        private readonly Lazy<IUnitOfWork> _unitOfWork;
        public EmployeeService(Lazy<IEmployeeFactory> EmployeeFactory,
            Lazy<IUnitOfWork> unitOfWork,
            Lazy<IHttpClientFactory> clientFactory)
        {
            _EmployeeFactory = EmployeeFactory;
            _unitOfWork = unitOfWork;
        }
        private IUnitOfWork UnitOfWork => _unitOfWork.Value;
        private IEmployeeFactory EmployeeFactory => _EmployeeFactory.Value;

        public async Task<ServiceResultDetail<string>> CreateEmployee(DTOEmployee model)
        {
            luftborn.Data.Entities.Employee employeeDto;
            var isThereAnyEmployee = await UnitOfWork.EmployeeRepository.IsThereAnyEmployee();
            if (isThereAnyEmployee)
            {
                var lastId = await UnitOfWork.EmployeeRepository.GetLastIdEmployee();
                 employeeDto =EmployeeFactory.CreateEmployeeDTO(model, lastId);
            }
            else
            {
                 employeeDto = EmployeeFactory.CreateEmployeeDTO(model, 0);
            }
            await UnitOfWork.EmployeeRepository.Create(employeeDto);
            await UnitOfWork.SaveChangesAsync();

            return new ServiceResultDetail<string>()
            {
                IsValid = true,
                Model = "added successfully"
            };
        }
        public async Task<ServiceResultDetail<string>> DeleteEmployee(int id)
        {
            await UnitOfWork.EmployeeRepository.DeleteEmployee(id);

            return new ServiceResultDetail<string>()
            {
                IsValid = true,
                Model = "deleted successfully"
            };
        }
        public async Task<ServiceResultDetail<string>> EditEmployee(DTOEmployee model)
        {
            var employee = await UnitOfWork.EmployeeRepository.GetEmployee(int.Parse(model.Id));
            if (employee != null)
            {
                EmployeeFactory.UpdateEmployee(employee , model);
                await UnitOfWork.SaveChangesAsync(true);
            return new ServiceResultDetail<string>()
            {
                IsValid = true,
                Model = "updated successfully"
            };
            }
            else
            {
                return new ServiceResultDetail<string>()
                {
                    IsValid = false,
                    Model = "updating failed",
                    Errors = new List<string>()
                };
            }

        }
        public async Task<ServiceResultList<DTOEmployee>> GetEmployees()
        {
            var data = await UnitOfWork.EmployeeRepository.GetEmployeeList();
            var dtos = data.Select(x => EmployeeFactory.CreateEmployeeListDTO(x)).ToList();
            return new ServiceResultList<DTOEmployee>()
            {
                Model = dtos,
                IsValid = true
            };
        }
    }
}