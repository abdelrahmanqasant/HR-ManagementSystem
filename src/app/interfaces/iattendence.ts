export interface IAttendence {
  empId: number;
  day: string;
  arrival: string | null;
  departure: string | null;
  status: number;
  empName: string;
  deptName: string;
}
