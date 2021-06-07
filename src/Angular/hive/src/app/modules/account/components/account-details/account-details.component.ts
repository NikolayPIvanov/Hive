import { Component, Input, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { FileUpload } from 'src/app/clients/gigs-client';
import { FileResponse, ProfileClient, UserProfileDto } from 'src/app/clients/profile-client';
import { AuthService } from 'src/app/modules/layout/services/auth.service';
import { ProfileService } from '../../services/profile.service';

@Component({
  selector: 'app-account-details',
  templateUrl: './account-details.component.html',
  styleUrls: ['./account-details.component.scss']
})
export class AccountDetailsComponent implements OnInit {
  @Input() profile!: UserProfileDto;

  public download!: Observable<FileResponse>;
  public upload!: (upload: FileUpload) => Observable<any>;

  fullName!: string;
  email!: string;

  constructor(private authService: AuthService,
    private profileClient: ProfileClient,
    private profileService: ProfileService) { }

  ngOnInit() {
    this.download = this.profileClient.getAvatar(this.profile.id!);
    this.upload = (upload: FileUpload) => {
      return this.profileClient.changeAvatar(this.profile.id!, upload);
    }


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
