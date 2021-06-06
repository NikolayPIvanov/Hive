import { Component, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { Observable, of } from 'rxjs';
import { mergeAll } from 'rxjs/operators';
import { AccountHoldersClient, TransactionDto, TransactionType, WalletDto } from 'src/app/clients/billing-client';
import { ProfileClient, UserProfileDto } from 'src/app/clients/profile-client';

@Component({
  selector: 'app-billing-overview',
  templateUrl: './billing-overview.component.html',
  styleUrls: ['./billing-overview.component.scss']
})
export class BillingOverviewComponent implements OnInit {
  wallet$!: Observable<WalletDto>;
  owner$!: Observable<UserProfileDto>;

  constructor(
    private billingApiClient: AccountHoldersClient,
    private profileClient: ProfileClient) { }
  
  ngOnInit(): void {
    this.wallet$ = this.billingApiClient.getWallet(undefined);
    this.owner$ = this.profileClient.getProfile();
  }

  onChange(data: any) {
    this.wallet$ = this.billingApiClient.getWallet(undefined);
  }

}
