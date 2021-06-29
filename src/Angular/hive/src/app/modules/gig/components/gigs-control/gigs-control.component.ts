import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { mergeAll, switchMap, tap } from 'rxjs/operators';
import { GigDto, GigOverviewDto, GigsClient, PaginatedListOfGigDto, PaginatedListOfGigOverviewDto, SellersClient } from 'src/app/clients/gigs-client';
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
  private gigs: GigOverviewDto[] = [];
  private gigsSubject = new BehaviorSubject<GigOverviewDto[]>(this.gigs);
  public gigs$ = this.gigsSubject.asObservable();

  public profile$!: Observable<UserProfileDto | undefined>;
  sellerId!: string;

  constructor(
    private profileService: ProfileService,
    private gigsClient: GigsClient,
    private authService: AuthService,
    private sellerClient: SellersClient,
    public dialog: MatDialog
  ) { }

  // Pagination
  // MatPaginator Inputs
  length = 100;
  pageSize = 10;
  pageNumber = 0;
  pageSizeOptions: number[] = [5, 10, 25, 100];

  pageChange(pageEvent: PageEvent) {
    this.pageSize = pageEvent.pageSize;
    this.pageNumber = pageEvent.pageIndex;

    this.sellerClient.getMyGigs(this.pageSize, this.pageNumber + 1, this.sellerId)
      .pipe(tap({
        next: (gigs) => {
          this.gigs = gigs.items!;
          this.length = gigs.totalCount!;
          this.gigsSubject.next(this.gigs);
        }
      }))
      .subscribe();
  }

  reload(id: number) {
    this.gigsClient.delete(id)
      .pipe(tap({
        next: () => {
          const index = this.gigs.findIndex(g => g.id! === id)
          if (index > -1) {
            this.gigs.splice(index, 1);
            this.length -= 1;
            this.gigsSubject.next(this.gigs);
          }
        }
      }))
    .subscribe()
  }

  ngOnInit(): void {
    this.profile$ = this.profileService.getProfile()
    this.sellerClient.getUserSellerId()
      .pipe(
        tap({
          next: (id) => this.sellerId = id
        }),
        switchMap(id => this.sellerClient.getMyGigs(10, 1, id)))
      .subscribe(gigs => {
        this.gigs = gigs.items!;
        this.length = gigs.totalCount!;
        this.gigsSubject.next(this.gigs);
      })
  }

  onCreateNew() {
    const ref = this.dialog.open(GigCreateComponent, {
      width: '50%'
    });

    ref.afterClosed().subscribe(id => {
      this.gigsClient.getGigById(id).subscribe(gig => {
        this.gigs.push(gig);
        this.length += 1;
        this.gigsSubject.next(this.gigs)
      })
    })
  }

  displayName(profile: UserProfileDto) {
    const display = `${profile.givenName} ${profile.surname}`;
    return display.trim() != '' ? display : this.authService.user?.profile.email;
  }

  get email() {
    return this.authService.user?.profile.email;
  }

}
