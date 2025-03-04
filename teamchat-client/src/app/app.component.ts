import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {HeaderComponent} from './components/header.component';
import {MatIcon} from '@angular/material/icon';
import {MatIconButton} from '@angular/material/button';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
  ],
  template: `
    <router-outlet></router-outlet>
  `
})
export class AppComponent {
}
