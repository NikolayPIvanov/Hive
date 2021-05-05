import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { GigManagementRoutingModule } from './gig-management-routing.module';
import { GigsControlComponent } from './gigs-control/gigs-control.component';
import { GigsDetailsComponent } from './gigs-details/gigs-details.component';



@NgModule({
  declarations: [
    GigsControlComponent,
    GigsDetailsComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    GigManagementRoutingModule
  ]
})
export class GigManagementModule { }
