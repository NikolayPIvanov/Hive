import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { PaginatedListOfTransactionDto, TransactionDto } from 'src/app/clients/billing-client';

@Injectable({
  providedIn: 'root'
})
export class BillingService {
  private transactionSubject =
    new BehaviorSubject<TransactionDto | undefined>(undefined);
  public newTransaction$ = this.transactionSubject.asObservable();

  public onNewTransaction(transaction: TransactionDto) {
    this.transactionSubject.next(transaction);
  }

  constructor() { }
}
