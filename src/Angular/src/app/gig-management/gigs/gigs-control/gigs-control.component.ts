import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { PageEvent } from '@angular/material/paginator';
import { Observable, Subscription } from 'rxjs';
import { map, mergeAll, startWith, switchMap } from 'rxjs/operators';
import { GigDto, GigOverviewDto, PaginatedListOfGigOverviewDto, SellersClient } from 'src/app/clients/gigs-client';


@Component({
  selector: 'app-gigs-control',
  templateUrl: './gigs-control.component.html',
  styleUrls: ['./gigs-control.component.scss']
})
export class GigsControlComponent implements OnInit {
  pageSize = 10
  pageIndex = 0
  pageSizeOptions = [5, 10, 25]
  paginatedList$: Observable<PaginatedListOfGigOverviewDto> | undefined;

  constructor(private sellersApiClient: SellersClient) { }

  ngOnInit(): void {
    this.paginatedList$ = this.sellersApiClient.getUserSellerId()
      .pipe(
        switchMap((sellerId: string) => this.sellersApiClient.getMyGigs(this.pageSize, this.pageIndex + 1, sellerId)))
  }

  onFilteredEvents($event: GigOverviewDto[]) {
    // todo: navigate and open up the gig
  }

  handlePageEvent(event: PageEvent) {
    this.pageSize = event.pageSize;
    this.pageIndex = event.pageIndex;
    if (this.pageIndex == 0) {
      
    }
    
    this.paginatedList$ = this.sellersApiClient.getUserSellerId()
      .pipe(
        switchMap((sellerId: string) => this.sellersApiClient.getMyGigs(this.pageSize, this.pageIndex + 1, sellerId)))

  }
}
