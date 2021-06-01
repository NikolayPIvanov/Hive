import { Component, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { Observable } from 'rxjs';
import { delay, map, mergeMap, mergeMapTo, switchMap } from 'rxjs/operators';
import { PackageDto } from 'src/app/clients/gigs-client';
import { OrdersClient, PlaceOrderCommand } from 'src/app/clients/ordering-client';
import { CheckoutService } from '../../services/checkout.service';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss']
})
export class CheckoutComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

}
