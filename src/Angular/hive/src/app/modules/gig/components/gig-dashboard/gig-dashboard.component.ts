import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { BehaviorSubject, Subject } from 'rxjs';
import { switchMap, takeUntil, tap } from 'rxjs/operators';
import { GigOverviewDto, GigsClient, PaginatedListOfGigOverviewDto, SellersClient } from 'src/app/clients/gigs-client';
import { AuthService } from 'src/app/modules/layout/services/auth.service';
import { GigCreateComponent } from '../gig-create/gig-create.component';

@Component({
  selector: 'app-gig-dashboard',
  templateUrl: './gig-dashboard.component.html',
  styleUrls: ['./gig-dashboard.component.scss']
})
export class GigDashboardComponent implements OnInit, OnDestroy {
  private unsubscribe = new Subject();
  private gigs: GigOverviewDto[] = [];
  private gigsSubject = new BehaviorSubject<GigOverviewDto[]>(this.gigs);
  public gigs$ = this.gigsSubject.asObservable();

  length = 0;
  pageSize = 8;
  pageNumber = 0;
  pageSizeOptions: number[] = [8, 16, 32, 64];

  public isSeller = false;
  public sellerId: string | undefined;
  public searchKey: string | undefined;

  constructor(
    private gigsClient: GigsClient,
    public dialog: MatDialog,
    private sellerClient: SellersClient,
    private authService: AuthService
  ) { }

  ngOnInit(): void {
    this.isSeller = this.authService.user?.profile.role.includes('Seller')
    this.fetchGigs();
  }

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  fetchGigs() {
    if (this.isSeller) {
      this.sellerClient.getUserSellerId()
        .pipe(
          takeUntil(this.unsubscribe),
          tap({ next: (id) => this.sellerId = id }),
          switchMap(id => this.sellerClient.getMyGigs(
            this.pageSize,
            this.pageNumber + 1,
            this.searchKey,
            id)),
          tap({
            next: (gigs) => {
              this.pushNewGigs(gigs);
          } }))
        .subscribe();
    }
    else {
      this.gigsClient.getRandom(this.pageNumber + 1, this.pageSize, this.searchKey)
        .pipe(
          takeUntil(this.unsubscribe),
          tap({
            next: (gigs) => {
              this.pushNewGigs(gigs)
          } }))
        .subscribe();
    }
  }

  onCreateNew() {
    const ref = this.dialog.open(GigCreateComponent, {
      width: '50%'
    });

    ref.afterClosed()
      .pipe(
        takeUntil(this.unsubscribe),
        switchMap(id => {
          debugger;
          return this.gigsClient.getGigById(id);
        }),
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

  pageChange(pageEvent: PageEvent) {
    this.pageSize = pageEvent.pageSize;
    this.pageNumber = pageEvent.pageIndex;

    this.fetchGigs();
  }

  applyFilter(event: any) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.searchKey = filterValue.trim().toLowerCase();
    this.fetchGigs()
  }
  
  private pushNewGigs(gigs: PaginatedListOfGigOverviewDto) {
    this.gigs = gigs.items!;
    this.length = gigs.totalCount!;
    this.gigsSubject.next(this.gigs);
  }
}
