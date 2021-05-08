import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Observable } from 'rxjs';
import { map, mergeAll, startWith, switchMap } from 'rxjs/operators';
import { GigDto, SellersClient } from 'src/app/clients/gigs-client';
import { QuestionBase, TextboxQuestion } from '../../questions-list/questions-list.component';

export interface MyUser {
  name: string;
}

@Component({
  selector: 'app-gigs-control',
  templateUrl: './gigs-control.component.html',
  styleUrls: ['./gigs-control.component.scss']
})
export class GigsControlComponent implements OnInit {
  gigs$!: Observable<GigDto[]>;
  myControl = new FormControl();
  options: MyUser[] = [
    {name: 'Mary'},
    {name: 'Shelley'},
    {name: 'Igor'}
  ];

  qs: QuestionBase<string>[] = [
    new TextboxQuestion({
      key: 'firstName',
      label: 'First name',
      value: 'Bombasto',
      required: true,
      order: 1
    })
  ]

  filteredOptions!: Observable<MyUser[]>;

  constructor(private sellersApiClient: SellersClient) { }

  ngOnInit(): void {
    this.gigs$ = this.sellersApiClient.getUserSellerId()
      .pipe(switchMap((sellerId: string) => this.sellersApiClient.getMyGigs(sellerId)));
    
    this.filteredOptions = this.myControl.valueChanges
      .pipe(
        startWith(''),
        map(value => typeof value === 'string' ? value : value.name),
        map(name => name ? this._filter(name) : this.options.slice())
      );
  }

  displayFn(user: MyUser): string {
    return user && user.name ? user.name : '';
  }

  private _filter(name: string): MyUser[] {
    const filterValue = name.toLowerCase();

    return this.options.filter(option => option.name.toLowerCase().indexOf(filterValue) === 0);
  }

}
