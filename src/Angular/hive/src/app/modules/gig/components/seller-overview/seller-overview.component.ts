import { Component, OnInit } from '@angular/core';
import { Observable, of } from 'rxjs';
import { mergeMap, tap } from 'rxjs/operators';
import { SellersClient } from 'src/app/clients/gigs-client';
import { UserProfileDto } from 'src/app/clients/profile-client';
import { ProfileService } from 'src/app/modules/account/services/profile.service';
import { SpinnerService } from 'src/app/modules/core/services/spinner.service';

@Component({
  selector: 'app-seller-overview',
  templateUrl: './seller-overview.component.html',
  styleUrls: ['./seller-overview.component.scss']
})
export class SellerOverviewComponent implements OnInit {
  profile$!: Observable<UserProfileDto | undefined>;

  url = '/assets/user.png';

  constructor(
    private profileApiClient: ProfileService,
    private sellerApiClient: SellersClient,
    private spinnerService: SpinnerService) { }

  ngOnInit(): void {
    this.spinnerService.show();

    this.profile$ = of(UserProfileDto.fromJS(
      {
        id: 1, firstName: 'Nikolay', lastName: 'Ivanov', education: 'University of Lorem',
        languages: [ 'Bulgarian', 'English', 'Russian']}))

    // this.profile$ = this.profileApiClient.getProfile()
    //   .pipe(tap({
    //     complete: () => this.spinnerService.hide()
    //   }))
  }

}
