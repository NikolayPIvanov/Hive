import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { ExporeRoutingModule } from './explore-routing.module';
import { ExploreOverviewComponent } from './explore-overview/explore-overview.component';
import { GigsModule } from '../gig-management/gigs/gigs.module';

@NgModule({
  declarations: [
  
    ExploreOverviewComponent
  ],
  imports: [
    CommonModule,
    ExporeRoutingModule,
    SharedModule,
    GigsModule
  ]
})
export class ExploreModule { }
