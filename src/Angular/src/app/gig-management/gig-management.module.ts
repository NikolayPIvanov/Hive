import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { DynFormsMaterialModule } from '@myndpm/dyn-forms/ui-material';
import { CategoriesModule } from './categories/categories.module';
import { GigsModule } from './gigs/gigs.module';
import { GigManagementRoutingModule } from './gig-management-routing.module';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    SharedModule,

    CategoriesModule,
    GigsModule,
    
    GigManagementRoutingModule,
    DynFormsMaterialModule.forFeature()
  ]
})
export class GigManagementModule { }
