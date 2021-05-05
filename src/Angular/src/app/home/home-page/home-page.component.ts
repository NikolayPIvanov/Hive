import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { User } from 'oidc-client';
import { NotificationService } from 'src/app/core/services/notification.service';
import { AuthenticationService } from 'src/authorization/authentication.service';

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.scss']
})
export class HomePageComponent implements OnInit {
  currentUser: User | null = null;
  public isUserAuthenticated: boolean = false;

  constructor(private notificationService: NotificationService,
    private authService: AuthenticationService,
    private _authService: AuthenticationService,
    private titleService: Title) { }

    ngOnInit() {
      this.currentUser = this.authService.getCurrentUser();
      this.titleService.setTitle('Hive - Home Page');

      this._authService.loginChanged
      .subscribe(res => {
        this.isUserAuthenticated = res;
      })
    }
  
  
    public login = () => {
      this._authService.login();
    }
    public logout = () => {
      this._authService.logout();
    }

}
