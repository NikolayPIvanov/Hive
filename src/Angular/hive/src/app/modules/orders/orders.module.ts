import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrdersRoutingModule } from './orders-routing.module';
import { CheckoutComponent } from './components/checkout/checkout.component';
import { LayoutModule } from '../layout/layout.module';
import { OrderSummaryComponent } from './components/order-summary/order-summary.component';
import { NgxSpinnerModule } from 'ngx-spinner';
import { CheckoutSummaryComponent } from './components/checkout-summary/checkout-summary.component';
import { OrderPlacedComponent } from './components/order-placed/order-placed.component';



@NgModule({
  declarations: [
    CheckoutComponent,
    OrderSummaryComponent,
    CheckoutSummaryComponent,
    OrderPlacedComponent
  ],
  imports: [
    CommonModule,

    LayoutModule,

    OrdersRoutingModule,

    NgxSpinnerModule
  ]
})
export class OrdersModule { }
