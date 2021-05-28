import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { User } from 'oidc-client';
import { NotificationService } from 'src/app/core/services/notification.service';
import { AuthenticationService } from 'src/authorization/authentication.service';

@Component({
  selector: 'app-dashboard-home',
  templateUrl: './dashboard-home.component.html',
  styleUrls: ['./dashboard-home.component.scss']
})
export class DashboardHomeComponent implements OnInit {

  constructor(private notificationService: NotificationService,
    private authService: AuthenticationService,
    private titleService: Title) {
  }

  ngOnInit() {
  }

}
