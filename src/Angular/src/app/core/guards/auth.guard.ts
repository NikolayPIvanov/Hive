import { Injectable } from '@angular/core';
import { Router, CanActivate, CanActivateChild, CanLoad, CanDeactivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Route, UrlSegment } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthenticationService } from 'src/authorization/authentication.service';
import { NotificationService } from '../services/notification.service';


@Injectable()
export class AuthGuard implements CanActivate, CanActivateChild, CanDeactivate<unknown>, CanLoad {

    constructor(private router: Router,
        private notificationService: NotificationService,
        private authService: AuthenticationService) { }

    canActivate(
        next: ActivatedRouteSnapshot,
        state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
        let url: string = state.url;
        return this.checkUserLogin(next, url);
    }

    canActivateChild(
        next: ActivatedRouteSnapshot,
        state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
        return this.canActivate(next, state);
    }
    
    canDeactivate(
        component: unknown,
        currentRoute: ActivatedRouteSnapshot,
        currentState: RouterStateSnapshot,
        nextState?: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
        return true;
    }
    
    canLoad(
        route: Route,
        segments: UrlSegment[]): Observable<boolean> | Promise<boolean> | boolean {
        return true;
    }
    
    checkUserLogin(route: ActivatedRouteSnapshot, url: any): boolean {
        const user = this.authService.getCurrentUser();
        const isAuthenticated = user && !user.expired && user.expires_in > 0;
        if (isAuthenticated) {
          if (route.data.role && !this.authService.isInRole(route.data.role)) {
              this.router.navigate(['/home']);
              this.notificationService.openSnackBar('You do not have the required permissions.');
            return false;
          }
            
          return true;
        }
    
        this.notificationService.openSnackBar('You are not signed in.');
        this.router.navigate(['/home']);
        return false;
      }
}