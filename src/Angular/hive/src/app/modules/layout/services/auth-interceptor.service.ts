import { HttpEvent, HttpHandler, HttpHeaders, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { from, Observable } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthInterceptorService {

  constructor(private authService: AuthService) { }
  
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return from(
      this.authService.getAccessToken()
      .then(token => {
        const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
        const authRequest = req.clone({ headers });
        return next.handle(authRequest).toPromise();
      })
    );
  }
}
