import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { NgxSpinnerService } from 'ngx-spinner';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { UserProfileDto } from 'src/app/clients/profile-client';
import { ProfileService } from '../../services/profile.service';

@Component({
  selector: 'app-account-overview',
  templateUrl: './account-overview.component.html',
  styleUrls: ['./account-overview.component.scss']
})
export class AccountOverviewComponent implements OnInit {
  public profile$!: Observable<UserProfileDto | undefined>;

  constructor(
    private titleService: Title,
    private spinnerService: NgxSpinnerService,
    private profileService: ProfileService) { }

  ngOnInit(): void {
    this.titleService.setTitle('Hive - Profile');
    this.spinnerService.show();

    this.profile$! = this.profileService.getProfile()
      .pipe(tap({
        complete: () => this.spinnerService.hide()
      }));
  }

}
