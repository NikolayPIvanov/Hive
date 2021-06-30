import { Component, Inject, OnInit } from '@angular/core';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { BehaviorSubject, Observable } from 'rxjs';
import { filter, map, repeatWhen, tap } from 'rxjs/operators';
import { InvestmentDto, PaginatedListOfInvestmentDto, PlanDto, PlansClient, ProcessInvestmentCommand } from 'src/app/clients/investing-client';
import { ProfileClient } from 'src/app/clients/profile-client';
import { ProfileService } from 'src/app/modules/account/services/profile.service';
import { AuthService } from 'src/app/modules/layout/services/auth.service';
import { PlansService } from '../../services/plans.service';
import { MakeInvestmentComponent } from '../make-investment/make-investment.component';

@Component({
  selector: 'app-plan-inspect',
  templateUrl: './plan-inspect.component.html',
  styleUrls: ['./plan-inspect.component.scss']
})
export class PlanInspectComponent implements OnInit {
  public isInvestor = false;
  public isOwner = false;

  constructor(
    private dialog: MatDialog,
    private authService: AuthService,
    private planService: PlansService,
    @Inject(MAT_DIALOG_DATA) public data: PlanDto) { }
  
  ngOnInit(): void {
    this.isInvestor = this.authService.user?.profile.role?.includes('Investor')!
    this.isOwner = this.authService.user?.profile.sub! == this.data.vendorUserId!;
  }

  openInvestmentDialog() {
    const invest = this.dialog.open(MakeInvestmentComponent, { data: this.data });

    invest.afterClosed()
      .pipe(filter(data => data))
      .subscribe(data => {
        this.planService.addNewInvestmentToPending(data);
      })
  }

}
