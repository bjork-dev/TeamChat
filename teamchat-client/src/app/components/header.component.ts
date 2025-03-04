import {Component, output} from '@angular/core';
import {MatToolbar} from '@angular/material/toolbar';
import {MatIconButton} from '@angular/material/button';
import {MatIcon} from '@angular/material/icon';
import {UserComponent} from './users/user.component';
import {ThemePickerComponent} from './theme-picker/theme-picker.component';

@Component(
  {
    selector: 'app-header',
    imports: [
      MatToolbar,
      UserComponent,
      ThemePickerComponent
    ],
    styles: [
      `
        .spacer {
          flex: 1 1 auto;
        }
      `
    ],
    template: `
      <mat-toolbar>
        <ng-content/>
        <h1>TeamChat</h1>
        <span class="spacer"></span>
        <app-theme-picker/>
        <app-user/>
      </mat-toolbar>

    `
  }
)
export class HeaderComponent {
  toggleSidenav = output<void>()
}
