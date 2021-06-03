import { Injectable } from '@angular/core';
import { Router, CanActivate } from '@angular/router';
import { AuthService } from '../../layout/services/auth.service';

@Injectable()
export class AdminGuard implements CanActivate {

    constructor(private router: Router,
        private authService: AuthService) { }

    canActivate() {
        const user = this.authService.user;
        const roles = user?.profile.role;
        
        if (user && this.isAdmin(roles)) {
            return true;

        } else {
            this.router.navigate(['/home']);
            return false;
        }
    }

    private isAdmin(roles: string[]) {
        return roles.indexOf('Admin') > -1;
    }
}