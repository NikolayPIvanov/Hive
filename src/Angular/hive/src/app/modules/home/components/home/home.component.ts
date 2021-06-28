import { Component, OnInit } from '@angular/core';
import { AuthService, UserRole } from 'src/app/modules/layout/services/auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  public role: UserRole | undefined;
  constructor(private authService: AuthService) { }

  ngOnInit(): void {
    debugger;
    this.role = this.authService.role;
  }

}
