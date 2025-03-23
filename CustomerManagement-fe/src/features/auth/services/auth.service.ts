import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly API = '/api/auth';

  constructor(private http: HttpClient) {}

  login(email: string, password: string) {
    return this.http.post<{ token: string }>(`${this.API}/login`, { email, password });
  }

  saveToken(token: string) {
    localStorage.setItem('access_token', token);
  }

  getToken(): string | null {
    return localStorage.getItem('access_token');
  }

  logout() {
    localStorage.removeItem('access_token');
  }
}
