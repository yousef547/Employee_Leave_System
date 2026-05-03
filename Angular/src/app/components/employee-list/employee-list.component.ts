import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Employee } from '../../models/models';
import { EmployeeService } from '../../services/employee.service';

@Component({
  selector: 'app-employee-list',
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.css']
})
export class EmployeeListComponent implements OnInit {
  employees: Employee[] = [];
  loading = false;
  errorMsg = '';
  pageNumber = 1;
  pageSize = 5;
  totalCount = 0;
  totalPages = 0;
  constructor(private employeeService: EmployeeService, private router: Router) { }

  ngOnInit(): void {
    this.loadEmployees();
  }

  loadEmployees(): void {
    this.loading = true;
    this.employeeService.getAll(this.pageNumber, this.pageSize).subscribe({
      next: (res) => {
        this.employees = res.data; this.loading = false; this.totalCount = res.totalCount;
        this.totalPages = Math.ceil(this.totalCount / this.pageSize);
      },
      error: () => { this.errorMsg = 'حدث خطأ في تحميل البيانات'; this.loading = false; }
    });
  }

  nextPage() {
    if (this.pageNumber * this.pageSize < this.totalCount) {
      this.pageNumber++;
      this.loadEmployees();
    }
  }

  prevPage() {
    if (this.pageNumber > 1) {
      this.pageNumber--;
      this.loadEmployees();
    }
  }


  addEmployee(): void {
    this.router.navigate(['/employees/new']);
  }

  editEmployee(id: number): void {
    this.router.navigate(['/employees/edit', id]);
  }

  viewLeaves(id: number): void {
    this.router.navigate(['/employees', id, 'leaves']);
  }

  deleteEmployee(id: number): void {
    if (confirm('هل أنت متأكد من حذف هذا الموظف؟')) {
      this.employeeService.delete(id).subscribe({
        next: () => this.loadEmployees(),
        error: () => this.errorMsg = 'حدث خطأ أثناء الحذف'
      });
    }
  }
}
