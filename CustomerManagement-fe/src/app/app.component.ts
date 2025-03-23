import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { MatToolbarModule } from '@angular/material/toolbar';
import { selectAuthToken } from '../features/auth/store/auth.selectors';
import { logout } from '../features/auth/store/auth.actions';
import { AuthState } from '../features/auth/store/auth.state';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-root',
  standalone: true,
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
  imports: [RouterModule, MatToolbarModule, CommonModule, MatButtonModule],
})
export class AppComponent {
  token$: Observable<string | null> = new Observable<string | null>();

  constructor(private store: Store<AuthState>, private router: Router) {
    
  }

  ngOnInit() {
    this.token$ = this.store.select(selectAuthToken)
  }
  logout() {
    this.store.dispatch(logout());
    this.router.navigate(['/login']);
  }
}
