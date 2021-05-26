import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { Component, Input, OnInit } from '@angular/core';
import { AbstractControl, FormGroup } from '@angular/forms';
import { MatChipInputEvent } from '@angular/material/chips';

@Component({
  selector: 'shared-chip-input',
  templateUrl: './chip-input.component.html',
  styleUrls: ['./chip-input.component.scss']
})
export class ChipInputComponent implements OnInit {
  @Input('title') title: string = '';
  @Input('form') form!: FormGroup;
  @Input('name') name!: string;
  
  visible = true;
  selectable = true;
  removable = true;
  addOnBlur = true;
  readonly separatorKeysCodes = [ENTER, COMMA] as const;

  constructor() { }

  ngOnInit(): void {
  }

  get items(): AbstractControl | null  {
    return this.form.get(this.name);
  }

  add(event: MatChipInputEvent): void {
    const value = (event.value || '').trim();
    if (value) {
      this.items!.setValue([...this.items!.value, value])
      this.items!.updateValueAndValidity();
    }

    if (event.input) {
      event.input.value = '';
    }
  }

  remove(item: any): void {
    const index = this.items!.value.indexOf(item);

    if (index >= 0) {
      this.items!.value.splice(index, 1);
      this.items!.updateValueAndValidity();
    }
  }
}
