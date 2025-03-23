import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(private snackBar: MatSnackBar, private router: Router) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        console.warn('HTTP Error Intercepted:', error);

        const status = error.status;
        let message = 'Something went wrong.';

        switch (status) {
          case 0:
            message = 'Server is not reachable!';
            this.router.navigate(['/login']);
            break;
          case 400:
            message = 'Bad request!';
            break;
          case 401:
            message = 'Unauthorized. Please login again.';
            this.router.navigate(['/login']);
            break;
          case 403:
            message = 'Access denied.';
            break;
          case 404:
            message = 'Resource not found.';
            break;
          case 409:
            message = 'Conflict detected.';
            break;
          case 500:
            message = 'Server error occurred.';
            break;
        }

        this.snackBar.open(message, 'Close', { duration: 3000 });
        return throwError(() => error);
      })
    );
  }
}
