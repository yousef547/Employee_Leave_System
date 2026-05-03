namespace EmployeeLeaveAPI.Models
{
    public enum LeaveType
    {
        Annual = 1,
        Sick = 2,
        Emergency = 3,
        Unpaid = 4
    }

    public class Leave
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public LeaveType LeaveType { get; set; }
        public DateTime StartDate { get; set; }
        public int DurationDays { get; set; }
        public DateTime EndDate => StartDate.AddDays(DurationDays - 1);
    }
}
