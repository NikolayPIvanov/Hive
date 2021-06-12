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
  @Output() onSelectedCategory = new EventEmitter<CategoryDto>();

  autocompleteControl = new FormControl('');
  filteredOptions: Observable<CategoryDto[]> | undefined;

  constructor(private categoriesApiClient: CategoriesClient) { }

  ngOnInit(): void {
    if (this.init) {
      this.categoriesApiClient.getCategories(1, 1, true, this.init)
        .subscribe(list => {
          if (list && list.items) {
            this.autocompleteControl.setValue(list.items![0]);
          }

        })
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
    return this.categoriesApiClient.getCategories(1, 5, true, val)
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

  displayFunc = (selected: any) => {
    return selected ? selected.title : undefined;
  }

  onCleared() {
    this.autocompleteControl.setValue('')
    this.onSelectedCategory.emit(undefined)
  }

  onSelected($event: MatAutocompleteSelectedEvent) {
    this.onSelectedCategory.emit($event.option.value)
  }
}
