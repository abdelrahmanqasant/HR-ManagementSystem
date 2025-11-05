import { Component, OnInit } from '@angular/core';
import { IRole } from '../../interfaces/irole';
import { IRolePermission } from '../../interfaces/irole-permission';
import { RoleService } from '../../services/role.service';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-update-role-permissions',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, RouterModule],
  templateUrl: './update-Permissions.component.html',
  styleUrls: ['./update-Permissions.component.css'],
})
export class updatePermissionsComponent implements OnInit {
  roleId!: string;
  selectedRoleName!: string;
  allPermissions!: IRolePermission;

  constructor(
    private roleService: RoleService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.roleId = this.route.snapshot.paramMap.get('id')!;
    this.getRolePermission(this.roleId);
  }

  getRolePermission(roleID: string) {
    this.roleService.getRolePermission(roleID).subscribe({
      next: (res: IRolePermission) => {
        this.allPermissions = res;
        this.selectedRoleName = res.roleName;
      },
      error: () => {},
    });
  }

  updateRolePermission() {
    this.roleService.updateRolePermission(this.allPermissions).subscribe({
      next: () => {
        this.router.navigate(['/roles']);
      },
      error: () => [],
    });
  }
}
