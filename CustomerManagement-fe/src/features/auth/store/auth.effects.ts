import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import * as AuthActions from './auth.actions';
import { catchError, map, mergeMap, of, tap } from 'rxjs';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable()
export class AuthEffects {
  constructor(private actions$: Actions, private authService: AuthService, private router: Router) {}

  login$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AuthActions.createLoginAction),
      mergeMap(({ email, password }) =>
        this.authService.login(email, password).pipe(
          map(res => AuthActions.loginSuccess({ token: res.token })),
          catchError(error => of(AuthActions.loginFailure({ error: error.message || 'Login failed' })))
        )
      )
    )
  );

  loginSuccess$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AuthActions.loginSuccess),
      tap(({ token }) => {
        this.authService.saveToken(token);
        this.router.navigate(['/customers']);
      })
    ),
    { dispatch: false }
  );

  logout$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AuthActions.logout),
      tap(() => {
        this.authService.logout();
      })
    ),
    { dispatch: false }
  );
}
