import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/shared/shared.module';
import { GigsRoutingModule } from './gigs-routing.module';
import { GigNameSearchComponent } from './gig-name-search/gig-name-search.component';
import { GigsControlComponent } from './gigs-control/gigs-control.component';
import { GigCardOverviewComponent } from './gig-card-overview/gig-card-overview.component';


@NgModule({
  declarations: [
    GigNameSearchComponent,
    GigsControlComponent,
    GigCardOverviewComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    GigsRoutingModule
  ],
  exports: [
    GigCardOverviewComponent
  ]
})
export class GigsModule { }
