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
        switchMap(val => this.filter(val || '')),
        map(options => {
          if (this.categories) {
            this.csEmitter.emit(this.categories);
            this.form.setValue(this.categories[0].title!)
          }
          return options;
        })
    );
  }

  private filter(value: string): Observable<string[]> {
    return this.categoriesApiClient.getCategories(undefined, value, undefined, undefined)
     .pipe(
       map((paginatedList: PaginatedListOfCategoryDto) => 
       {
         this.categories =
           paginatedList.items!.filter(option =>
             option.title!.toLowerCase().indexOf(value.toLowerCase()) === 0);
         
        return this.categories.map(category => category.title!);
       })
     )
  }
}
