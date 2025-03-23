import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators, ReactiveFormsModule, FormBuilder } from '@angular/forms';
import { Store } from '@ngrx/store';
import { AuthState } from '../../store/auth.state';
import { createLoginAction } from '../../store/auth.actions';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
  imports: [CommonModule, MatFormFieldModule, MatCardModule, ReactiveFormsModule, MatInputModule, MatButtonModule]
})
export class LoginComponent implements OnInit {
  loginForm?: FormGroup;

  constructor(private store: Store<AuthState>, private fb: FormBuilder) { }

  ngOnInit() {
    this.loginForm = this.fb.group({
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', [Validators.required])
    });
  }

  onSubmit() {
    if (this.loginForm && this.loginForm.valid) {
      this.store.dispatch(createLoginAction(this.loginForm.value));
    }
  }
}
