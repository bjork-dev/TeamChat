import {Component} from '@angular/core';
import {HeaderComponent} from '../header.component';

@Component(
  {
    selector: 'app-admin-portal',
    imports: [
      HeaderComponent
    ],
    template: `
      <app-header/>
      <h1>Admin Portal</h1>
    `
  }
)
export class AdminPortalComponent {
}
