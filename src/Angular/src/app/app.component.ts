import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from 'src/authorization/authentication.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'CompanyEmployees.Client.Oidc';
  public userAuthenticated = false;
  
  constructor(private _authService: AuthenticationService, private router: Router){
    this._authService.loginChanged
    .subscribe(userAuthenticated => {
      this.userAuthenticated = userAuthenticated;
    })

    for (var i = 0; i < this.router.config.length; i++) {
      var routePath = this.router.config[i].path;
      console.log(routePath);
    }
  }
  
  ngOnInit(): void {
    this._authService.isAuthenticated()
    .then(userAuthenticated => {
      this.userAuthenticated = userAuthenticated;
    })
  }
}
