import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../core/guards/auth.guard';
import { LayoutComponent } from '../layout/layout/layout.component';
import { CheckoutComponent } from './components/checkout/checkout.component';
import { OrderPlacedComponent } from './components/order-placed/order-placed.component';
import { OrdersListComponent } from './components/orders-list/orders-list.component';

const routes: Routes = [
    {
        path: '',
        component: LayoutComponent,
        children: [
            {
                path: 'gigs/:gigId/packages/:id/checkout',
                component: CheckoutComponent
            },
            {
                path: ':id/placed',
                component: OrderPlacedComponent
            },
            {
                path: 'orders',
                component: OrdersListComponent
            }
        ]
    }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class OrdersRoutingModule { }
