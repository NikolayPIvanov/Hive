import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { GigsListComponent } from './components/gigs-list/gigs-list.component';

const routes: Routes = [
  {
    path: '',
    children: [
      { path: ':id/list', component: GigsListComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GigsRoutingModule { }
