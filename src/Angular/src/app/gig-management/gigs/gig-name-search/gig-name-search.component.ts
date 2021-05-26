import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Observable, of } from 'rxjs';
import { startWith, debounceTime, distinctUntilChanged, switchMap, map, filter } from 'rxjs/operators';
import { CategoriesClient, GigDto, GigOverviewDto, GigsClient } from 'src/app/clients/gigs-client';

class Searchable<TId, TKey> {
  id!: TId;
  key!: TKey;

  constructor(id: TId, key: TKey) {
    this.id = id;
    this.key = key;
  }
}

@Component({
  selector: 'app-gig-name-search',
  templateUrl: './gig-name-search.component.html',
  styleUrls: ['./gig-name-search.component.scss']
})
export class GigNameSearchComponent implements OnInit {
  @Input('init') initCategory: GigDto | undefined;
  // @Input('categoryId') categoryId: number;
  @Output('filteredEmitter') csEmitter = new EventEmitter<GigDto[]>();

  gigs: GigOverviewDto[] | undefined;
  control = new FormControl();
  filteredOptions: Observable< Searchable<number, string>[] | undefined>;

  constructor(private gigsApiClient: CategoriesClient) {
    this.filteredOptions = this.control.valueChanges.pipe(
      filter(val => val.length >= 3),
      debounceTime(400),
      distinctUntilChanged(),
      switchMap((val: string) => this.filter(val)),
      switchMap((options: Searchable<number, string>[] | undefined) => {
        this.notify()
        return of(options);
      }))
  }

  ngOnInit(): void {
  }

  private notify() {
    this.csEmitter.emit(this.gigs);
  }

  private filter(option: string): Observable<Searchable<number, string>[] | undefined> {
    if (!option) {
      this.control.setValue('');
      return of(undefined);
    }

    const filtered = this.gigs?.filter((value => {
      return value.title?.includes(option)
    }));

    const callApi = !filtered

    // if (callApi) {
    //   return this.gigsApiClient.getCategoryGigs(option)
    //   .pipe(map((response: GigOverviewDto[]) => {
    //     this.gigs = response;
    //     return response.map(g => new Searchable<number, string>(g.id!, g.title!))
    //   }));
    // }
    // else {
      this.gigs = filtered!;
      return of(filtered!.map(g => new Searchable<number, string>(g.id!, g.title!)));
    // }
   }

}
