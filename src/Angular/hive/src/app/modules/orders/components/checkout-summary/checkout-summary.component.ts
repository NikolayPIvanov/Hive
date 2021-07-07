import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { Observable, Subject } from 'rxjs';
import { map, takeUntil, tap } from 'rxjs/operators';
import { DeliveryFrequency, GigDto, PackageDto } from 'src/app/clients/gigs-client';
import { OrdersClient, PlaceOrderCommand } from 'src/app/clients/ordering-client';
import { NotificationService } from 'src/app/modules/core/services/notification.service';
import { CheckoutService } from '../../services/checkout.service';

@Component({
  selector: 'app-checkout-summary',
  templateUrl: './checkout-summary.component.html',
  styleUrls: ['./checkout-summary.component.scss']
})
export class CheckoutSummaryComponent implements OnInit, OnDestroy {
  @Input() gig!: GigDto;
  spinnerName = 'checkoutorder';

  totalPrice = 0.0;
  package$!: Observable<PackageDto | undefined>;
  quantity$!: Observable<number>;

  requirements = new FormControl('');
  
  constructor(private checkoutService: CheckoutService,
    private ordersApiClient: OrdersClient,
    private spinner: NgxSpinnerService,
    private notificationService: NotificationService,
    private router: Router) { }

  ngOnInit(): void {
    this.package$ = this.checkoutService.checkoutPackage$;
    this.quantity$ = this.checkoutService.orderQuantity$;
  }

  ngOnDestroy(): void {
    //Called once, before the instance is destroyed.
    //Add 'implements OnDestroy' to the class.
    this.subject.next();
    this.subject.complete();
  }

  private subject = new Subject();
  
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
      .pipe(
        takeUntil(this.subject),
        tap({
          next: (orderNumber) => {
            this.router.navigate(['/orders', orderNumber, 'placed'])
            this.notificationService.openSnackBar('Order Placed')
          },
        error: () => this.notificationService.openSnackBar('Order creation failed'),
        complete: () => this.spinner.hide(this.spinnerName)
      }))
      .subscribe();
  }

  displayDeliveryType(type: DeliveryFrequency) {
    return DeliveryFrequency[type];
  }

}
