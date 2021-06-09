import { Component, Input, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { GigDto, PackageDto } from 'src/app/clients/gigs-client';
import { OrdersClient, PlaceOrderCommand } from 'src/app/clients/ordering-client';
import { CheckoutService } from '../../services/checkout.service';

@Component({
  selector: 'app-checkout-summary',
  templateUrl: './checkout-summary.component.html',
  styleUrls: ['./checkout-summary.component.scss']
})
export class CheckoutSummaryComponent implements OnInit {
  @Input() gig!: GigDto;
  spinnerName = 'checkoutorder';

  totalPrice = 0.0;
  package$!: Observable<PackageDto | undefined>;
  quantity$!: Observable<number>;

  requirements = new FormControl('');
  
  constructor(private checkoutService: CheckoutService,
    private ordersApiClient: OrdersClient,
    private spinner: NgxSpinnerService,
    private router: Router) { }

  ngOnInit(): void {
    this.package$ = this.checkoutService.checkoutPackage$;
    this.quantity$ = this.checkoutService.orderQuantity$;
  }
  
  completeOrder(quantity: number, p: PackageDto) {
    const command = PlaceOrderCommand.fromJS(
      {
        unitPrice: p.price,
        quantity: quantity,
        requirements: this.requirements.value || '',
        sellerUserId: this.gig.sellerUserId!,
        packageId: p.id!
      });
    this.spinner.show(this.spinnerName);
    this.ordersApiClient.placeOrder(command)
      .pipe(tap({
        next: (orderNumber) => {
          this.router.navigate(['/orders', orderNumber, 'placed'])
        },
        complete: () => this.spinner.hide(this.spinnerName)
      }))
      .subscribe();
  }

}
