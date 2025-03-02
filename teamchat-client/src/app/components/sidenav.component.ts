import {Component, effect, ElementRef, input, signal, viewChild} from '@angular/core';
import {MatDrawer, MatSidenav, MatSidenavContainer, MatSidenavContent} from '@angular/material/sidenav';
import {MatListItem, MatNavList} from '@angular/material/list';
import {RouterLink} from '@angular/router';
import {TeamsComponent} from './teams/teams.component';

@Component({
  selector: 'app-sidenav',
  imports: [
    MatSidenavContainer,
    MatSidenav,
    MatNavList,
    MatSidenavContent,
    TeamsComponent
  ],
  styles: [
    `
    mat-sidenav {
      width: 300px;
    }

    mat-sidenav-container {
      height: calc(100vh - 64px);
    }
    `
  ],
  template: `
    <mat-sidenav-container>
      <mat-sidenav #sidenav [mode]="isMobile() ? 'over' : 'side'" [opened]="true">
        <mat-nav-list>
          <app-team/>
        </mat-nav-list>
      </mat-sidenav>
      <mat-sidenav-content>
        <div class="container">
          <ng-content/>
        </div>
      </mat-sidenav-content>
    </mat-sidenav-container>
  `
})
export class SidenavComponent {
  toggleDrawer = input.required<number>()
  sidenavRef = viewChild.required<MatSidenav>('sidenav');

  isMobile = signal(false);

  constructor() {
    effect(() => {
      const toggleDrawer = this.toggleDrawer();
      if (toggleDrawer > 0) {
        this.sidenavRef()?.toggle();
      }
    });
  }

}
