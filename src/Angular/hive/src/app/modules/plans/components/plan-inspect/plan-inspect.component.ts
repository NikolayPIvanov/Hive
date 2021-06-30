import { Component, Inject, OnInit } from '@angular/core';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { BehaviorSubject, Observable } from 'rxjs';
import { repeatWhen, tap } from 'rxjs/operators';
import { InvestmentDto, PaginatedListOfInvestmentDto, PlanDto, PlansClient, ProcessInvestmentCommand } from 'src/app/clients/investing-client';
import { AuthService } from 'src/app/modules/layout/services/auth.service';
import { MakeInvestmentComponent } from '../make-investment/make-investment.component';

@Component({
  selector: 'app-plan-inspect',
  templateUrl: './plan-inspect.component.html',
  styleUrls: ['./plan-inspect.component.scss']
})
export class PlanInspectComponent implements OnInit {
  pageSize = 5
  pageSizeOptions = [5, 10, 25]
  pageNumber = 0;
  length = 0;

  private repeater = new BehaviorSubject<any>(null);

  public pendingInvestments$: Observable<PaginatedListOfInvestmentDto> | undefined;
  public acceptedInvestments$: Observable<PaginatedListOfInvestmentDto> | undefined;
  public pending: InvestmentDto[] = []
  public accepted: InvestmentDto[] = []
  
  public isInvestor = false;
  public isOwner = false;

  constructor(
    private dialog: MatDialog,
    private planClient: PlansClient,
    private authService: AuthService,
    @Inject(MAT_DIALOG_DATA) public data: PlanDto) { }

  ngOnInit(): void {
    this.isInvestor = this.authService.user?.profile.role?.includes('Investor')!
    this.isOwner = this.authService.user?.profile.sub! == this.data.vendorUserId!;
    this.pendingInvestments$ = this.planClient.getPlanInvestments(
      this.data.id!, this.pageSize, this.pageNumber + 1, false)
      .pipe(
        repeatWhen(this.repeater.asObservable),
        tap({
          next: (data) => {
            if (data && data.items) {
              this.pending = data.items;
              this.length = data.totalCount!;
            }
          }
        }));
    
      this.acceptedInvestments$ = this.planClient.getPlanInvestments(
        this.data.id!, this.pageSize, this.pageNumber + 1, true)
        .pipe(
          tap({
            next: (data) => {
              if (data && data.items) {
                this.accepted = data.items;
            }
        }}))
  }

  processInvestment(investment: InvestmentDto, accept = true) {
    const command = ProcessInvestmentCommand.fromJS(
      { planId: investment.planId!, investmentId: investment.id!, accept: accept })
    this.planClient.processInvestment(investment.planId!, investment.id!, command)
      .pipe(tap({
        next: (result) => {
          if (accept) {
            
            const index = this.pending.indexOf(investment);
            this.pending.splice(index, 1);
            this.length -= 1;

            this.accepted.push(investment);
          }
        }
      }))
      .subscribe();
  }

  openInvestmentDialog() {
    const invest = this.dialog.open(MakeInvestmentComponent, {
      data: this.data
    });

    invest.afterClosed()
      .subscribe(data => {
        if (!data) {
          return;
        }

        if (this.pending.length === 5) {
          this.pending.splice(0, 1, data);
        }
        else {
          this.pending.push(data);
        }

        this.length += 1;
        
      })
  }

  handlePageEvent(event: PageEvent) {
    this.pageSize = event.pageSize;
    this.pageNumber = event.pageIndex;

    this.repeater.next(null)
  }
  

}
