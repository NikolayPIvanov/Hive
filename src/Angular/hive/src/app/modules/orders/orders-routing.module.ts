import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../core/guards/auth.guard';
import { LayoutComponent } from '../layout/layout/layout.component';
import { CheckoutComponent } from './components/checkout/checkout.component';
import { OrderPlacedComponent } from './components/order-placed/order-placed.component';

const routes: Routes = [
    {
        path: '',
        component: LayoutComponent,
        children: [
            {
                path: 'checkout/:id',
                component: CheckoutComponent,
                canActivate: [AuthGuard]
            },
            {
                path: ':id/placed',
                component: OrderPlacedComponent,
                canActivate: [AuthGuard]
            }
        ]
    }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class OrdersRoutingModule { }
