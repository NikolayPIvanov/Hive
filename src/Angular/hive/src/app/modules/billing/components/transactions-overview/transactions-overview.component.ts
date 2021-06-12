import { trigger, state, style, transition, animate } from '@angular/animations';
import { AfterViewInit, Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { NgxSpinnerService } from 'ngx-spinner';
import { BehaviorSubject, Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { AccountHoldersClient, PaginatedListOfTransactionDto, TransactionDto, TransactionType, WalletDto } from 'src/app/clients/billing-client';
import { BillingService } from '../../services/billing.service';

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
  @Input() wallet!: WalletDto;
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  itemsCount: number = 0;
  pageSize = 5;
  pageIndex = 0;
  
  onChange(pageEvent: PageEvent) {
    this.pageSize = pageEvent.pageSize;
    this.pageIndex = pageEvent.pageIndex
    this.getList();
  }

  private transactionsSubject = new BehaviorSubject<PaginatedListOfTransactionDto | undefined>(undefined)
  public transactions$ = this.transactionsSubject.asObservable();
  
  columnsToDisplay = ['transactionNumber', 'amount', 'transactionType', 'orderNumber'];
  dataSource = new MatTableDataSource<TransactionDto>([]);

  constructor(
    private billingService: BillingService,
    private billingClient: AccountHoldersClient, private spinner: NgxSpinnerService) { }

  ngOnInit(): void {
    this.getList().subscribe()

    this.billingService.newTransaction$
      .subscribe(transaction => {
        if (transaction) {
          const copy = this.dataSource.data;
          copy.shift()
          copy.unshift(transaction);
          this.dataSource.data = copy;
          this.itemsCount += 1;
        }
      })
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  getTransactionType(index: TransactionType) {
    return TransactionType[index];
  }

  private getList() {
    this.spinner.show('inner')
    
    return this.billingClient.getWalletTransactions(
      this.wallet.id!,
      this.pageIndex + 1,
      this.pageSize,
      this.wallet.accountHolderId?.toString()!)
      .pipe(tap({
        next: (paginatedList) => {
          this.transactionsSubject.next(paginatedList);
          this.dataSource = new MatTableDataSource<TransactionDto>(paginatedList.items);
          this.itemsCount = paginatedList.totalCount!;
        },
        complete: () => this.spinner.hide('inner')
      }))
  }

}
