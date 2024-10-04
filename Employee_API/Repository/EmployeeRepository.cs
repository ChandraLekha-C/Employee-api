using Dapper;
using Employee_API.Models;
using Employee_API.Models.NewFolder;
using Employee_API.Repository;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly IConfiguration configuration;

    public EmployeeRepository(IConfiguration config)
    {
        configuration = config;
    }

    public IEnumerable<EmployeeDto> GetAllEmployees()
    {
        using (IDbConnection conn = new SqlConnection(configuration.GetConnectionString("Employee")))
        {
            string sql = "SELECT * FROM employees";
            return conn.Query<EmployeeDto>(sql).ToList();
        }
    }

    public EmployeeDto GetEmployeeById(int id)
    {
        using (IDbConnection conn = new SqlConnection(configuration.GetConnectionString("Employee")))
        {
            string sql = "SELECT * FROM employees WHERE Employee_Id = @id";
            return conn.Query<EmployeeDto>(sql, new { id }).FirstOrDefault();
        }
    }

    public void AddEmployee(EmployeeDto employeeDto)
    {
        using (IDbConnection conn = new SqlConnection(configuration.GetConnectionString("Employee")))
        {
            string sql = @"
                INSERT INTO employees (Employee_Id, Employee_Name, Age, Department_Id, Role)
                VALUES (@Employee_Id, @Employee_Name, @Age, @Department_Id, @Role)";

            conn.Execute(sql, employeeDto);
        }
    }

    public void UpdateEmployee(EmployeeDto employeeDto)
    {
        using (IDbConnection conn = new SqlConnection(configuration.GetConnectionString("Employee")))
        {
            string sql = @"
                UPDATE employees
                SET Employee_Name = @Employee_Name, 
                    Age = @Age,
                    Department_Id = @Department_Id,
                    Role = @Role
                WHERE Employee_Id = @Employee_Id";

            conn.Execute(sql, employeeDto);
        }
    }

    public void DeleteEmployee(int id)
    {
        using (IDbConnection conn = new SqlConnection(configuration.GetConnectionString("Employee")))
        {
            string sql = "DELETE FROM employees WHERE Employee_Id = @id";
            conn.Execute(sql, new { id });
        }
    }

    public void UpdatePartialEmployee(EmployeeDto employeeDto)
    {
        using (IDbConnection conn = new SqlConnection(configuration.GetConnectionString("Employee")))
        {
            var setClause = new List<string>();
            var parameters = new DynamicParameters();

            if (employeeDto.Employee_Name != null)
            {
                setClause.Add("Employee_Name = @Employee_Name");
                parameters.Add("Employee_Name", employeeDto.Employee_Name);
            }
            if (employeeDto.Age != null)
            {
                setClause.Add("Age = @Age");
                parameters.Add("Age", employeeDto.Age);
            }
            if (employeeDto.Department_Id != null)
            {
                setClause.Add("Department_Id = @Department_Id");
                parameters.Add("Department_Id", employeeDto.Department_Id);
            }
            if (employeeDto.Role != null)
            {
                setClause.Add("Role = @Role");
                parameters.Add("Role", employeeDto.Role);
            }

            if (setClause.Any())
            {
                var sql = $@"
                    UPDATE employees
                    SET {string.Join(", ", setClause)}
                    WHERE Employee_Id = @Employee_Id";

                parameters.Add("Employee_Id", employeeDto.Employee_Id);
                conn.Execute(sql, parameters);
            }
        }
    }
}
