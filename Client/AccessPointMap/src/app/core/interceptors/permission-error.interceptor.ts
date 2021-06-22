import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { AuthService } from 'src/app/authentication/services/auth.service';
import { catchError } from 'rxjs/operators';

@Injectable()
export class PermissionErrorInterceptor implements HttpInterceptor {

  constructor(private authService: AuthService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request)
      .pipe(catchError(err => {
        if([401].includes(err.status) && this.authService.userValue) {
          this.authService.logout();
        }

        const error = (err && err.error && err.error.message) || err.statusText;
        console.error(error);
        return throwError(error);
      }));
  }
}