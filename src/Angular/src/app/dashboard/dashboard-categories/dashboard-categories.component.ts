import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { Observable } from 'rxjs';
import { CategoriesClient } from 'src/app/clients/gigs-client';

@Component({
  selector: 'app-dashboard-categories',
  templateUrl: './dashboard-categories.component.html',
  styleUrls: ['./dashboard-categories.component.scss']
})
export class DashboardCategoriesComponent implements OnInit {
  length = 500;
  pageSize = 10;
  pageIndex = 1;
  pageSizeOptions = [5, 10, 25];
  showFirstLastButtons = true;
  
  categories$!: Observable<any>;

  handlePageEvent(event: PageEvent) {
    this.length = event.length;
    this.pageSize = event.pageSize;
    if (event.pageIndex == 0) {
      this.pageIndex = 1;
    }
    else {
      this.pageIndex = event.pageIndex;
    }
    
    this.categories$ = this.categoriesApiClient.getCategories(false, undefined, this.pageIndex, this.pageSize);
  }
  
  constructor(private categoriesApiClient: CategoriesClient) { }

  ngOnInit(): void {
    this.categories$ = this.categoriesApiClient.getCategories(false, undefined, this.pageIndex, this.pageSize);
  }
}
