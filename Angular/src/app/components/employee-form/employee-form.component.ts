import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { EmployeeService } from '../../services/employee.service';
import { HttpErrorResponse } from '@angular/common/http';
import { Observable, finalize } from 'rxjs';

@Component({
  selector: 'app-employee-form',
  templateUrl: './employee-form.component.html'
})
export class EmployeeFormComponent implements OnInit {
  form!: FormGroup;
  isEdit = false;
  employeeId?: number;
  errorMsg = '';
  loading = false;

  qualifications = ['دبلوم', 'بكالوريوس', 'ماجستير', 'دكتوراه', 'ثانوية عامة'];

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private employeeService: EmployeeService
  ) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      employeeNumber: [null, [Validators.required]],
      name: ['', [Validators.required]],
      dateOfBirth: [null],
      qualification: [null]
    });

    const id = this.route.snapshot.paramMap.get('id');
    if (id && id !== 'new') {
      this.isEdit = true;
      this.employeeId = +id;
      this.loadEmployee(this.employeeId);
    }
  }

  loadEmployee(id: number): void {
    this.employeeService.getById(id).subscribe({
      next: (emp) => {
        this.form.patchValue({
          employeeNumber: emp.employeeNumber,
          name: emp.name,
          dateOfBirth: emp.dateOfBirth ? emp.dateOfBirth.substring(0, 10) : null,
          qualification: emp.qualification
        });
      },
      error: () => this.errorMsg = 'حدث خطأ في تحميل البيانات'
    });
  }

 save(): void {
  if (this.form.invalid) return;

  this.loading = true;
  this.errorMsg = '';

  const dto = this.form.value;

  const action: Observable<any> = this.isEdit
    ? this.employeeService.update(this.employeeId!, dto)
    : this.employeeService.create(dto);

  action
    .pipe(finalize(() => (this.loading = false)))
    .subscribe({
      next: () => this.router.navigate(['/employees']),
      error: (err: HttpErrorResponse) => {
        this.errorMsg = err?.error?.message || err?.error || 'حدث خطأ أثناء الحفظ';
      }
    });
}

  cancel(): void {
    this.router.navigate(['/employees']);
  }
}
