import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Employee, Leave, LeaveTypes } from '../../models/models';
import { EmployeeService } from '../../services/employee.service';
import { LeaveService } from '../../services/leave.service';
import { HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-leave-management',
  templateUrl: './leave-management.component.html'
})
export class LeaveManagementComponent implements OnInit {
  employee?: Employee;
  leaves: Leave[] = [];
  leaveTypes = LeaveTypes;
  form!: FormGroup;
  editingLeaveId?: number;
  errorMsg = '';
  successMsg = '';
  loading = false;
  employeeId!: number;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder,
    private employeeService: EmployeeService,
    private leaveService: LeaveService
  ) { }

  ngOnInit(): void {

    this.employeeId = +this.route.snapshot.paramMap.get('id')!;
    this.initForm();
    this.loadEmployee();
    this.loadLeaves();
  }


  getleaveTypes(leaveTypesId: number) {
    return this.leaveTypes.find(x => x.id == leaveTypesId)?.name
  }
  initForm(): void {
    this.form = this.fb.group({
      leaveType: [null, Validators.required],
      startDate: ['', Validators.required],
      durationDays: [1, [Validators.required, Validators.min(1), Validators.max(30)]]
    });
  }

  loadEmployee(): void {
    this.employeeService.getById(this.employeeId).subscribe({
      next: (e) => this.employee = e,
      error: () => this.errorMsg = 'لم يتم العثور على الموظف'
    });
  }

  loadLeaves(): void {
    this.leaveService.getByEmployee(this.employeeId).subscribe({
      next: (data) => this.leaves = data,
      error: () => this.errorMsg = 'حدث خطأ في تحميل الإجازات'
    });
  }

  saveLeave(): void {
    if (this.form.invalid) return;

    this.loading = true;
    this.errorMsg = '';
    this.successMsg = '';

    const dto = { ...this.form.value, employeeId: this.employeeId };

    const action: Observable<any> = this.editingLeaveId
      ? this.leaveService.update(this.editingLeaveId, dto)
      : this.leaveService.create(dto);

    action.subscribe({
      next: () => {
        this.successMsg = 'تم الحفظ بنجاح';
        this.loading = false;
        this.editingLeaveId = undefined;
        this.initForm();
        this.loadLeaves();
      },
      error: (err: HttpErrorResponse) => {
        this.errorMsg = err?.error?.message || err?.error || 'حدث خطأ أثناء الحفظ';
        this.loading = false;
      }
    });
  }

  editLeave(leave: Leave): void {
    this.editingLeaveId = leave.id;
    this.form.patchValue({
      leaveType: leave.leaveType,
      startDate: leave.startDate.substring(0, 10),
      durationDays: leave.durationDays
    });
    this.errorMsg = '';
    this.successMsg = '';
  }

  deleteLeave(id: number): void {
    if (confirm('هل أنت متأكد من حذف هذه الإجازة؟')) {
      this.leaveService.delete(id).subscribe({
        next: () => { this.successMsg = 'تم الحذف بنجاح'; this.loadLeaves(); },
        error: () => this.errorMsg = 'حدث خطأ أثناء الحذف'
      });
    }
  }

  cancelEdit(): void {
    this.editingLeaveId = undefined;
    this.initForm();
    this.errorMsg = '';
    this.successMsg = '';
  }

  print(): void {
    window.print();
  }

  back(): void {
    this.router.navigate(['/employees']);
  }
}
