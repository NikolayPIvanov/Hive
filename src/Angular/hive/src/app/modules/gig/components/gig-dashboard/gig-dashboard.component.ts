import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';
import { GigOverviewDto, GigsClient } from 'src/app/clients/gigs-client';

@Component({
  selector: 'app-gig-dashboard',
  templateUrl: './gig-dashboard.component.html',
  styleUrls: ['./gig-dashboard.component.scss']
})
export class GigDashboardComponent implements OnInit {
  private gigs: GigOverviewDto[] = [];
  private gigsSubject = new BehaviorSubject<GigOverviewDto[]>(this.gigs);
  public gigs$ = this.gigsSubject.asObservable();

  // Pagination
  // MatPaginator Inputs
  length = 100;
  pageSize = 8;
  pageNumber = 0;
  pageSizeOptions: number[] = [4, 8, 16, 64];

  pageChange(pageEvent: PageEvent) {
    this.pageSize = pageEvent.pageSize;
    this.pageNumber = pageEvent.pageIndex;

    this.gigsClient.getRandom(10)
      .pipe(tap({
        next: (gigs) => {
          this.gigs = gigs.items!;
          this.gigsSubject.next(this.gigs);
        }
      }))
      .subscribe();
  }

  constructor(
    private router: Router,
    private gigsClient: GigsClient
  ) { }

  ngOnInit(): void {
    this.gigsClient.getRandom(10)
      .pipe(tap({
        next: (gigs) => {
          this.gigs = gigs.items!;
          this.length = gigs.totalCount!
          this.gigsSubject.next(this.gigs);
        }
      }))
      .subscribe();
  }

}
