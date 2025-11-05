import { Routes } from '@angular/router';
import { SignInComponent } from './Components/sign-in/sign-in.component';
import { authGuard } from './_guards/auth.guard';
import { HomeComponent } from './Components/home/home.component';
import { preventloginGuard } from './_guards/prevent-login.guard';
import { UsersComponent } from './Components/users/users.component';
import { AttendenceReportComponent } from './Components/attendence-report/attendence-report.component';
import { SalaryReportComponent } from './Components/salary-report/salary-report.component';
import { OfficialDaysComponent } from './Components/official-days/official-days.component';
import { AddEmployeeComponent } from './Components/add-employee/add-employee.component';
import { OrganizationSettingsComponent } from './Components/organization-settings/organization-settings.component';
import { UpdateEmployeeComponent } from './Components/update-employee/update-employee.component';
import { DisplayEmployeeComponent } from './Components/display-employee/display-employee.component';
import { AddAttendenceComponent } from './Components/add-attendence/add-attendence.component';
import { RolesComponent } from './Components/roles/roles.component';
import { DisplayAttendencesComponent } from './Components/display-attendences/display-attendences.component';
import { AddUserComponent } from './Components/AddUser/AddUser.component';
import { AddRoleComponent } from './Components/add-role/add-role.component';
import { updatePermissionsComponent } from './Components/update-Permissions/update-Permissions.component';
import { UpdateRoleComponent } from './Components/update-role/update-role.component';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  {
    path: 'login',
    component: SignInComponent,
    canActivate: [preventloginGuard],
  },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [authGuard],
    children: [
      { path: 'home', component: HomeComponent },
      { path: 'users', component: UsersComponent, canActivate: [authGuard] },
      {
        path: 'roles/update/:id',
        component: UpdateRoleComponent,
      },

      {
        path: 'users/add',
        component: AddUserComponent,
        canActivate: [authGuard],
      },

      { path: 'attendance/report', component: AttendenceReportComponent },
      { path: 'salary/report', component: SalaryReportComponent },
      { path: 'daysOff', component: OfficialDaysComponent },
      { path: 'Employee/add', component: AddEmployeeComponent },
      { path: 'settings', component: OrganizationSettingsComponent },
      { path: 'Employee/update', component: UpdateEmployeeComponent },
      { path: 'Employee/display', component: DisplayEmployeeComponent },
      { path: 'attendance/add', component: AddAttendenceComponent },
      { path: 'display-attendences', component: DisplayAttendencesComponent },
      { path: 'roles', component: RolesComponent },
      { path: 'roles/add', component: AddRoleComponent },

      { path: 'permissions/update', component: updatePermissionsComponent },
    ],
  },
];
