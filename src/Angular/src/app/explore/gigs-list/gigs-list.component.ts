import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { Observable } from 'rxjs';
import { filter, mergeMap } from 'rxjs/operators';
import { CategoriesClient, GigsClient, PaginatedListOfGigDto } from 'src/app/clients/gigs-client';
import { ExploreService } from '../explore.service';

@Component({
  selector: 'app-gigs-list',
  templateUrl: './gigs-list.component.html',
  styleUrls: ['./gigs-list.component.scss']
})
export class GigsListComponent implements OnInit {
  length = 500;
  pageSize = 10;
  pageIndex = 1;
  pageSizeOptions = [5, 10, 25];
  showFirstLastButtons = true;
  paginatedList: PaginatedListOfGigDto | undefined;

  private fetchGigs() {
    this.exploreService.selectedCategory$
    .pipe(
      filter(id => id != 0),
      mergeMap(id => this.categoryApiClient.getCategoryGigs(id, this.pageIndex, this.pageSize))
    )
    .subscribe(pagedList => this.paginatedList = pagedList)
  }
  
  constructor(private exploreService: ExploreService,
              private categoryApiClient: CategoriesClient) { }

  ngOnInit(): void {
    this.fetchGigs();
  }

  handlePageEvent(event: PageEvent) {
    this.length = event.length;
    this.pageSize = event.pageSize;
    if (event.pageIndex == 0) {
      this.pageIndex = 1;
    }
    else {
      this.pageIndex = event.pageIndex;
    }

    this.fetchGigs();
  }

}
