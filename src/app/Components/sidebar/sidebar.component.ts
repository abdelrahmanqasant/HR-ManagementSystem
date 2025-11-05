import { Component, OnInit, inject } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AccountService } from '../../services/account.service';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css'],
})
export class SidebarComponent implements OnInit {
  fullName: string = '';
  userRole: string = '';


  private accountService = inject(AccountService);
  private router = inject(Router);


  ngOnInit() {
    this.accountService.currentUser$.subscribe({
      next: (user) => {
        if (user) {
          this.fullName = user.fullName;
          this.userRole = user.role;
        }
      },
      error: (error) => {
        console.error('Error loading user:', error);
      },
    });
  }

logout() {
  console.log(' Logging out...');

  this.accountService.logout().subscribe({
    next: (res) => {
      console.log(' Logout successful:', res);
      this.router.navigate(['/login']);
    },
    error: (error) => {
      console.error(' Logout error:', error);

      this.accountService.forceLogout();
      this.router.navigate(['/login']);
    }
  });
}

  toggleSidebar() {
    const sidebar = document.querySelector('.sidebar');
    const closeBtn = document.querySelector('#btn') as HTMLElement;

    if (sidebar && sidebar.classList) {
      sidebar.classList.toggle('open');
      this.menuBtnChange(closeBtn);
    }
  }

  menuBtnChange(closeBtn: HTMLElement): void {
    const sidebar = document.querySelector('.sidebar');
    if (sidebar && closeBtn) {
      if (sidebar.classList.contains('open')) {
        closeBtn.classList.replace('bx-menu', 'bx-menu-alt-right');
      } else {
        closeBtn.classList.replace('bx-menu-alt-right', 'bx-menu');
      }
    }
  }

  closeSidebar() {
    const sidebar = document.querySelector('.sidebar');
    const closeBtn = document.querySelector('#btn') as HTMLElement;

    if (sidebar && sidebar.classList.contains('open')) {
      sidebar.classList.remove('open');
      this.menuBtnChange(closeBtn);
    }
  }
}
