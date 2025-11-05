import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IDepartment } from '../interfaces/idepartment';

@Injectable({
  providedIn: 'root',
})
export class DepartmentService {
  baseURL = environment.baseUrl;

  constructor(private http: HttpClient) {}
  getDepartments(): Observable<IDepartment[]> {
    return this.http.get<IDepartment[]>(`${this.baseURL}/department`);
  }
}
