import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from '../core/guards/auth.guard';

import { LayoutComponent } from '../shared/layout/layout.component';

const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children: [
      {
        path: 'categories',
        loadChildren: './categories/categories.module#CategoriesModule',
        canActivate: [AuthGuard],
        data: {
          role: 'Admin'
        }
      },
      {
        path: 'gigs',
        loadChildren: './gigs/gigs.module#GigsModule',
        canActivate: [AuthGuard]
      },
      { path: '', redirectTo: '/', pathMatch: 'full'}
    ]
  },
  
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GigManagementRoutingModule { }