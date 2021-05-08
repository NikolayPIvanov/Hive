import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { ExporeRoutingModule } from './explore-routing.module';
import { ExploreOverviewComponent } from './explore-overview/explore-overview.component';

@NgModule({
  declarations: [
  
    ExploreOverviewComponent
  ],
  imports: [
    CommonModule,
    ExporeRoutingModule,
    SharedModule
  ]
})
export class ExploreModule { }
