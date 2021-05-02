import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from 'src/authorization/authentication.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'CompanyEmployees.Client.Oidc';
  public userAuthenticated = false;
  
  constructor(private _authService: AuthenticationService){
    this._authService.loginChanged
    .subscribe(userAuthenticated => {
      this.userAuthenticated = userAuthenticated;
    })
  }
  
  ngOnInit(): void {
    this._authService.isAuthenticated()
    .then(userAuthenticated => {
      this.userAuthenticated = userAuthenticated;
    })
  }
}
