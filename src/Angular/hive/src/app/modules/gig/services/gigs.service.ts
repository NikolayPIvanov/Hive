import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { GigDto, GigOverviewDto, GigsClient, PaginatedListOfGigOverviewDto, SellersClient } from 'src/app/clients/gigs-client';

@Injectable({
  providedIn: 'root'
})
export class GigsService {

  constructor(private gigsApiClient: GigsClient, private sellersApiClient: SellersClient) { }

  getGigDetailsById(id: number): Observable<GigDto> {
    return this.gigsApiClient.getGigById(id);
  }

  getSellerGigsOverview(pageIndex = 1, pageSize = 8): Observable<PaginatedListOfGigOverviewDto> {
    return this.sellersApiClient.getUserSellerId()
      .pipe(switchMap(id => this.sellersApiClient.getMyGigs(pageSize, pageIndex, null, id)));
  }
}
