import { Routes } from '@angular/router';
import { authGuard } from './guards/auth-guard';
import { nonAuthGuard } from './guards/non-auth-guard';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'accueil',
    pathMatch: 'full',
  },
  {
    path: 'accueil',
    loadComponent: () => import('./pages/accueil/accueil').then((m) => m.Accueil),
    canActivate: [authGuard],
  },
  {
    path: 'auth',
    canActivate: [nonAuthGuard],
    children: [
      { path: '', redirectTo: 'login', pathMatch: 'full' },
      { path: 'login', loadComponent: () => import('./pages/login/login').then((m) => m.Login) },
      {
        path: 'register',
        loadComponent: () => import('./pages/register/register').then((m) => m.Register),
      },
    ],
  },
  {
    path: 'auth/register',
    loadComponent: () => import('./pages/register/register').then((m) => m.Register),
    canActivate: [nonAuthGuard],
  },
  {
    path: '**',
    redirectTo: 'accueil',
  },
];
