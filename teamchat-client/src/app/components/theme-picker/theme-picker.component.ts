import {Component, inject, signal} from '@angular/core';
import {MatIconButton} from '@angular/material/button';
import {MatIcon} from '@angular/material/icon';
import {ThemePickerService} from './theme-picker.service';

@Component(
  {
    selector: 'app-theme-picker',
    imports: [
      MatIconButton,
      MatIcon
    ],
    template: `
      <div>
        <button mat-icon-button (click)="toggleTheme()">
          <mat-icon>{{ isDarkMode() ? 'light_mode' : 'dark_mode' }}</mat-icon>
        </button>
      </div>
    `
  }
)
export class ThemePickerComponent {

  themePickerService = inject(ThemePickerService)
  isDarkMode = this.themePickerService.isDarkMode

  toggleTheme() {
    this.themePickerService.toggleTheme()
  }

}
