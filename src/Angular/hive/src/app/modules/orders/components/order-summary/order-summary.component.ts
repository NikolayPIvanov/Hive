import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { Observable, throwError } from 'rxjs';
import { delay, map, switchMap, tap } from 'rxjs/operators';
import { GigsClient, PackageDto } from 'src/app/clients/gigs-client';
import { CheckoutService } from '../../services/checkout.service';

@Component({
  selector: 'app-order-summary',
  templateUrl: './order-summary.component.html',
  styleUrls: ['./order-summary.component.scss']
})
export class OrderSummaryComponent implements OnInit {
  selectedQuantity: number = 1;
  initialPrice = 45.0;

  quantity!: number[];
  package$!: Observable<PackageDto>

  private currentPackage!: PackageDto;

  constructor(
    private activatedRoute: ActivatedRoute,
    private gigsApiClient: GigsClient,
    private checkoutService: CheckoutService,
    private spinner: NgxSpinnerService
  ) {
    this.quantity = Array.from({ length: 10 }, (_, i) => i + 1).filter(value => !!value);
   }

  ngOnInit(): void {

    const idOrNone = this.activatedRoute.snapshot.paramMap.get('id');
    if (idOrNone === null) {
      throwError('Id missing from route');
    }
    const id = +idOrNone!;

    this.currentPackage = new PackageDto({ price: this.initialPrice });
    this.checkoutService.selectPackage(this.currentPackage);
    
    this.package$ =
      this.gigsApiClient.getPackageById(-1, id)
        .pipe(map(p => {
          this.currentPackage = (p || new PackageDto({ price: this.initialPrice }));
          this.checkoutService.selectPackage(this.currentPackage);
          return p;
        }));
    
    this.checkoutService.selectQuantity(this.selectedQuantity);
  }

  onSelected($event: any) {
    this.checkoutService.selectQuantity($event.value)
  }

  public get totalPrice(): number {
    return this.initialPrice * this.selectedQuantity;
  }

}
