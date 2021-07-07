import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminGuard } from '../core/guards/admin.guard';
import { AuthGuard } from '../core/guards/auth.guard';
import { LayoutComponent } from '../layout/layout/layout.component';
import { GigDashboardComponent } from './components/gig-dashboard/gig-dashboard.component';
import { GigDetailsComponent } from './components/gig-details/gig-details.component';
import { GigsControlComponent } from './components/gigs-control/gigs-control.component';

const routes: Routes = [
    {
        path: '',
        component: LayoutComponent,
        children: [
            {
                path: 'control',
                component: GigsControlComponent,
                canActivate: [AuthGuard]
            },
            {
                path: 'dashboard',
                component: GigDashboardComponent,
                canActivate: [AuthGuard]
            },
            {
                path: ':id/details',
                component: GigDetailsComponent,
                canActivate: [AuthGuard]
            }
        ]
    }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GigRoutingModule { }
