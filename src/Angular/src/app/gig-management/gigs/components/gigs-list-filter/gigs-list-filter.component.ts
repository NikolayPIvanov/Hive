import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';

@Component({
  selector: 'app-gigs-list-filter',
  templateUrl: './gigs-list-filter.component.html',
  styleUrls: ['./gigs-list-filter.component.scss']
})
export class GigsListFilterComponent implements OnInit {
  
  lowerPriceControl = new FormControl(1.0, [Validators.required, Validators.min(1.0)]);
  upperPriceControl = new FormControl(10000, [Validators.required, Validators.max(10000)]);

  myControl = new FormControl();
  options: string[] = ['One', 'Two', 'Three'];
  filteredOptions!: Observable<string[]>;
  
  constructor() { }

  ngOnInit(): void {
    this.filteredOptions = this.myControl.valueChanges.pipe(
      startWith(''),
      map(value => this._filter(value))
    );
  }

  formatLabel(value: number) {
    if (value >= 1000) {
      return Math.round(value / 1000) + '$';
    }

    return value;
  }
  
  private _filter(value: string): string[] {
    const filterValue = value.toLowerCase();

    return this.options.filter(option => option.toLowerCase().indexOf(filterValue) === 0);
  }
}
