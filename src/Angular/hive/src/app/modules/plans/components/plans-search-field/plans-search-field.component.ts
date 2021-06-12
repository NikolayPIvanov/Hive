import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, startWith, switchMap } from 'rxjs/operators';
import { PlanDto } from 'src/app/clients/investing-client';
import { PlansService } from '../../services/plans.service';

@Component({
  selector: 'app-plans-search-field',
  templateUrl: './plans-search-field.component.html',
  styleUrls: ['./plans-search-field.component.scss']
})
export class PlansSearchFieldComponent implements OnInit {
  @Input() selfOnly: boolean = true;
  @Input() init: string | null = null;
  @Output() onSelected = new EventEmitter<PlanDto | undefined>();

  autocompleteControl = new FormControl('');
  filteredOptions: Observable<PlanDto[]> | undefined;

  constructor(private plansService: PlansService) { }

  ngOnInit(): void {
    this.autocompleteControl.setValue(this.init);

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

  onSelectedEvent($event: MatAutocompleteSelectedEvent) {
    this.onSelected.emit($event.option.value)
  }

  onCleared($event: any) {
    this.autocompleteControl.setValue(null);
    this.onSelected.emit(undefined);
  }

  displayFn(plan: PlanDto): string {
    return plan ? plan.title! : ''
  }

  private filter(val: string): Observable<any[]> {
    const source = this.selfOnly ?
      this.plansService.getPlansAsVendor(val) :
      this.plansService.getPlansAsVendor(val) // change to random
        
    return source
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
}
