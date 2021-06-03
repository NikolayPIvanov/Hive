import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { Title } from '@angular/platform-browser';
import { NotificationService } from 'src/app/modules/core/services/notification.service';
import { AuthenticationService } from 'src/app/modules/core/services/auth.service';
import { AuthService } from 'src/app/modules/layout/services/auth.service';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';


@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
    private unsubscribe = new Subject();
    
    public isUserAuthenticated: boolean = false;

    constructor(private router: Router,
        private titleService: Title,
        private notificationService: NotificationService,
        private authenticationService: AuthService) {
    }

    ngOnInit() {
        this.titleService.setTitle('Hive - Login');
        this.authenticationService.loginChanged
            .pipe(takeUntil(this.unsubscribe))
            .subscribe(res => {
                this.isUserAuthenticated = res;
            })
    }

    public login = () => {
        this.authenticationService.login();
    }

    public logout = () => {
        this.authenticationService.logout();
    }
}