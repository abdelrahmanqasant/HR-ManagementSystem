import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IRole } from '../interfaces/irole';
import { IRolePermission } from '../interfaces/irole-permission';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class RoleService {
  private apiURL = environment.baseUrl;

  constructor(private http: HttpClient) {}

  getAllRoles(): Observable<IRole[]> {
    return this.http.get<IRole[]>(`${this.apiURL}/superadmin/AllRoles`);
  }

  getRoleById(roleId: string): Observable<IRole> {
    return this.http.get<IRole>(
      `${this.apiURL}/superadmin/GetRoleById?roleId=${roleId}`
    );
  }

  addRole(model: any): Observable<any> {
    return this.http.post(`${this.apiURL}/superadmin/AddRole`, model);
  }

  deleteRole(roleId: string): Observable<any> {
    return this.http.delete(
      `${this.apiURL}/superadmin/DeleteRole?roleId=${roleId}`
    );
  }

  getRolePermission(roleId: string): Observable<IRolePermission> {
    return this.http.get<IRolePermission>(
      `${this.apiURL}/superadmin/AllPermissions?roleId=${roleId}`
    );
  }

  updateRolePermission(model: IRolePermission): Observable<any> {
    return this.http.post(`${this.apiURL}/superadmin/AddPermission`, model);
  }

  updateRoleById(roleId: string, model: any): Observable<any> {
    return this.http.put(
      `${this.apiURL}/superadmin/UpdateRole?roleId=${roleId}`,
      model
    );
  }
}
