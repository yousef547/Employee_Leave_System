import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Employee, CreateEmployee, PagedResult } from '../models/models';

@Injectable({ providedIn: 'root' })
export class EmployeeService {
  private apiUrl = 'https://localhost:59020/api/employees';

  constructor(private http: HttpClient) {}

  getAll(pageNumber: number, pageSize: number) {
  return this.http.get<PagedResult<Employee>>(
    `${this.apiUrl}?pageNumber=${pageNumber}&pageSize=${pageSize}`
  );
  }

  getById(id: number): Observable<Employee> {
    return this.http.get<Employee>(`${this.apiUrl}/${id}`);
  }

  create(dto: CreateEmployee): Observable<Employee> {
    return this.http.post<Employee>(this.apiUrl, dto);
  }

  update(id: number, dto: CreateEmployee): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, dto);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
