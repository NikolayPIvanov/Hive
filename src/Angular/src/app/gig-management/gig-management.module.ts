import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { GigManagementRoutingModule } from './gig-management-routing.module';
import { GigsControlComponent } from './gigs-control/gigs-control.component';



@NgModule({
  declarations: [
    GigsControlComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    GigManagementRoutingModule
  ]
})
export class GigManagementModule { }
