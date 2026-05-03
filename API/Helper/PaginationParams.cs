namespace EmployeeLeaveAPI.Helper
{
    public class PaginationParams
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        private const int MaxPageSize = 50;
        public void Validate()
        {
            if (PageSize > MaxPageSize) PageSize = MaxPageSize;
            if (PageNumber < 1) PageNumber = 1;
        }
    }
}
