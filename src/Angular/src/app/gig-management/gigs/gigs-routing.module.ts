import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { GigsControlComponent } from './gigs-control/gigs-control.component';
import { GigsDetailsComponent } from './gigs-details/gigs-details.component';

const routes: Routes = [
  {
    path: '',
    children: [
      { path: 'list', component: GigsControlComponent },
      { path: ':id/details', component: GigsDetailsComponent },
    ]
  }
];


@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GigsRoutingModule { }
