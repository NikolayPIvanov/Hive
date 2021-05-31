import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LayoutComponent } from './layout/layout.component';
import { MaterialModule } from '../material/material.module';
import { RouterModule } from '@angular/router';
import { FlexLayoutModule } from '@angular/flex-layout';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SignedOutLayoutComponent } from './signed-out-layout/signed-out-layout.component';
import { LayoutSidenavComponent } from './layout/layout-sidenav/layout-sidenav.component';
import { LimitToPipe } from './pipes/limit-to.pipe';



@NgModule({
  declarations: [
    LayoutComponent,
    SignedOutLayoutComponent,
    LayoutSidenavComponent,
    LimitToPipe
  ],
  imports: [
    CommonModule,

    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    FlexLayoutModule,

    MaterialModule
  ],
  exports: [
    FormsModule,
    ReactiveFormsModule,
    FlexLayoutModule,
    MaterialModule,

    LimitToPipe
  ]
})
export class LayoutModule { }
