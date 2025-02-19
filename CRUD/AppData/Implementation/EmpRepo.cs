using CRUD.Models;
using Microsoft.Data.SqlClient;
using System.IO;

namespace CRUD.AppData.Implementation
{
    public class EmpRepo : IEmpRepo
    {
        private readonly string Constr;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public EmpRepo(ConnectionHelper connection, IWebHostEnvironment webHostEnvironment)
        {
            Constr = connection.Default;
            _webHostEnvironment = webHostEnvironment;
        }
        public void AddEmployee(Employee emp)
        {
            string imagePath = null;
            if (emp.ImageFile != null)
            {
                string UploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(UploadsFolder))
                {
                    Directory.CreateDirectory(UploadsFolder);
                }
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + emp.ImageFile.FileName;
                string FilePath = Path.Combine(UploadsFolder, uniqueFileName);
                using(var fileStream=new FileStream(FilePath, FileMode.Create))
                {
                    emp.ImageFile.CopyTo(fileStream);
                    imagePath = "/uploads/" + uniqueFileName;
                }
                using(SqlConnection con=new SqlConnection(Constr))
                {
                    string sqlquery = "insert into Employee(Name,Email,DeptID,ProfileImage) values(@Name,@Email,@DeptID,@ProfileImage)";
                    SqlCommand cmd = new SqlCommand(sqlquery, con);
                    cmd.Parameters.AddWithValue("@Name", emp.Name);
                    cmd.Parameters.AddWithValue("@Email", emp.Email);
                    cmd.Parameters.AddWithValue("@DeptID", emp.DeptID);
                    cmd.Parameters.AddWithValue("@ProfileImage", (object)imagePath ?? DBNull.Value);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Department> GetDepartments()
        {
            List<Department> departments = new List<Department>();
            using (SqlConnection con = new SqlConnection(Constr))
            {
                SqlCommand cmd = new SqlCommand("Select * from Department", con);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    departments.Add(new Department
                    {
                        Id = Convert.ToInt32(dr["ID"]),
                        DeptName = dr["DeptName"].ToString()
                    });
                }
            }
            return departments;
        }

        public List<Employee> GetEmployees()
        {
            List<Employee> employees = new List<Employee>();
            using(SqlConnection con=new SqlConnection(Constr))
            {
                string squery = "select e.*,d.DeptName as DepartmentName from Employee e inner join Department d on e.DeptID=d.ID";
                SqlCommand cmd = new SqlCommand(squery, con);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    employees.Add(new Employee
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Name = dr["Name"].ToString(),
                        Email = dr["Email"].ToString(),
                        DeptID = Convert.ToInt32(dr["DeptID"]),
                        ProfileImage = dr["ProfileImage"].ToString(),
                        DeptName = dr["DepartmentName"].ToString()
                    });
                };
            }
            return employees;
        }
    }
}
