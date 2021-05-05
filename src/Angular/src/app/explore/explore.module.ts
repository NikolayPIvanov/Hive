import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { CategoriesTreeComponent } from './categories-tree/categories-tree.component';
import { ExploreOverviewComponent } from './explore-overview/explore-overview.component';
import { ExporeRoutingModule } from './explore-routing.module';
import { GigsListComponent } from './gigs-list/gigs-list.component';



@NgModule({
  declarations: [
    CategoriesTreeComponent,
    ExploreOverviewComponent,
    GigsListComponent
  ],
  imports: [
    CommonModule,
    ExporeRoutingModule,
    SharedModule
  ]
})
export class ExploreModule { }
