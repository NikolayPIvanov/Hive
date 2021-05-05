import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { LayoutComponent } from '../shared/layout/layout.component';
import { GigsControlComponent } from './gigs-control/gigs-control.component';
import { GigsDetailsComponent } from './gigs-details/gigs-details.component';

const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children: [
      { path: '', component: GigsControlComponent },
      { path: ':id/details', component: GigsDetailsComponent}
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GigManagementRoutingModule { }