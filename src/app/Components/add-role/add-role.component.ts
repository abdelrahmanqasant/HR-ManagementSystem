import { Component, OnInit } from '@angular/core';
import { IRolePermission } from '../../interfaces/irole-permission';
import { RoleService } from '../../services/role.service';
import { ToastrModule, ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { IRole, IRoleName } from '../../interfaces/irole';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-add-role',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './add-role.component.html',
  styleUrls: ['./add-role.component.css'],
})
export class AddRoleComponent implements OnInit {
  allPermissions: IRolePermission;
  constructor(
    private roleService: RoleService,
    private toastr: ToastrService,
    private router: Router
  ) {
    this.allPermissions = {
      roleId: '',
      roleName: '',
      roleClaims: [
        {
          displayValue: 'Permissions.Employees.View',
          isSelected: false,
        },
        {
          displayValue: 'Permissions.Employees.Create',
          isSelected: false,
        },
        {
          displayValue: 'Permissions.Employees.Edit',
          isSelected: false,
        },
        {
          displayValue: 'Permissions.Employees.Delete',
          isSelected: false,
        },
        {
          displayValue: 'Permissions.Settings.View',
          isSelected: false,
        },
        {
          displayValue: 'Permissions.Settings.Create',
          isSelected: false,
        },
        {
          displayValue: 'Permissions.Settings.Edit',
          isSelected: false,
        },
        {
          displayValue: 'Permissions.Settings.Delete',
          isSelected: false,
        },
        {
          displayValue: 'Permissions.Attendance.View',
          isSelected: false,
        },
        {
          displayValue: 'Permissions.Attendance.Create',
          isSelected: false,
        },
        {
          displayValue: 'Permissions.Attendance.Edit',
          isSelected: false,
        },
        {
          displayValue: 'Permissions.Attendance.Delete',
          isSelected: false,
        },
        {
          displayValue: 'Permissions.Salary.View',
          isSelected: false,
        },
        {
          displayValue: 'Permissions.Salary.Create',
          isSelected: false,
        },
        {
          displayValue: 'Permissions.Salary.Edit',
          isSelected: false,
        },
        {
          displayValue: 'Permissions.Salary.Delete',
          isSelected: false,
        },
      ],
    };
  }
  ngOnInit(): void {}
  addRole(roleName: string) {
    let role = {} as IRoleName;
    role.name = roleName;
    this.roleService.addRole(role).subscribe((data: IRole) => {
      this.allPermissions.roleId = data.id;
      this.roleService.updateRolePermission(this.allPermissions).subscribe(
        (data) => {},
        (errors) => {}
      );
      this.router.navigate(['/roles']);
    });
  }
}
