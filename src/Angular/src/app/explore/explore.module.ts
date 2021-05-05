import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { CategoriesTreeComponent } from './categories-tree/categories-tree.component';
import { ExploreOverviewComponent } from './explore-overview/explore-overview.component';
import { ExporeRoutingModule } from './explore-routing.module';



@NgModule({
  declarations: [
    CategoriesTreeComponent,
    ExploreOverviewComponent
  ],
  imports: [
    CommonModule,
    ExporeRoutingModule,
    SharedModule
  ]
})
export class ExploreModule { }
