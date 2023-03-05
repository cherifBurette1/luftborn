using Microsoft.AspNetCore.Mvc;
using luftborn.DTO.Models;
using System;
using System.Threading.Tasks;
using luftborn.Service.Feature.Employees.Interfaces.Services;
using luftborn.Data.Entities;
using Microsoft.AspNetCore.Authorization;

namespace luftborn.Api.Controllers
{
    public class EmployeeController : BaseController
    {
        private readonly Lazy<IEmployeeService> _employeeService;
        /// <summary>
        /// Academic-Level Constructor
        /// </summary>
        /// <param name="employeeService"></param>
        public EmployeeController(Lazy<IEmployeeService> employeeService)
        {
            _employeeService = employeeService;
        }
        private IEmployeeService EmployeeService => _employeeService.Value;
        /// <summary>
        /// save employee
        /// </summary>
        /// <returns>successs</returns>
        /// <param name="employee"></param>
        [AllowAnonymous]
        [HttpPost("employee", Name = "SaveEmployee")]
        public async Task<IActionResult> SaveEmployee(DTOEmployee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await EmployeeService.CreateEmployee(employee);
            return GetApiResponse(result);
        }
        /// <summary>
        /// gets list of employee
        /// </summary>
        /// <returns>list of employee</returns>
        [AllowAnonymous]
        [HttpGet("employees", Name = "GetEmployees")]
        public async Task<IActionResult> GetEmployees()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await EmployeeService.GetEmployees();
            return GetApiResponse(result);
        }
        /// <summary>
        /// delete employee
        /// </summary>
        /// <returns>list of employee</returns>
        /// <param name="id"></param>
        [AllowAnonymous]
        [HttpDelete("employee/{id}", Name = "DeleteEmployee")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await EmployeeService.DeleteEmployee(id);
            return GetApiResponse(result);
        }
        /// <summary>
        /// edit employee
        /// m
        /// </summary>
        /// <returns>list of employee</returns>
        /// <param name="model"></param>
        [AllowAnonymous] 
        [HttpPut("employee", Name = "EditEmployee")]
        public async Task<IActionResult> EditEmployee(DTOEmployee model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await EmployeeService.EditEmployee(model);
            return GetApiResponse(result);
        }
    }
}