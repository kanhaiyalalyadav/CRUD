using CRUD.Models;

namespace CRUD.AppData
{
    public interface IEmpRepo
    {
        List<Department> GetDepartments();
        void AddEmployee(Employee emp);
        List<Employee> GetEmployees();
    }
}
