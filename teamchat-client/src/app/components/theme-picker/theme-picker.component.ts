import {Component, signal} from '@angular/core';
import {MatIconButton} from '@angular/material/button';
import {MatIcon} from '@angular/material/icon';

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

  isDarkMode = signal(false)

  constructor() {
    this.setTheme();
  }

  toggleTheme() {
    const isDarkMode = localStorage.getItem('dark') === 'true';
    localStorage.setItem('dark', isDarkMode ? 'false' : 'true');
    this.setTheme();
  }

  setTheme() {
    const isDarkMode = localStorage.getItem('dark') === 'true';
    const body = document.getElementsByTagName('body')[0];
    if (isDarkMode) {
      body.classList.remove('light-mode');
      body.classList.add('dark-mode');
      localStorage.setItem('dark', 'true');
      this.isDarkMode.set(true);
    } else {
      body.classList.remove('dark-mode');
      body.classList.add('light-mode');
      localStorage.setItem('dark', 'false');
      this.isDarkMode.set(false);
    }
  }
}
