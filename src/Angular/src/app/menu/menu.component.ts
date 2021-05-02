import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from 'src/authorization/authentication.service';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss']
})
export class MenuComponent implements OnInit {
  public isUserAuthenticated: boolean = false;

  constructor(private _authService: AuthenticationService) { }
  
  ngOnInit(): void {
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