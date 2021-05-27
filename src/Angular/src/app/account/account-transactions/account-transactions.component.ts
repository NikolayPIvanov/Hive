import { Component, Input, OnInit } from '@angular/core';
import { TransactionDto } from 'src/app/clients/billing-client';

export enum FundType {
  Fund,
  Withdraw
}

@Component({
  selector: 'app-account-transactions',
  templateUrl: './account-transactions.component.html',
  styleUrls: ['./account-transactions.component.scss']
})
export class AccountTransactionsComponent implements OnInit {
  @Input('balance') balance!: number;
  @Input('transactions') transactions!: TransactionDto[];
  
  typesOfShoes: string[] = ['Boots', 'Clogs', 'Loafers', 'Moccasins', 'Sneakers'];

  constructor() { }

  ngOnInit(): void {
  }

  convertTransactionType(index: number): string {
    return FundType[index];
  }

}
