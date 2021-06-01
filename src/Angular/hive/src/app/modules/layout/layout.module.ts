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
import { CreditCardDirectivesModule } from 'angular-cc-library';
import { CategoriesSearchComponent } from './components/categories-search/categories-search.component';

@NgModule({
  declarations: [
    LayoutComponent,
    SignedOutLayoutComponent,
    LayoutSidenavComponent,
    LimitToPipe,

    CategoriesSearchComponent
  ],
  imports: [
    CommonModule,

    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    FlexLayoutModule,
    CreditCardDirectivesModule,

    MaterialModule
  ],
  exports: [
    FormsModule,
    ReactiveFormsModule,
    FlexLayoutModule,
    MaterialModule,

    LimitToPipe,

    CategoriesSearchComponent
  ]
})
export class LayoutModule { }
