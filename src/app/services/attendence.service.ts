import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BehaviorSubject, Observable, map } from 'rxjs';
import { environment } from '../../environments/environment';
import { IAttendence } from '../interfaces/iattendence';
import { UserParams } from '../models/userParams';
import { PaginationResult } from '../interfaces/pagination';

@Injectable({
  providedIn: 'root',
})
export class AttendenceService {
  userParams: UserParams = new UserParams();
  baseURL = environment.baseUrl;
  paginatedResult: PaginationResult<IAttendence[]> = new PaginationResult();

  constructor(private http: HttpClient) {}


  addAttendence(attendence: IAttendence): Observable<IAttendence> {
    const payload = {
      empId: attendence.empId,
      day: attendence.day,
      arrival: attendence.arrival,
      departure: attendence.departure, 
      status: attendence.status,
      deptName: attendence.deptName,
      empName: attendence.empName,
    };

    return this.http.post<IAttendence>(`${this.baseURL}/Attendence`, payload);
  }


  getAll(userParams: UserParams): Observable<PaginationResult<IAttendence[]>> {
    let params = new HttpParams();

    if (userParams.pageNumber)
      params = params.append('pageNumber', userParams.pageNumber.toString());
    if (userParams.pageSize)
      params = params.append('pageSize', userParams.pageSize.toString());
    if (userParams.startDate)
      params = params.append('startDate', userParams.startDate);
    if (userParams.endDate)
      params = params.append('endDate', userParams.endDate);
    if (userParams.queryString?.trim())
      params = params.append('stringQuery', userParams.queryString.trim());

    return this.http
      .get<IAttendence[]>(`${this.baseURL}/Attendence`, {
        observe: 'response',
        params,
      })
      .pipe(
        map((response) => {
          if (response.body) this.paginatedResult.result = response.body;

          const paginationHeader = response.headers.get('Pagination');
          if (paginationHeader)
            this.paginatedResult.Pagination = JSON.parse(paginationHeader);

          return this.paginatedResult;
        })
      );
  }

  private refreshSubject = new BehaviorSubject<void>(undefined);
  refresh$ = this.refreshSubject.asObservable();

  refreshData() {
    this.refreshSubject.next(undefined);
  }


  deleteRecord(id: number, date: string): Observable<IAttendence> {
    return this.http.delete<IAttendence>(
      `${this.baseURL}/Attendence/${id}?date=${date}`
    );
  }


  resetUserParams() {
    this.userParams = new UserParams();
  }
}
