import {Component, inject} from '@angular/core';
import {MatIcon} from '@angular/material/icon';
import {MatButton, MatIconButton} from '@angular/material/button';
import {AuthService} from './auth/auth.service';
import {MatMenu, MatMenuItem, MatMenuTrigger} from '@angular/material/menu';

@Component({
  selector: 'app-user',
  imports: [
    MatIcon,
    MatButton,
    MatMenu,
    MatMenuItem,
    MatMenuTrigger
  ],
  template: `
    @if (user()) {
      <button mat-flat-button [matMenuTriggerFor]="menu">
        <mat-icon>person</mat-icon>
        <span>{{ user()?.firstName }}</span>
      </button>

      <mat-menu #menu="matMenu">
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
    location.reload();
  }
}
