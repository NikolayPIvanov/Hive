import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { GigsControlPanelComponent } from './components/gigs-control-panel/gigs-control-panel.component';
import { GigsListComponent } from './components/gigs-list/gigs-list.component';

const routes: Routes = [
  {
    path: '',
    children: [
      { path: ':id/list', component: GigsListComponent },
      { path: 'control', component: GigsControlPanelComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GigsRoutingModule { }
