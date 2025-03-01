import {Routes} from '@angular/router';
import {authGuard} from './util/guards';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () => import('./components/auth/login.component').then(m => m.LoginComponent)
  },
  {
    path: '',
    loadComponent: () => import('./components/home.component').then(m => m.HomeComponent),
    canActivate: [authGuard],
    canActivateChild: [authGuard],
    children: [
      {
        path: 'group/:id',
        loadComponent: () => import('./components/teams/group.component').then(m => m.GroupComponent),
        canActivate: [authGuard]
      },
    ]
  },

  {
    path: '',
    redirectTo: '/home',
    pathMatch: 'full'
  },
  {
    path: '**',
    redirectTo: '/',
  }
];
