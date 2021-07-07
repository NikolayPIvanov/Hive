import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { Observable } from 'rxjs';
import { delay, map, mergeMap, mergeMapTo, switchMap } from 'rxjs/operators';
import { GigDto, GigsClient, PackageDto } from 'src/app/clients/gigs-client';
import { OrdersClient, PlaceOrderCommand } from 'src/app/clients/ordering-client';
import { CheckoutService } from '../../services/checkout.service';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss']
})
export class CheckoutComponent implements OnInit {
  public gig$!: Observable<GigDto>;

  constructor(private activiatedRoute: ActivatedRoute, private gigsClient: GigsClient) { }

  ngOnInit(): void {
    const idOrNone = this.activiatedRoute.snapshot.paramMap.get('id');
    const gigIdOrNone = this.activiatedRoute.snapshot.paramMap.get('gigId');

    if (idOrNone == null || gigIdOrNone == null) {
      
    }

    const packageId = +idOrNone!;
    const gigId = +gigIdOrNone!;
    
    this.gig$ = this.gigsClient.getGigById(gigId)
      .pipe(map(gig => {
        gig.packages = gig.packages?.filter(p => p.id == packageId);
        return gig;
      }))
  }

}
