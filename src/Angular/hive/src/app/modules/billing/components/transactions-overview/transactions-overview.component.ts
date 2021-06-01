import { trigger, state, style, transition, animate } from '@angular/animations';
import { Component, Input, OnInit } from '@angular/core';
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
export class TransactionsOverviewComponent implements OnInit {
  @Input() transactions!: TransactionDto[];
  columnsToDisplay = ['transactionNumber', 'amount', 'transactionType', 'orderNumber'];
  expandedElement: TransactionDto | null = null;

  constructor() { }

  ngOnInit(): void {
    this.transactions = this.transactions.splice(0,5)
  }

  getTransactionType(index: TransactionType) {
    return TransactionType[index];
  }

}
