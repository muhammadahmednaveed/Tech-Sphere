import {
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor() {}

  intercept(req: HttpRequest<any>, next: HttpHandler) {
    console.log('interceptor');
    // Get the auth token from the local storage

    let authToken =
      localStorage.getItem('User') != undefined
        ? JSON.parse(localStorage.getItem('User')).Token
        : '';
    // Clone the request and replace the original headers with
    // cloned headers, updated with the authorization.
    if (authToken == '' || req.url.includes('http://localhost/Auth/api/Auth')) {
      return next.handle(req);
    }

    const authReq = req.clone({
      headers: req.headers.set('Authorization', 'Bearer ' + authToken),
    });

    // send cloned request with header to the next handler.
    return next.handle(authReq);
  }
}
