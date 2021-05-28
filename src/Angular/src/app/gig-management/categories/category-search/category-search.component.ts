import { AfterViewInit, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged, filter, map, switchMap, tap } from 'rxjs/operators';
import { CategoriesClient, CategoryDto, PaginatedListOfCategoryDto } from 'src/app/clients/gigs-client';

@Component({
  selector: 'app-category-search',
  templateUrl: './category-search.component.html',
  styleUrls: ['./category-search.component.scss']
})
export class CategorySearchComponent implements OnInit {
  @Input('init') initCategory: CategoryDto | undefined;
  @Output('filteredEmitter') csEmitter = new EventEmitter<CategoryDto[]>();
  form = new FormControl();
  filteredOptions!: Observable<string[]>;

  private categories: CategoryDto[] = [];
  
  constructor(private categoriesApiClient: CategoriesClient) { }

  ngOnInit() {
    if (this.initCategory) {
      this.categories = [this.initCategory];
      this.form.setValue(this.initCategory.title!)
    }

    this.filteredOptions = this.form.valueChanges
      .pipe(
        filter(value => value != ''),
        debounceTime(400),
        distinctUntilChanged(),
        switchMap(value => this.filter(value)),
        map(options => {
          if (this.categories) {
            this.csEmitter.emit(this.categories);
          }
          if (this.categories && this.categories.length == 0) {
            this.form.setValue(this.categories[0].title!)
          }
          return options;
        })
    );
  }

  clear(): void {
    this.form.setValue(null);
  }

  private filter(value: string): Observable<string[]> {
    return this.categoriesApiClient.getCategories(undefined, undefined, undefined, value)
     .pipe(
       map((paginatedList: PaginatedListOfCategoryDto) => 
       {
        this.categories = paginatedList.items!
        return this.categories.map(category => category.title!);
       })
     )
  }
}
