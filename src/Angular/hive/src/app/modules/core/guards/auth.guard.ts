import { Injectable } from '@angular/core';
import { Router, CanActivate } from '@angular/router';
import { AuthService } from '../../layout/services/auth.service';

import { AuthenticationService } from '../services/auth.service';
import { NotificationService } from '../services/notification.service';

@Injectable()
export class AuthGuard implements CanActivate {
    
    constructor(private router: Router,
        private notificationService: NotificationService,
        private authService: AuthService) { }

    canActivate() {
        const user = this.authService.user;

        if (user) {
            if (!user.expired) {
                return true;
            } else {
                this.notificationService.openSnackBar('Your session has expired');
                this.router.navigate(['auth/login']);
                return false;
            }
        }

        this.router.navigate(['auth/login']);
        return false;
    }
}