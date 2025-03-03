import {Component, inject} from '@angular/core';
import {MatIcon} from '@angular/material/icon';
import {MatButton, MatIconButton} from '@angular/material/button';
import {AuthService} from '../auth/auth.service';
import {MatMenu, MatMenuItem, MatMenuTrigger} from '@angular/material/menu';
import {isAdmin} from '../../domain/users/user';
import {RouterLink} from '@angular/router';

@Component({
  selector: 'app-user',
  imports: [
    MatIcon,
    MatButton,
    MatMenu,
    MatMenuItem,
    MatMenuTrigger,
    RouterLink
  ],
  template: `
    @if (user(); as user) {
      <button mat-flat-button [matMenuTriggerFor]="menu">
        <mat-icon>person</mat-icon>
        <span>{{ user.firstName }}</span>
      </button>

      <mat-menu #menu="matMenu">
        @if (isAdmin(user)) {
          <a mat-menu-item routerLink="/admin">Admin Portal</a>
        }
        <button mat-menu-item (click)="logout()">Logout</button>
      </mat-menu>
    }
  `

})
export class UserComponent {
  user = inject(AuthService).user
  authService = inject(AuthService);

  logout() {
    this.authService.logout()
  }

  protected readonly isAdmin = isAdmin;
}
