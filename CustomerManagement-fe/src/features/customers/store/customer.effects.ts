import { Injectable } from "@angular/core";
import { Actions, createEffect } from "@ngrx/effects";
import { CustomerService } from "../services/customer.service";
import { mergeMap, map, catchError, of, tap } from "rxjs";
import { ofType } from '@ngrx/effects';
import * as CustomerActions from './customer.actions';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable()
export class CustomerEffects {
  constructor(
    private actions$: Actions,
    private customerService: CustomerService,
    private snackBar: MatSnackBar
  ) {}

  loadCustomers$ = createEffect(() =>
    this.actions$.pipe(
      ofType(CustomerActions.loadCustomers),
      mergeMap(() =>
        this.customerService.getAll().pipe(
          map(customers => CustomerActions.loadCustomersSuccess({ customers })),
          catchError(error => of(CustomerActions.loadCustomersFailure({ error: error.message })))
        )
      )
    )
  );

  addCustomer$ = createEffect(() =>
    this.actions$.pipe(
      ofType(CustomerActions.addCustomer),
      mergeMap(({ customer }) =>
        this.customerService.create(customer).pipe(
          map(added => CustomerActions.addCustomerSuccess({ customer: added })),
          catchError(error =>
            of(CustomerActions.addCustomerFailure({ error: error.message }))
          )
        )
      )
    )
  );

  addSuccessToast$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(CustomerActions.addCustomerSuccess),
        tap(() => {
          this.snackBar.open('Adding Customer Successfully', 'Close', {
            duration: 3000
          });
        })
      ),
    { dispatch: false }
  );

  addFailToast$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(CustomerActions.addCustomerFailure),
        tap(({ error }) => {
          this.snackBar.open('Adding Customer Failed', 'Close', {
            duration: 3000
          });
          console.error('Add failed:', error);
        })
      ),
    { dispatch: false }
  );
}

