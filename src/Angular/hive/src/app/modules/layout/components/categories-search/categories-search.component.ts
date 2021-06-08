import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { Observable } from 'rxjs';
import { startWith, map, distinctUntilChanged, debounceTime, switchMap } from 'rxjs/operators';
import { CategoriesClient, CategoryDto } from 'src/app/clients/gigs-client';

@Component({
  selector: 'app-categories-search',
  templateUrl: './categories-search.component.html',
  styleUrls: ['./categories-search.component.scss']
})
export class CategoriesSearchComponent implements OnInit {
  @Input() init: string | null = null;
  @Input() includeParents: boolean = true;
  @Output() onSelectedCategoryName = new EventEmitter<string>();

  autocompleteControl = new FormControl('');
  filteredOptions: Observable<CategoryDto[]> | undefined;

  constructor(private categoriesApiClient: CategoriesClient) { }

  ngOnInit(): void {
    if (this.init) {
      this.autocompleteControl.setValue(this.init);
    }

    this.filteredOptions = this.autocompleteControl.valueChanges
      .pipe(
        startWith(''),
        debounceTime(400),
        distinctUntilChanged(),
        switchMap(val => {
          return this.filter(val || '')
        })
      );
  }

  filter(val: string): Observable<any[]> {
    return this.categoriesApiClient.getCategories(1, 5, false, val)
     .pipe(
       map(response => {
         if (response && response.items) {
           return response.items
             .filter(option => {
                return option.title!.toLowerCase().indexOf(val.toLowerCase()) === 0
              })
         }
         else {
           return [];
         }
       })
     )
  }

  onSelected($event: MatAutocompleteSelectedEvent) {
    this.onSelectedCategoryName.emit($event.option.value)
  }
}
