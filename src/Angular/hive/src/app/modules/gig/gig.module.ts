import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GigDashboardComponent } from './components/gig-dashboard/gig-dashboard.component';
import { LayoutModule } from '../layout/layout.module';
import { GigRoutingModule } from './gig-routing.module';
import { GigDetailsComponent } from './components/gig-details/gig-details.component';
import { GigsControlComponent } from './components/gigs-control/gigs-control.component';
import { GigCardComponent } from './components/gig-card/gig-card.component';
import { GigEditComponent } from './components/gig-edit/gig-edit.component';
import { CategoryModule } from '../category/category.module';
import { SellerOverviewComponent } from './components/seller-overview/seller-overview.component';
import { AvatarModule } from 'ngx-avatar';
import { GigCreateComponent } from './components/gig-create/gig-create.component';



@NgModule({
  declarations: [
    GigDashboardComponent,
    GigDetailsComponent,
    GigsControlComponent,
    GigCardComponent,
    GigEditComponent,
    SellerOverviewComponent,
    GigCreateComponent
  ],
  imports: [
    CommonModule,

    LayoutModule,

    GigRoutingModule,

    AvatarModule
  ]
})
export class GigModule { }
