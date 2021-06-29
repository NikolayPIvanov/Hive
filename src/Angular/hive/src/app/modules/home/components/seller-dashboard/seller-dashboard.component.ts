import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { GigsClient, PaginatedListOfGigOverviewDto, SellersClient } from 'src/app/clients/gigs-client';

import { ChartDataSets, ChartOptions, ChartType } from 'chart.js';
import { Color, Label } from 'ng2-charts';
import { OrdersClient, PaginatedListOfOrderDto } from 'src/app/clients/ordering-client';
import { PaginatedListOfPlanDto, PlansClient } from 'src/app/clients/investing-client';

@Component({
  selector: 'app-seller-dashboard',
  templateUrl: './seller-dashboard.component.html',
  styleUrls: ['./seller-dashboard.component.scss']
})
export class SellerDashboardComponent implements OnInit {
  gigs$: Observable<PaginatedListOfGigOverviewDto> | undefined;
  orders$: Observable<PaginatedListOfOrderDto> | undefined;
  plans$: Observable<PaginatedListOfPlanDto> | undefined;

  constructor(
    private sellersClient: SellersClient,
    private gigsClient: GigsClient,
    private plansClient: PlansClient,
    private ordersClient: OrdersClient) { }

  ngOnInit(): void {
    this.gigs$ = this.sellersClient.getUserSellerId()
      .pipe(switchMap(id => this.sellersClient.getMyGigs(3, 1, id)));
    
    this.orders$ = this.ordersClient.getMyOrders(1, 3, true)
    this.plans$ = this.plansClient.getPlans(1, 3, undefined);
  }

  constructDownload(gigId: number) {
    return this.gigsClient.getAvatar(gigId);
  }

  public lineChartData: ChartDataSets[] = [
    {
      data:
        [0, 200, 350, 450, 467, 980, 1200, 2300, 2800, 2942, 3200, 3600],
      label: 'Revenue'
    },
  ];
  public lineChartLabels: Label[] = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
  public lineChartOptions: (ChartOptions) = {
    responsive: true,
  };
  public lineChartColors: Color[] = [
    {
      borderColor: 'black',
      backgroundColor: 'rgba(255,0,0,0.3)',
    },
  ];
  public lineChartLegend = true;
  public lineChartType: ChartType = 'line';
  public lineChartPlugins = [];

}
