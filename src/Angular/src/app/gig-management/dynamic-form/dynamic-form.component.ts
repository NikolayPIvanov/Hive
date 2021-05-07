import { AfterViewInit, Component, VERSION, ViewChild, OnInit } from '@angular/core';
import { FormArray, FormControl, FormGroup } from '@angular/forms';
import { DynFormComponent } from '@myndpm/dyn-forms';
import { simpleData, simpleForm } from './simple.form';

@Component({
  selector: 'app-dynamic-form',
  templateUrl: './dynamic-form.component.html',
  styleUrls: ['./dynamic-form.component.scss']
})
export class DynamicFormComponent implements AfterViewInit {

   // dyn-form inputs
   config = simpleForm;
   form = new FormGroup({});
   mode = 'edit';
 
   @ViewChild(DynFormComponent, { static: true })
   dynForm!: DynFormComponent;
 
   ngAfterViewInit(): void {
     // logs each debounced change in the console just to demo
     this.dynForm.valueChanges().subscribe(console.log);
   }
 
   loadData(): void {
     // we can load data AfterViewInit
     this.dynForm.patchValue(simpleData);
   }
 
   toggleMode(): void {
     this.mode = (this.mode === 'edit') ? 'display' : 'edit';
 
     if (this.mode === 'display') {
       // reset invalid styles on display markAllAsPristine
       this.markAsUntouched(this.form);
     }
   }
 
   private markAsUntouched(group: FormGroup | FormArray): void {
     group.markAsUntouched();
 
     Object.keys(group.controls).map((field) => {
       const control = group.get(field);
       if (control instanceof FormControl) {
         control.markAsUntouched();
       } else if (control instanceof FormGroup) {
         this.markAsUntouched(control);
       }
     });
   }

}
