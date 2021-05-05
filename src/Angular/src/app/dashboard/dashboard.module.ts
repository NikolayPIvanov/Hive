import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DashboardRoutingModule } from './dashboard-routing.module';
import { SharedModule } from '../shared/shared.module';
import { DashboardHomeComponent } from './dashboard-home/dashboard-home.component';
import { CoreModule } from '../core/core.module';
import { DashboardCategoriesComponent } from './dashboard-categories/dashboard-categories.component';

@NgModule({
  declarations: [
    DashboardHomeComponent,
    DashboardCategoriesComponent
  ],
  imports: [
    CommonModule,
    DashboardRoutingModule,
    SharedModule,
    CoreModule
  ]
})
export class DashboardModule { }
