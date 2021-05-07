import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from '../core/guards/auth.guard';

import { LayoutComponent } from '../shared/layout/layout.component';
import { GigsControlComponent } from './gigs-control/gigs-control.component';
import { GigsCreateComponent } from './gigs-create/gigs-create.component';
import { GigsDetailsComponent } from './gigs-details/gigs-details.component';

const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children: [
      { path: '', component: GigsControlComponent },
      { path: ':id/details', component: GigsDetailsComponent },
      { path: 'create', component: GigsCreateComponent },
      {
        path: 'categories',
        loadChildren: './categories/categories.module#CategoriesModule',
        canActivate: [AuthGuard],
        data: {
          role: 'Admin'
        }
      },
    ]
  },
  
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GigManagementRoutingModule { }