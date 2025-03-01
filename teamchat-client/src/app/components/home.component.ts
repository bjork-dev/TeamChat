import {Component, signal} from '@angular/core';
import {HeaderComponent} from './header.component';
import {SidenavComponent} from './sidenav.component';
import {FooterComponent} from './footer.component';
import {RouterOutlet} from '@angular/router';

@Component(
  {
    selector: 'app-home',
    imports: [
      HeaderComponent,
      SidenavComponent,
      FooterComponent,
      RouterOutlet
    ],
    styles: [
      `
        .content {

        }
      `
    ],
    template: `
      <div class="content">
        <app-header (toggleSidenav)="toggle()"/>
        <app-sidenav [toggleDrawer]="toggleSidenav()">
          <router-outlet></router-outlet>
        </app-sidenav>
        <app-footer class="d-flex justify-content-center"/>
      </div>
    `
  }
)
export class HomeComponent {
  toggleSidenav = signal<number>(0);

  toggle() {
    this.toggleSidenav.update(val => val + 1);
  }
}
