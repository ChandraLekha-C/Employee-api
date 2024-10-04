using Employee_API.Models.NewFolder;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;

public interface IEmployeeService
{
    IEnumerable<EmployeeDto> GetAllEmployees();
    EmployeeDto GetEmployeeById(int id);
    void AddEmployee(EmployeeDto employeeDto);
    void UpdateEmployee(int id, EmployeeDto employeeDto);
    void DeleteEmployee(int id);
    void UpdatePartialEmployee(int id, JsonPatchDocument<EmployeeDto> patchDto);
}
