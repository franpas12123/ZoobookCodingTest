using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZoobookCodingTest.Models;
using ZoobookCodingTest.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ZoobookCodingTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        // Have something to connect to db
        // the supposed code for getting db -_-
        // EmployeeControllerDataContext employeeData = new EmployeeControllerDataContext();
        EmployeeManagementService employeeManagementService;

        public EmployeeController(EmployeeManagementService employeeManagementService)
        {
            this.employeeManagementService = employeeManagementService;
        }

        // GET: api/<EmployeeController>
        [HttpGet]
        public IEnumerable<Employee> Get()
        {
            return employeeManagementService.GetEmployees();
        }

        // GET api/<EmployeeController>/5
        [HttpGet("{id}")]
        public Employee Get(string id)
        {
            
            IEnumerable <Employee> employees = employeeManagementService.GetEmployees();
            var result = employees.FirstOrDefault(employee => employee.id == id);

            if(result != null)
            {
                return result;
            } else
            {
                // return null for now. frontend will check if the result is null xD
                return null;
            }
        }

        // POST api/<EmployeeController>
        [HttpPost]
        public IEnumerable<Employee> Post([FromQuery] string firstName, [FromQuery] string middleName, [FromQuery] string lastName)
        {
            var result = employeeManagementService.AddEmployee(firstName, middleName, lastName);

            if(result)
            {
                // code to return if success
            } else
            {
                // code to return if unsuccessful
            }
            return employeeManagementService.GetEmployees();
        }

        // PUT api/<EmployeeController>/5
        [HttpPut("{id}")]
        public IEnumerable<Employee> Put(string id, [FromQuery] string firstName, [FromQuery] string middleName, [FromQuery] string lastName)
        {
            // This code should check if adding is sucess. For now, I'll just return the updated employee
            /*if (true)
            {
                return Ok();
            }
            return BadRequest();*/

            return employeeManagementService.UpdateEmployee(id, firstName, lastName, middleName);
        }

        // DELETE api/<EmployeeController>/5
        [HttpDelete("{id}")]
        public IEnumerable<Employee> Delete(string id)
        {
            return employeeManagementService.DeleteEmployee(id);
            // I'd like to return the status, but for this scenario, I'll just return the updated employee
            /*if (result != null)
            {
                if (employeeManagementService.DeleteEmployee(id))
                {
                    return Ok();
                }
                return BadRequest();
            }
            else
            {
                return NotFound();
            }*/
        }
    }
}
