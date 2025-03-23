import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler } from '@angular/common/http';
import { environment } from '../../../environments/environment.dev';

@Injectable()
export class ApiPrefixInterceptor implements HttpInterceptor {
  private baseUrl = environment.apiUrl;

  intercept(req: HttpRequest<any>, next: HttpHandler) {
    if (!req.url.startsWith('https')) {
      const apiReq = req.clone({ url: `${this.baseUrl}${req.url}` });
      return next.handle(apiReq);
    }
    return next.handle(req);
  }
}
