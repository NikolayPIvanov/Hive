import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { PackageDto } from 'src/app/clients/gigs-client';

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {
  private checkoutPackage = new BehaviorSubject<PackageDto>(PackageDto.fromJS({price: 0.0}));
  public checkoutPackage$ = this.checkoutPackage.asObservable();

  private orderQuantity = new BehaviorSubject<number>(1);
  public orderQuantity$ = this.orderQuantity.asObservable();

  constructor() { }

  public selectPackage(current: PackageDto) {
    this.checkoutPackage.next(current);
  }

  public selectQuantity(current: number) {
    this.orderQuantity.next(current);
  }
}
