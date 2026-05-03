import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Leave, CreateLeave } from '../models/models';

@Injectable({ providedIn: 'root' })
export class LeaveService {
  private apiUrl = 'https://localhost:59020/api/leaves';

  constructor(private http: HttpClient) {}

  getByEmployee(employeeId: number): Observable<Leave[]> {
    return this.http.get<Leave[]>(`${this.apiUrl}/employee/${employeeId}`);
  }

  create(dto: CreateLeave): Observable<Leave> {
    return this.http.post<Leave>(this.apiUrl, dto);
  }

  update(id: number, dto: CreateLeave): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, dto);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
