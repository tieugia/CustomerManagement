import { Injectable, inject } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { map, take, tap } from 'rxjs/operators';
import { selectAuthToken } from '../../features/auth/store/auth.selectors';
import { AuthState } from '../../features/auth/store/auth.state';
import { loginSuccess } from '../../features/auth/store/auth.actions';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
  constructor(private store: Store<AuthState>, private router: Router) {}

  canActivate() {
    return this.store.select(selectAuthToken).pipe(
      take(1),
      tap(token => {
        if (!token) {
          const localToken = localStorage.getItem('access_token');
          if (localToken) {
            this.store.dispatch(loginSuccess({ token: localToken }));
          }
        }
      }),
      map(token => {
        const localToken = token || localStorage.getItem('access_token');
        if (localToken) return true;
        this.router.navigate(['/login']);
        return false;
      })
    );
  }
}
