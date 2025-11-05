import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class DaysOffService {
  private baseURL = environment.baseUrl;

  constructor(private http: HttpClient) {}

  addDayOff(dayOff: any): Observable<any> {
    const payload = {
      name: dayOff.name,
      date: new Date(dayOff.date).toISOString().substring(0, 10),
    };
    return this.http.post<any>(`${this.baseURL}/DaysOff`, payload);
  }

  getAllDaysOff(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseURL}/DaysOff`);
  }

  deleteDayOff(day: string | Date): Observable<any> {
    const dayStr =
      day instanceof Date ? day.toISOString().substring(0, 10) : day;
    return this.http.delete<any>(`${this.baseURL}/DaysOff/${dayStr}`);
  }

  updateDayOff(dayOff: any): Observable<any> {
    const payload = {
      name: dayOff.name,
      date: new Date(dayOff.date).toISOString().substring(0, 10), 
    };
    return this.http.post<any>(
      `${this.baseURL}/DaysOff/${payload.date}`,
      payload
    );
  }
}
