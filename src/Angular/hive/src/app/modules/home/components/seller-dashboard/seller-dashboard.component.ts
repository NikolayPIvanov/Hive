import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { GigsClient, PaginatedListOfGigOverviewDto, SellersClient } from 'src/app/clients/gigs-client';

@Component({
  selector: 'app-seller-dashboard',
  templateUrl: './seller-dashboard.component.html',
  styleUrls: ['./seller-dashboard.component.scss']
})
export class SellerDashboardComponent implements OnInit {
  gigs$: Observable<PaginatedListOfGigOverviewDto> | undefined

  constructor(private sellersClient: SellersClient) { }

  ngOnInit(): void {
    this.gigs$ = this.sellersClient.getUserSellerId()
      .pipe(switchMap(id => this.sellersClient.getMyGigs(3, 1, id)));
    
  }

}
