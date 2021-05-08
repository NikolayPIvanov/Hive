import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { GigsControlComponent } from './gigs-control/gigs-control.component';

const routes: Routes = [
  {
    path: '',
    children: [
      { path: 'list', component: GigsControlComponent },
    ]
  }
];


@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GigsRoutingModule { }
