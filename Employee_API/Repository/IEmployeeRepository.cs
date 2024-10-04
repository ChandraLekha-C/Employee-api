using Employee_API.Models.NewFolder;
using System.Collections.Generic;

namespace Employee_API.Repository
{
    public interface IEmployeeRepository
    {
        IEnumerable<EmployeeDto> GetAllEmployees();
        EmployeeDto GetEmployeeById(int id);
        void AddEmployee(EmployeeDto employeeDto);
        void UpdateEmployee(EmployeeDto employeeDto);
        void DeleteEmployee(int id);
        void UpdatePartialEmployee(EmployeeDto employeeDto);
    }

}