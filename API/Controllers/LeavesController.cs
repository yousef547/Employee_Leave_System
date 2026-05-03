using EmployeeLeaveAPI.Data;
using EmployeeLeaveAPI.DTOs;
using EmployeeLeaveAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLeaveAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeavesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LeavesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/leaves/employee/5
        [HttpGet("employee/{employeeId}")]
        public async Task<ActionResult<IEnumerable<LeaveDto>>> GetByEmployee(int employeeId)
        {
            var leaves = await _context.Leaves
                .Include(l => l.Employee)
                .Where(l => l.EmployeeId == employeeId)
                .Select(l => new LeaveDto
                {
                    Id = l.Id,
                    EmployeeId = l.EmployeeId,
                    EmployeeName = l.Employee!.Name,
                    LeaveType = (int)l.LeaveType,
                    LeaveTypeName = l.LeaveType.ToString(),
                    StartDate = l.StartDate,
                    DurationDays = l.DurationDays,
                    EndDate = l.StartDate.AddDays(l.DurationDays - 1)
                }).ToListAsync();

            return Ok(leaves);
        }

        // GET: api/leaves/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LeaveDto>> GetById(int id)
        {
            var leave = await _context.Leaves.Include(l => l.Employee).FirstOrDefaultAsync(l => l.Id == id);
            if (leave == null) return NotFound();

            return Ok(new LeaveDto
            {
                Id = leave.Id,
                EmployeeId = leave.EmployeeId,
                EmployeeName = leave.Employee!.Name,
                LeaveType = (int)leave.LeaveType,
                LeaveTypeName = leave.LeaveType.ToString(),
                StartDate = leave.StartDate,
                DurationDays = leave.DurationDays,
                EndDate = leave.StartDate.AddDays(leave.DurationDays - 1)
            });
        }

        // POST: api/leaves
        [HttpPost]
        public async Task<ActionResult<LeaveDto>> Create(CreateLeaveDto dto)
        {
            // Rule: Duration must be at least 30 days
            if (dto.DurationDays > 30 && dto.DurationDays < 1)
                return BadRequest("مدة الإجازة يجب ألا تقل عن 1 يوم ولا تذيد عن 30 يوم.");

            var endDate = dto.StartDate.AddDays(dto.DurationDays - 1);

            // Rule: No overlapping leaves for same employee
            var overlap = await _context.Leaves.AnyAsync(l =>
                l.EmployeeId == dto.EmployeeId &&
                dto.StartDate <= l.StartDate.AddDays(l.DurationDays - 1) &&
                endDate >= l.StartDate);

            if (overlap)
                return BadRequest("لا يمكن تسجيل إجازتين في نفس الفترة لنفس الموظف.");

            // Rule: Max 30 days per year per leave type
            var year = dto.StartDate.Year;
            var totalDaysThisYear = await _context.Leaves
                .Where(l =>
                    l.EmployeeId == dto.EmployeeId &&
                    l.LeaveType == (LeaveType)dto.LeaveType &&
                    l.StartDate.Year == year)
                .SumAsync(l => l.DurationDays);

            if (totalDaysThisYear + dto.DurationDays > 30)
                return BadRequest($"لا يمكن أن يتجاوز إجمالي الإجازة {30} يوم في السنة لنفس النوع.");

            var leave = new Leave
            {
                EmployeeId = dto.EmployeeId,
                LeaveType = (LeaveType)dto.LeaveType,
                StartDate = dto.StartDate,
                DurationDays = dto.DurationDays
            };

            _context.Leaves.Add(leave);
            await _context.SaveChangesAsync();

            var created = await _context.Leaves.Include(l => l.Employee).FirstAsync(l => l.Id == leave.Id);

            return CreatedAtAction(nameof(GetById), new { id = leave.Id }, new LeaveDto
            {
                Id = leave.Id,
                EmployeeId = leave.EmployeeId,
                EmployeeName = created.Employee!.Name,
                LeaveType = (int)leave.LeaveType,
                LeaveTypeName = leave.LeaveType.ToString(),
                StartDate = leave.StartDate,
                DurationDays = leave.DurationDays,
                EndDate = leave.StartDate.AddDays(leave.DurationDays - 1)
            });
        }

        // PUT: api/leaves/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreateLeaveDto dto)
        {
            var leave = await _context.Leaves.FindAsync(id);
            if (leave == null) return NotFound();

            if (dto.DurationDays > 30 && dto.DurationDays < 1)
                return BadRequest("مدة الإجازة يجب ألا تقل عن 1 يوم ولا تذيد عن 30 يوم.");

            var endDate = dto.StartDate.AddDays(dto.DurationDays - 1);

            var overlap = await _context.Leaves.AnyAsync(l =>
                l.EmployeeId == dto.EmployeeId &&
                l.Id != id &&
                dto.StartDate <= l.StartDate.AddDays(l.DurationDays - 1) &&
                endDate >= l.StartDate);

            if (overlap)
                return BadRequest("لا يمكن تسجيل إجازتين في نفس الفترة لنفس الموظف.");

            var year = dto.StartDate.Year;
            var totalDaysThisYear = await _context.Leaves
                .Where(l =>
                    l.EmployeeId == dto.EmployeeId &&
                    l.LeaveType == (LeaveType)dto.LeaveType &&
                    l.StartDate.Year == year &&
                    l.Id != id)
                .SumAsync(l => l.DurationDays);

            if (totalDaysThisYear + dto.DurationDays > 30)
                return BadRequest("لا يمكن أن يتجاوز إجمالي الإجازة 30 يوم في السنة لنفس النوع.");

            leave.LeaveType = (LeaveType)dto.LeaveType;
            leave.StartDate = dto.StartDate;
            leave.DurationDays = dto.DurationDays;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/leaves/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var leave = await _context.Leaves.FindAsync(id);
            if (leave == null) return NotFound();

            _context.Leaves.Remove(leave);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
