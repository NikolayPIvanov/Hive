import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { Observable, of } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';
import { CategoriesClient, CategoryDto, GigDto, GigOverviewDto, GigsClient, PaginatedListOfCategoryDto, PaginatedListOfGigOverviewDto } from 'src/app/clients/gigs-client';

@Component({
  selector: 'app-explore-overview',
  templateUrl: './explore-overview.component.html',
  styleUrls: ['./explore-overview.component.scss']
})
export class ExploreOverviewComponent implements OnInit {
  private FIRST_PAGE = 1;
  private CATEGORY_SIZE = 5;
  private PARENT_CATEGORY_ONLY = true;
  private RANDOM_QUANTITY = 10;

  private selectedCategoryId: number | undefined;

  gigsLength: number | undefined;
  gigsPageSize = 10;
  gigsPageIndex = 0;
  pageSizeOptions = [5, this.gigsPageSize, 25];
  showFirstLastButtons = true;
 
  categories$!: Observable<CategoryDto[] | undefined>;
  gigs$!: Observable<GigOverviewDto[] | undefined>;

  constructor(
    private categoriesApiClient: CategoriesClient,
    private gigsApiClient: GigsClient) { }

  ngOnInit(): void {
    this.categories$ = this.categoriesApiClient.getCategories(this.FIRST_PAGE, this.CATEGORY_SIZE, this.PARENT_CATEGORY_ONLY)
      .pipe(map((list: PaginatedListOfCategoryDto) => list.items));
    
    this.gigs$ = this.gigsApiClient.getRandom(this.RANDOM_QUANTITY);
  }

  categorySelected(category: CategoryDto) {
    this.selectedCategoryId = category.id;
    this.gigs$ = this.categoriesApiClient.getCategoryGigs(category.id!, this.gigsPageIndex + 1, this.gigsPageSize)
      .pipe(
        switchMap((list: PaginatedListOfGigOverviewDto) => {
          this.gigsLength = list.totalCount!;
          return of(list.items);
        })
      );
  }

  handlePageEvent(event: PageEvent) {
    this.gigsPageSize = event.pageSize;
    this.gigsPageIndex = event.pageIndex;
    
    if (this.selectedCategoryId) {
      this.gigs$ = this.categoriesApiClient.getCategoryGigs(this.selectedCategoryId, this.gigsPageIndex + 1, this.gigsPageSize)
      .pipe(
        switchMap((list: PaginatedListOfGigOverviewDto) => {
          this.gigsLength = list.totalCount!;
          return of(list.items);
        })
      );
    }
    else {
      this.gigs$ = this.gigsApiClient.getRandom(this.RANDOM_QUANTITY);
    }

  }

}
