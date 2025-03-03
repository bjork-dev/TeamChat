import {Routes} from '@angular/router';
import {adminGuard, authGuard} from './util/guards';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () => import('./components/auth/login.component').then(m => m.LoginComponent),
    title: 'Login',
  },
  {
    path: 'admin',
    loadComponent: () => import('./components/admin/admin-portal.component').then(m => m.AdminPortalComponent),
    canActivate: [authGuard],
    canActivateChild: [authGuard],
    title: 'Admin Portal',
    children: [
      {
        path: 'users',
        loadComponent: () => import('./components/admin/users.component').then(m => m.UsersComponent),
      }
    ],
  },
  {
    path: '',
    loadComponent: () => import('./components/teams/home.component').then(m => m.HomeComponent),
    canActivate: [adminGuard],
    canActivateChild: [adminGuard],
    title: 'Teams',
    children: [
      {
        path: 'group/:id',
        loadComponent: () => import('./components/teams/group.component').then(m => m.GroupComponent),
      }
    ]
  },

  {
    path: '**',
    redirectTo: '/',
  }
];
