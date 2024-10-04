using Employee_API.Models.NewFolder;
using Employee_API.Repository;
using Microsoft.AspNetCore.JsonPatch;
using System;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public IEnumerable<EmployeeDto> GetAllEmployees()
    {
        return _employeeRepository.GetAllEmployees();
    }

    public EmployeeDto GetEmployeeById(int id)
    {
        return _employeeRepository.GetEmployeeById(id);
    }

    public void AddEmployee(EmployeeDto employeeDto)
    {
        _employeeRepository.AddEmployee(employeeDto);
    }

    public void UpdateEmployee(int id, EmployeeDto employeeDto)
    {
        if (id != employeeDto.Employee_Id)
        {
            throw new ArgumentException("ID mismatch");
        }

        var existingEmployee = _employeeRepository.GetEmployeeById(id);
        if (existingEmployee == null)
        {
            throw new KeyNotFoundException($"Employee with ID {id} not found.");
        }

        _employeeRepository.UpdateEmployee(employeeDto);
    }

    public void DeleteEmployee(int id)
    {
        var existingEmployee = _employeeRepository.GetEmployeeById(id);
        if (existingEmployee == null)
        {
            throw new KeyNotFoundException($"Employee with ID {id} not found.");
        }

        _employeeRepository.DeleteEmployee(id);
    }

    public void UpdatePartialEmployee(int id, JsonPatchDocument<EmployeeDto> patchDto)
    {
        var existingEmployee = _employeeRepository.GetEmployeeById(id);
        if (existingEmployee == null)
        {
            throw new KeyNotFoundException($"Employee with ID {id} not found.");
        }

        patchDto.ApplyTo(existingEmployee);
        _employeeRepository.UpdatePartialEmployee(existingEmployee);
    }

    //void IEmployeeService.UpdateEmployee(EmployeeDto employeeDto)
    //{
    //    throw new NotImplementedException();
    //}
}
