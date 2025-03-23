import { Routes } from '@angular/router';
import { CustomerListComponent } from './components/customer-list/customer-list.component';
import { CustomerFormComponent } from './components/customer-form/customer-form.component';
import { AuthGuard } from '../../core/guards/auth.guard';

export const CUSTOMER_ROUTES: Routes = [
  { path: '', component: CustomerListComponent, canActivate: [AuthGuard] },
  { path: 'add', component: CustomerFormComponent, canActivate: [AuthGuard] }
];
