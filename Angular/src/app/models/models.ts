export interface Employee {
  id: number;
  employeeNumber: number;
  name: string;
  dateOfBirth?: string;
  qualification?: string;
  numberOfDays?:number;
}


export interface PagedResult<T> {
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  data: T[];
}
export interface CreateEmployee {
  employeeNumber: number;
  name: string;
  dateOfBirth?: string;
  qualification?: string;
}

export interface Leave {
  id: number;
  employeeId: number;
  employeeName: string;
  leaveType: number;
  leaveTypeName: string;
  startDate: string;
  durationDays: number;
  endDate: string;
}

export interface CreateLeave {
  employeeId: number;
  leaveType: number;
  startDate: string;
  durationDays: number;
}

export const LeaveTypes = [
  { id: 1, name: 'سنوية' },
  { id: 2, name: 'مرضية' },
  { id: 3, name: 'طارئة' },
  { id: 4, name: 'بدون أجر' }
];
