import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { RoleService } from '../../services/role.service';

@Component({
  selector: 'app-update-role',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './update-role.component.html'
})
export class UpdateRoleComponent implements OnInit {

  roleForm!: FormGroup;
  roleId!: string;

  constructor(
    private fb: FormBuilder,
    private roleService: RoleService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.roleId = this.route.snapshot.paramMap.get('id')!;

    this.roleForm = this.fb.group({
      roleName: ['', Validators.required]
    });

    this.loadRole();
  }

  loadRole() {
    this.roleService.getRoleById(this.roleId).subscribe({
      next: (role) => {
        this.roleForm.patchValue({
          roleName: role.name
        });
      },
      error: (err) => console.error('Failed to load role:', err)
    });
  }

  updateRole() {
    if (this.roleForm.invalid) return;

    const updatedRole = { name: this.roleForm.value.roleName };

    this.roleService.updateRoleById(this.roleId, updatedRole).subscribe({
      next: (res) => {
        alert("Role Updated Successfully");
        this.router.navigate(['/roles']);
      },
      error: (err) => console.error('Failed to update role:', err)
    });
  }
}
