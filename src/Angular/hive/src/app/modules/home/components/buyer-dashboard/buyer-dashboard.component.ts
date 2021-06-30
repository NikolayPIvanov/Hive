import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { OrdersClient, PaginatedListOfOrderDto } from 'src/app/clients/ordering-client';

import { ChartDataSets, ChartOptions, ChartType } from 'chart.js';
import { Color } from 'ng2-charts';
import { SingleDataSet, Label, monkeyPatchChartJsLegend, monkeyPatchChartJsTooltip } from 'ng2-charts';
import { AccountHoldersClient, TransactionDto, TransactionType, WalletDto } from 'src/app/clients/billing-client';
import { MatTableDataSource } from '@angular/material/table';
import { tap } from 'rxjs/operators';

@Component({
  selector: 'app-buyer-dashboard',
  templateUrl: './buyer-dashboard.component.html',
  styleUrls: ['./buyer-dashboard.component.scss']
})
export class BuyerDashboardComponent implements OnInit {
  orders$: Observable<PaginatedListOfOrderDto> | undefined;
  wallet$: Observable<WalletDto> | undefined;

  constructor(private ordersClient: OrdersClient,
    private billingClient: AccountHoldersClient) {
    monkeyPatchChartJsTooltip();
    monkeyPatchChartJsLegend();
  }

  getTransactionType(index: TransactionType) {
    return TransactionType[index];
  }

  columnsToDisplay = ['transactionNumber', 'amount', 'transactionType'];
  dataSource = new MatTableDataSource<TransactionDto>([]);

  ngOnInit(): void {
    this.orders$ = this.ordersClient.getMyOrders(1, 3, false)
    this.wallet$ =
      this.billingClient.getWallet(undefined)
      .pipe(
        tap({
          next: (wallet) => {
            this.billingClient.getWalletTransactions(wallet.id!, 1, 6, wallet.accountHolderId!.toString())
              .subscribe(transactions => {
                this.dataSource = new MatTableDataSource<TransactionDto>(transactions.items);
              })
          }
        })
      )
  }

  public lineChartData: ChartDataSets[] = [
    {
      data:
        [0, 3, 5, 1, 0, 7, 3, 9, 0, 4, 7, 8],
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
      backgroundColor: 'rgba(235,0,200,0.3)',
    },
  ];
  public lineChartLegend = true;
  public lineChartType: ChartType = 'line';
  public lineChartPlugins = [];

  public pieChartOptions: ChartOptions = {
    responsive: true,
  };
  public pieChartLabels: Label[] = [['Marketing'], ['Social'], 'Web Design'];
  public pieChartData: SingleDataSet = [30, 50, 20];
  public pieChartType: ChartType = 'pie';
  public pieChartLegend = true;
  public pieChartPlugins = [];

}
