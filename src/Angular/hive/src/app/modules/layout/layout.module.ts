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
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptorService } from './services/auth-interceptor.service';
import { NgxSpinnerModule } from "ngx-spinner";
import { ImageUploadComponent } from './components/image-upload/image-upload.component';

import { ChartsModule } from 'ng2-charts';


@NgModule({
  declarations: [
    LayoutComponent,
    SignedOutLayoutComponent,
    LayoutSidenavComponent,
    LimitToPipe,

    CategoriesSearchComponent,
     ImageUploadComponent
  ],
  imports: [
    CommonModule,

    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    FlexLayoutModule,
    CreditCardDirectivesModule,
    NgxSpinnerModule,

    MaterialModule,

    ChartsModule
  ],
  exports: [
    FormsModule,
    ReactiveFormsModule,
    FlexLayoutModule,
    MaterialModule,
    NgxSpinnerModule,

    LimitToPipe,

    CategoriesSearchComponent,
    ImageUploadComponent,
    ChartsModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptorService, multi: true }
  ]
})
export class LayoutModule { }
