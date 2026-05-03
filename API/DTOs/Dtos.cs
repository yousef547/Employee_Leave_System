namespace EmployeeLeaveAPI.DTOs
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public int EmployeeNumber { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public string? Qualification { get; set; }
        public int? NumberOfDays { get; set; }
    }

    public class CreateEmployeeDto
    {
        public int EmployeeNumber { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public string? Qualification { get; set; }
    }

    public class LeaveDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public int LeaveType { get; set; }
        public string LeaveTypeName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public int DurationDays { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class CreateLeaveDto
    {
        public int EmployeeId { get; set; }
        public int LeaveType { get; set; }
        public DateTime StartDate { get; set; }
        public int DurationDays { get; set; }
    }
}
