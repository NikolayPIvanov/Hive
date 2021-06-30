import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './components/home/home.component';
import { HomeRoutingModule } from './home-routing.module';
import { LayoutModule } from '../layout/layout.module';
import { CoreModule } from '@angular/flex-layout';
import { AdminDashboardComponent } from './components/admin-dashboard/admin-dashboard.component';
import { SellerDashboardComponent } from './components/seller-dashboard/seller-dashboard.component';
import { BuyerDashboardComponent } from './components/buyer-dashboard/buyer-dashboard.component';
import { InvestorDashboardComponent } from './components/investor-dashboard/investor-dashboard.component';

@NgModule({
  declarations: [
    HomeComponent,
    AdminDashboardComponent,
    SellerDashboardComponent,
    BuyerDashboardComponent,
    InvestorDashboardComponent
  ],
  imports: [
    CommonModule,

    CoreModule,
    LayoutModule,

    HomeRoutingModule
  ]
})
export class HomeModule { }
