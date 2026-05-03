using EmployeeLeaveAPI.Data;
using EmployeeLeaveAPI.DTOs;
using EmployeeLeaveAPI.Helper;
using EmployeeLeaveAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLeaveAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmployeesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAll([FromQuery] PaginationParams param)
        {
            param.Validate();

            var query = _context.Employees
                .Include(x => x.Leaves)
                .Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    EmployeeNumber = e.EmployeeNumber,
                    Name = e.Name,
                    DateOfBirth = e.DateOfBirth,
                    Qualification = e.Qualification,
                    NumberOfDays = e.Leaves.Sum(l => (int?)l.DurationDays) ?? 0
                });

            var totalCount = await query.CountAsync();

            var data = await query
                .Skip((param.PageNumber - 1) * param.PageSize)
                .Take(param.PageSize)
                .ToListAsync();

            return Ok(new
            {
                TotalCount = totalCount,
                PageNumber = param.PageNumber,
                PageSize = param.PageSize,
                Data = data
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDto>> GetById(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound();

            return Ok(new EmployeeDto
            {
                Id = employee.Id,
                EmployeeNumber = employee.EmployeeNumber,
                Name = employee.Name,
                DateOfBirth = employee.DateOfBirth,
                Qualification = employee.Qualification
            });
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeDto>> Create(CreateEmployeeDto dto)
        {
            var exists = await _context.Employees
                .AnyAsync(e => e.EmployeeNumber == dto.EmployeeNumber);

            if (exists)
                return BadRequest("رقم الموظف موجود بالفعل.");

            var employee = new Employee
            {
                EmployeeNumber = dto.EmployeeNumber,
                Name = dto.Name,
                DateOfBirth = dto.DateOfBirth,
                Qualification = dto.Qualification
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = employee.Id }, new EmployeeDto
            {
                Id = employee.Id,
                EmployeeNumber = employee.EmployeeNumber,
                Name = employee.Name,
                DateOfBirth = employee.DateOfBirth,
                Qualification = employee.Qualification
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreateEmployeeDto dto)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound();

            var exists = await _context.Employees
                .AnyAsync(e => e.EmployeeNumber == dto.EmployeeNumber && e.Id != id);
            if (exists)
                return BadRequest("رقم الموظف موجود بالفعل.");

            employee.EmployeeNumber = dto.EmployeeNumber;
            employee.Name = dto.Name;
            employee.DateOfBirth = dto.DateOfBirth;
            employee.Qualification = dto.Qualification;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound();

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
