import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { PackageDto } from 'src/app/clients/gigs-client';
import { OrdersClient, PlaceOrderCommand } from 'src/app/clients/ordering-client';
import { CheckoutService } from '../../services/checkout.service';

@Component({
  selector: 'app-checkout-summary',
  templateUrl: './checkout-summary.component.html',
  styleUrls: ['./checkout-summary.component.scss']
})
export class CheckoutSummaryComponent implements OnInit {
  spinnerName = 'checkoutorder'

  totalPrice = 0.0;
  package$!: Observable<PackageDto | undefined>;
  quantity$!: Observable<number>;
  
  constructor(private checkoutService: CheckoutService,
    private ordersApiClient: OrdersClient,
    private spinner: NgxSpinnerService,
    private router: Router) { }

  ngOnInit(): void {
    this.package$ = this.checkoutService.checkoutPackage$;
    this.quantity$ = this.checkoutService.orderQuantity$;

    this.spinner.show(this.spinnerName)

    setTimeout(() => {
      this.spinner.hide(this.spinnerName);
    }, 2000)
  }
  
  completeOrder(quantity: number, p: PackageDto) {
    // TODO: Get seller id with package
    // mock
    this.router.navigate(['orders/', 1, 'placed'])

    const command = PlaceOrderCommand.fromJS(
      { unitPrice: p.price, requirements: '', sellerUserId: 1, packageId: p.id });
    this.spinner.show(this.spinnerName);
    this.ordersApiClient.placeOrder(command)
      .pipe(tap({
        next: (orderNumber) => {
          this.spinner.hide(this.spinnerName);
          this.router.navigate(['orders/', orderNumber, 'placed'])
        },
        error: (error) => {
          this.spinner.hide(this.spinnerName);
        }
      }))
  }

}
