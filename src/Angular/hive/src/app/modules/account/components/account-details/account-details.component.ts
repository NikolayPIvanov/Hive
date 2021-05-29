import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from 'src/app/modules/core/services/auth.service';

@Component({
  selector: 'app-account-details',
  templateUrl: './account-details.component.html',
  styleUrls: ['./account-details.component.scss']
})
export class AccountDetailsComponent implements OnInit {

  fullName!: string;
  email!: string;
  alias!: string;

  constructor(private authService: AuthenticationService) { }

  ngOnInit() {
    this.fullName = this.authService.getCurrentUser().fullName;
    this.email = this.authService.getCurrentUser().email;
  }

}
