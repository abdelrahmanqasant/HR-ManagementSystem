import { Component, OnInit, TemplateRef } from '@angular/core';
import { IRole } from '../../interfaces/irole';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { RoleService } from '../../services/role.service';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-roles',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './roles.component.html',
  styleUrls: ['./roles.component.css'],
})
export class RolesComponent implements OnInit {
  allUserRoles: IRole[] = [];
  modalRef!: BsModalRef;
  toDeleteRole = '';

  constructor(
    private roleService: RoleService,
    private router: Router,
    private modalService: BsModalService
  ) {}

  ngOnInit(): void {
    this.getAllRoles();
  }

  getAllRoles() {
    this.roleService.getAllRoles().subscribe({
      next: (roles: IRole[]) => {
        this.allUserRoles = roles;
      },
      error: (err: any) => console.error('Failed to load roles:', err),
    });
  }

  addRole() {
    this.router.navigate(['roles/add']);
  }

  deleteRole(roleId: string) {
    this.modalRef?.hide();
    this.roleService.deleteRole(roleId).subscribe({
      next: () => {
        this.allUserRoles = this.allUserRoles.filter(
          (role) => role.id !== roleId
        );
      },
      error: (err) => console.error('Failed to delete role:', err),
    });
  }

  decline(): void {
    this.modalRef?.hide();
  }

  openModal(template: TemplateRef<void>, role: string) {
    this.toDeleteRole = role;
    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
  }

  updateRole(role: IRole) {
    this.router.navigate(['roles/update', role.id], { state: { role } });
  }
}
