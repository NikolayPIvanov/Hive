import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { ActivatedRoute } from '@angular/router';
import { Observable, of } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';
import { CategoriesClient, CategoryDto, GigDto, GigOverviewDto, GigsClient, PaginatedListOfCategoryDto, PaginatedListOfGigOverviewDto } from 'src/app/clients/gigs-client';

@Component({
  selector: 'app-explore-overview',
  templateUrl: './explore-overview.component.html',
  styleUrls: ['./explore-overview.component.scss']
})
export class ExploreOverviewComponent implements OnInit {
  gigsList$!: Observable<PaginatedListOfGigOverviewDto>;
  categoriesList$!: Observable<PaginatedListOfCategoryDto>;
  categoryDetails$: Observable<CategoryDto> | undefined;

  private pageNumber: number = 1;
  private pageSize: number = 10;

  constructor(
    private categoriesApiClient: CategoriesClient,
    private gigsApiClient: GigsClient) { }

  ngOnInit(): void {
    this.fetchGigsList();
    this.fetchCategoriesList();
  }

  categorySelected(id: number) {
    this.fetchGigsList(id);
  }

  private fetchCategoriesList() {
    const pageNumber = 1;
    const pageSize = 10;
    this.categoriesList$ = this.categoriesApiClient.getCategories(pageNumber, pageSize, true, null)
  }

  private fetchCategory(id: number) {
    this.categoryDetails$ = this.categoriesApiClient.getCategoryById(id);
  }

  private fetchGigsList(categoryId: number | undefined = undefined) {
    debugger;
    if (categoryId) {
      this.gigsList$ = this.categoriesApiClient
        .getCategoryGigs(+categoryId, this.pageNumber, this.pageSize, undefined);
      this.fetchCategory(+categoryId);
    }
    else {
      this.gigsList$ = this.gigsApiClient.getRandom(10).pipe(map(x => {
        debugger;
        console.log(x);

        return x;
      }));
    }
    
  }
}
