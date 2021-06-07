import { Component, Input, OnInit } from '@angular/core';
import { UserProfileDto } from 'src/app/clients/profile-client';
import { AuthenticationService } from 'src/app/modules/core/services/auth.service';
import { AuthService } from 'src/app/modules/layout/services/auth.service';
import { ProfileService } from '../../services/profile.service';

@Component({
  selector: 'app-account-details',
  templateUrl: './account-details.component.html',
  styleUrls: ['./account-details.component.scss']
})
export class AccountDetailsComponent implements OnInit {
  @Input() profile!: UserProfileDto;

  fullName!: string;
  email!: string;

  constructor(private authService: AuthService, private profileService: ProfileService) { }

  ngOnInit() {
    this.email = this.authService.user?.profile.email!;
    this.setFullName(this.profile);

    this.profileService.profile$
      .subscribe(profile => {
        if (profile) {
          this.setFullName(profile!);
        }
      });
  }

  setFullName(profile: UserProfileDto) {
    this.fullName = `${profile.firstName} ${profile.lastName}`;
  }

}
