namespace CRUD.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int DeptID { get; set; }
        public string ? DeptName { get; set; }
        public string ? ProfileImage { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
