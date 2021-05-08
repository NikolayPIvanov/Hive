import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { CategoriesClient, CategoryDto, PaginatedListOfCategoryDto } from 'src/app/clients/gigs-client';

@Component({
  selector: 'app-explore-overview',
  templateUrl: './explore-overview.component.html',
  styleUrls: ['./explore-overview.component.scss']
})
export class ExploreOverviewComponent implements OnInit {
  
  categories$!: Observable<CategoryDto[] | undefined>;
  numbers: any[];

  constructor(private categoriesApiClient: CategoriesClient) {
    this.numbers = Array(10).fill(4); // [4,4,4,4,4]
  }

  ngOnInit(): void {
    this.categories$ = this.categoriesApiClient.getCategories(1, 5, true)
      .pipe(map((list: PaginatedListOfCategoryDto) => list.items));
  }

}
