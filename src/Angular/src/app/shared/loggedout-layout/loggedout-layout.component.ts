import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { AuthenticationService } from 'src/authorization/authentication.service';

@Component({
  selector: 'app-loggedout-layout',
  templateUrl: './loggedout-layout.component.html',
  styleUrls: ['./loggedout-layout.component.scss']
})
export class LoggedoutLayoutComponent implements OnInit {
  isLoggedIn: boolean = false;

  constructor(private authenticationService: AuthenticationService,
              private titleService: Title) { }
  
  ngOnInit() {
    this.titleService.setTitle('Hive - Home Page');

    this.authenticationService.isAuthenticated()
      .then((r) => { this.isLoggedIn = r})
  }

  public login = () => {
    this.authenticationService.login();
  }
}
