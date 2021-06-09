import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { Observable, throwError } from 'rxjs';
import { delay, map, switchMap, tap } from 'rxjs/operators';
import { FileResponse, GigDto, GigsClient, PackageDto } from 'src/app/clients/gigs-client';
import { CheckoutService } from '../../services/checkout.service';

@Component({
  selector: 'app-order-summary',
  templateUrl: './order-summary.component.html',
  styleUrls: ['./order-summary.component.scss']
})
export class OrderSummaryComponent implements OnInit {
  @Input() gig!: GigDto;
  currentPackage!: PackageDto;

  public download!: Observable<FileResponse>;
  public default = '/assets/no_image.png'


  selectedQuantity: number = 1;
  quantity!: number[];

  constructor(
    private checkoutService: CheckoutService,
    private gigsClient: GigsClient
  ) {
    this.quantity = Array.from({ length: 10 }, (_, i) => i + 1).filter(value => !!value);
   }

  ngOnInit(): void {
    this.currentPackage = this.gig.packages![0];
    this.checkoutService.selectPackage(this.currentPackage);
    this.checkoutService.selectQuantity(this.selectedQuantity);
    this.download = this.gigsClient.getAvatar(this.gig.id!);
  }

  onSelected($event: any) {
    this.checkoutService.selectQuantity($event.value)
  }

  public get totalPrice(): number {
    return this.currentPackage.price! * this.selectedQuantity;
  }

}
