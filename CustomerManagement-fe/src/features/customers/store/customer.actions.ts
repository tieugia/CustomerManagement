import { createAction, props } from '@ngrx/store';
import { Customer } from '../models/customer.model';

export const loadCustomers = createAction('[Customer] Load Customers');
export const loadCustomersSuccess = createAction('[Customer] Load Customers Success', props<{ customers: Customer[] }>());
export const loadCustomersFailure = createAction('[Customer] Load Customers Failure', props<{ error: string }>());

export const addCustomer = createAction('[Customer] Add Customer', props<{ customer: Customer }>());
export const addCustomerSuccess = createAction('[Customer] Add Customer Success', props<{ customer: Customer }>());
export const addCustomerFailure = createAction('[Customer] Add Customer Failure', props<{ error: string }>());
