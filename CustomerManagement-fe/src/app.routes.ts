import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: 'login',
    loadChildren: () =>
      import('./features/auth/auth.routes').then(m => m.AUTH_ROUTES)
  },
  {
    path: 'customers',
    loadChildren: () =>
      import('./features/customers/customers.routes').then(m => m.CUSTOMER_ROUTES)
  },
  {
    path: '',
    redirectTo: 'customers',
    pathMatch: 'full'
  },
  {
    path: '**',
    redirectTo: 'customers'
  }
];