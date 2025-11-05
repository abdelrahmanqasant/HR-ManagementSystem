import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { IOrganizationSettings } from '../interfaces/iorganization-settings';

@Injectable({
  providedIn: 'root'
})
export class SettingsService {
private baseURL = environment.baseUrl;

  constructor(private http :HttpClient) { }
  getOrganization():Observable<any>{
   return   this.http.get<any>(`${this.baseURL}/Organization`)

  }
  updateOrganization(settings:IOrganizationSettings):Observable<any>{
    return this.http.put<any>(`${this.baseURL}/Organization`,settings)
  }
}
