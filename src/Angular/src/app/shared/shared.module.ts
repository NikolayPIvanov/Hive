import { NgModule } from '@angular/core';
import { LayoutComponent } from './layout/layout.component';
import { RouterModule } from '@angular/router';
import { CustomMaterialModule } from '../custom-material/custom-material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';
import { LoggedoutLayoutComponent } from './loggedout-layout/loggedout-layout.component';
import { ChipInputComponent } from './components/chip-input/chip-input.component';

@NgModule({
  declarations: [LayoutComponent, LoggedoutLayoutComponent, ChipInputComponent],
  imports: [
    RouterModule,
    CustomMaterialModule,
    FormsModule,
    ReactiveFormsModule,
    FlexLayoutModule,
  ],
  exports: [
    CustomMaterialModule,
    FormsModule,
    ReactiveFormsModule,
    FlexLayoutModule,

    ChipInputComponent
  ]
})
export class SharedModule { }
