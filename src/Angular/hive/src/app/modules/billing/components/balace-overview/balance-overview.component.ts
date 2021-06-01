import { Component, Input, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { AccountHoldersClient, TransactionDto, WalletDto } from 'src/app/clients/billing-client';

@Component({
  selector: 'app-balance-overview',
  templateUrl: './balance-overview.component.html',
  styleUrls: ['./balance-overview.component.scss']
})
export class BalanceOverviewComponent implements OnInit {
  @Input() wallet!: WalletDto;

  constructor() { }

  ngOnInit(): void {
  }

}
