import {Component, signal} from '@angular/core';
import {HeaderComponent} from '../header.component';
import {SidenavComponent} from '../sidenav.component';
import {RouterOutlet} from '@angular/router';
import {MatIcon} from '@angular/material/icon';
import {MatIconButton} from '@angular/material/button';

@Component(
  {
    selector: 'app-home',
    imports: [
      HeaderComponent,
      SidenavComponent,
      RouterOutlet,
      MatIcon,
      MatIconButton
    ],
    template: `
      <app-header>
        <button mat-icon-button (click)="toggle()">
          <mat-icon>menu</mat-icon>
        </button>
      </app-header>
        <app-sidenav [toggleDrawer]="toggleSidenav()">
          <router-outlet></router-outlet>
        </app-sidenav>
    `
  }
)
export class HomeComponent {
  toggleSidenav = signal<number>(0);

  toggle() {
    this.toggleSidenav.update(val => val + 1);
  }
}
