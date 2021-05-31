import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GigDashboardComponent } from './components/gig-dashboard/gig-dashboard.component';
import { LayoutModule } from '../layout/layout.module';
import { GigRoutingModule } from './gig-routing.module';
import { GigDetailsComponent } from './components/gig-details/gig-details.component';



@NgModule({
  declarations: [
    GigDashboardComponent,
    GigDetailsComponent
  ],
  imports: [
    CommonModule,

    LayoutModule,

    GigRoutingModule
  ]
})
export class GigModule { }
