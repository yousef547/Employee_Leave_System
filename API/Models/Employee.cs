namespace EmployeeLeaveAPI.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public int EmployeeNumber { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public string? Qualification { get; set; }
        public ICollection<Leave> Leaves { get; set; } = new List<Leave>();
    }
}
