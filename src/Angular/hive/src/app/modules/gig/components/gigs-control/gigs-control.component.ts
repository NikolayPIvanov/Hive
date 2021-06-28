import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { mergeAll, switchMap, tap } from 'rxjs/operators';
import { GigDto, GigsClient, PaginatedListOfGigDto, PaginatedListOfGigOverviewDto, SellersClient } from 'src/app/clients/gigs-client';
import { UserProfileDto } from 'src/app/clients/profile-client';
import { ProfileService } from 'src/app/modules/account/services/profile.service';
import { AuthService } from 'src/app/modules/layout/services/auth.service';
import { GigCreateComponent } from '../gig-create/gig-create.component';

export class GigProfileData
{
  gig: GigDto;
  profile: UserProfileDto;

  constructor(gig: GigDto, profile: UserProfileDto) {
    this.gig = gig;
    this.profile = profile;
  }
}


@Component({
  selector: 'app-gigs-control',
  templateUrl: './gigs-control.component.html',
  styleUrls: ['./gigs-control.component.scss']
})
export class GigsControlComponent implements OnInit {
  public profile$!: Observable<UserProfileDto | undefined>;
  public gigs$!: Observable<PaginatedListOfGigOverviewDto>;
  sellerId!: string;

  constructor(
    private profileService: ProfileService,
    private gigsClient: GigsClient,
    private authService: AuthService,
    private sellerClient: SellersClient,
    public dialog: MatDialog
  ) { }

  reload(id: number) {
    this.gigs$ = this.gigsClient.delete(id)
      .pipe(switchMap(() => this.sellerClient.getMyGigs(10, 1, this.sellerId)))
  }

  ngOnInit(): void {
    this.profile$ = this.profileService.getProfile()
    this.gigs$ = this.sellerClient.getUserSellerId()
      .pipe(
        tap({
          next: (id) => this.sellerId = id
        }),
        switchMap(id => this.sellerClient.getMyGigs(10, 1, id)))
  }

  onCreateNew() {
    this.dialog.open(GigCreateComponent, {
      width: '50%'
    });
  }

  displayName(profile: UserProfileDto) {
    const display = `${profile.givenName} ${profile.surname}`;
    return display.trim() != '' ? display : this.authService.user?.profile.email;
  }

  get email() {
    return this.authService.user?.profile.email;
  }

}
