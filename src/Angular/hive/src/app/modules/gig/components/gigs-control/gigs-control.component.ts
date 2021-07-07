import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { mergeAll, switchMap, takeUntil, tap } from 'rxjs/operators';
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
export class GigsControlComponent implements OnInit, OnDestroy {
  private subject = new Subject();
  private gigs: GigOverviewDto[] = [];
  private gigsSubject = new BehaviorSubject<GigOverviewDto[]>(this.gigs);
  public gigs$ = this.gigsSubject.asObservable();

  public sellerId!: string;
  public profile$!: Observable<UserProfileDto | undefined>;

  length = 100;
  pageSize = 8;
  pageNumber = 0;
  pageSizeOptions: number[] = [4, 8, 16, 64];

  constructor(
    private profileService: ProfileService,
    private gigsClient: GigsClient,
    private authService: AuthService,
    private sellerClient: SellersClient,
    public dialog: MatDialog
  ) { }

  ngOnInit(): void {
    this.profile$ = this.profileService.getProfile()
    this.sellerClient.getUserSellerId()
      .pipe(
        takeUntil(this.subject),
        tap({ next: (id) => this.sellerId = id }),
        switchMap(id => this.sellerClient.getMyGigs(this.pageSize, this.pageNumber + 1, null, id)),
        tap({ next: (gigs) => this.pushNewGigs(gigs) }))
      .subscribe()
  }

  ngOnDestroy(): void {
    this.subject.next();
    this.subject.complete();
  }

  get email() {
    return this.authService.user?.profile.email;
  }
  
  pageChange(pageEvent: PageEvent) {
    this.pageSize = pageEvent.pageSize;
    this.pageNumber = pageEvent.pageIndex;

    this.sellerClient.getMyGigs(this.pageSize, this.pageNumber + 1, null, this.sellerId)
      .pipe(
        takeUntil(this.subject),
        tap({
          next: (gigs) => {
            this.pushNewGigs(gigs)
        } }))
      .subscribe();
  }

  deleteAndRemoveGig(id: number) {
    this.gigsClient.delete(id)
      .pipe(
        takeUntil(this.subject),
        tap({
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

  onCreateNew() {
    const ref = this.dialog.open(GigCreateComponent, {
      width: '50%'
    });

    ref.afterClosed()
      .pipe(
        takeUntil(this.subject),
        switchMap(id => this.gigsClient.getGigById(id)),
        tap({
          next: (gig) => {
            this.gigs.push(gig);
            this.length += 1;
            this.gigsSubject.next(this.gigs)
          }
        })
      )
      .subscribe();
  }

  private pushNewGigs(gigs: PaginatedListOfGigOverviewDto) {
    this.gigs = gigs.items!;
    this.length = gigs.totalCount!;
    this.gigsSubject.next(this.gigs);
  }

}
