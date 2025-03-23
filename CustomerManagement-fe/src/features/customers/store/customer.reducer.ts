import { createReducer, on } from '@ngrx/store';
import * as CustomerActions from './customer.actions';
import { CustomerState } from './customer.state';

export const initialState: CustomerState = {
  customers: [],
  loading: false,
  error: null,
};

export const customerReducer = createReducer(
  initialState,
  
  on(CustomerActions.loadCustomers, state => ({ ...state, loading: true })),

  on(CustomerActions.loadCustomersSuccess, (state, { customers }) => ({
    ...state, loading: false, customers
  })),

  on(CustomerActions.loadCustomersFailure, (state, { error }) => ({
    ...state, loading: false, error
  })),

  on(CustomerActions.addCustomerSuccess, (state, { customer }) => ({
    ...state, customers: [...state.customers || [], customer]
  }))
);
