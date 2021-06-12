import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { NgxSpinnerService } from 'ngx-spinner';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { UserProfileDto } from 'src/app/clients/profile-client';
import { AuthService } from 'src/app/modules/layout/services/auth.service';
import { ProfileService } from '../../services/profile.service';

@Component({
  selector: 'app-account-overview',
  templateUrl: './account-overview.component.html',
  styleUrls: ['./account-overview.component.scss']
})
export class AccountOverviewComponent implements OnInit {
  public profile$!: Observable<UserProfileDto | undefined>;
  public isSeller = false;

  constructor(
    private titleService: Title,
    private authService: AuthService,
    private spinnerService: NgxSpinnerService,
    private profileService: ProfileService) { }

  ngOnInit(): void {
    this.titleService.setTitle('Hive - Profile');
    this.spinnerService.show();
    this.isSeller = (this.authService.user?.profile.role as string[]).includes('Seller')
    
    this.profile$! = this.profileService.getProfile()
      .pipe(tap({
        complete: () => this.spinnerService.hide()
      }));
  }

}
