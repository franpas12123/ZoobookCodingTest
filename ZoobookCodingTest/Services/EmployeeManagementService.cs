using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using ZoobookCodingTest.Models;

namespace ZoobookCodingTest.Services
{
    public class EmployeeManagementService
    {
        // Get saved json
        public string employeeDataPath {
            get { return Path.Combine(Directory.GetCurrentDirectory(), "employee.json"); }
        }

        // Get employee; Change json to employee model
        public IEnumerable<Employee> GetEmployees()
        {
            // some query stuff to get some stuff in db...
            // serialize it and return it as IEnumerable<Employee>
            using (var jsonFileReader = File.OpenText(employeeDataPath))
            {
                return JsonSerializer.Deserialize<Employee[]>(jsonFileReader.ReadToEnd(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
        }

        public bool AddEmployee(string firstName="", string middleName="", string lastName="")
        {
            if(!string.IsNullOrEmpty(firstName) || !string.IsNullOrEmpty(middleName) || !string.IsNullOrEmpty(lastName))
            {
                IEnumerable<Employee> employees = GetEmployees();

                Employee newEmployee = new Employee();
                newEmployee.id = firstName + middleName + lastName;
                newEmployee.firstName = firstName;
                newEmployee.middleName = middleName;
                newEmployee.lastName = lastName;

                var employeesList = employees.ToList();
                employeesList.Add(newEmployee);

                employees = employeesList.ToArray();

                // simulating adding to db
                using (var outputStream = File.OpenWrite(employeeDataPath))
                {
                    JsonSerializer.Serialize<IEnumerable<Employee>>(
                        new Utf8JsonWriter(outputStream, new JsonWriterOptions
                        {
                            SkipValidation = true,
                            Indented = true
                        }),
                        employees
                    );
                }
                var result = true; // let's say it's a success
                return result;
            }
            return false;
        }

        public IEnumerable<Employee> UpdateEmployee(string id, string firstName="", string middleName="", string lastName="")
        {
            // simulate db query here
            // Check if that employee exist
            var employeesList = GetEmployees().ToList();
            int index = employeesList.FindIndex(employee => employee.id == id);

            if(!string.IsNullOrEmpty(firstName))
                employeesList[index].firstName = firstName;

            if (!string.IsNullOrEmpty(middleName))
                employeesList[index].middleName = middleName;

            if (!string.IsNullOrEmpty(lastName))
                employeesList[index].lastName = lastName;

            var updatedEmployeesList = employeesList.ToArray();

            using (var outputStream = File.OpenWrite(employeeDataPath))
            {
                JsonSerializer.Serialize<IEnumerable<Employee>>(
                    new Utf8JsonWriter(outputStream, new JsonWriterOptions
                    {
                        SkipValidation = true,
                        Indented = true
                    }),
                    updatedEmployeesList
                );
            }
            return updatedEmployeesList;
        }
        public IEnumerable<Employee> DeleteEmployee(string id)
        {
            // simulate db query here
            // Check if that employee exist
            var employeesList = GetEmployees().ToList();
            int index = employeesList.FindIndex(employee => employee.id == id);

            if(index >= 0)
            {
                employeesList.RemoveAt(index);
                var updatedEmployeesList = employeesList.ToArray();

                File.WriteAllText(employeeDataPath, string.Empty);
                using (var outputStream = File.OpenWrite(employeeDataPath))
                {
                    JsonSerializer.Serialize<IEnumerable<Employee>>(
                        new Utf8JsonWriter(outputStream, new JsonWriterOptions
                        {
                            SkipValidation = true,
                            Indented = true
                        }),
                        updatedEmployeesList
                    );
                }
                return updatedEmployeesList;
            }
            return GetEmployees();
        }

        // Change employee model back to json
    }
}
