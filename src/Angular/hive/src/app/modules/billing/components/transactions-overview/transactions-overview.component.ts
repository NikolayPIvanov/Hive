import { trigger, state, style, transition, animate } from '@angular/animations';
import { AfterViewInit, Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { TransactionDto, TransactionType } from 'src/app/clients/billing-client';

@Component({
  selector: 'app-transactions-overview',
  templateUrl: './transactions-overview.component.html',
  styleUrls: ['./transactions-overview.component.scss'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({height: '0px', minHeight: '0'})),
      state('expanded', style({height: '*'})),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ],
})
export class TransactionsOverviewComponent implements OnInit, AfterViewInit {
  @Input() transactions!: TransactionDto[];
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  
  columnsToDisplay = ['transactionNumber', 'amount', 'transactionType', 'orderNumber'];
  dataSource = new MatTableDataSource<TransactionDto>(this.transactions);

  constructor() { }

  ngOnInit(): void {
    this.dataSource = new MatTableDataSource<TransactionDto>(this.transactions);
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  getTransactionType(index: TransactionType) {
    return TransactionType[index];
  }

}
