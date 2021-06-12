import { Component, Inject, OnInit } from '@angular/core';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { BehaviorSubject, Observable } from 'rxjs';
import { repeatWhen, tap } from 'rxjs/operators';
import { InvestmentDto, PaginatedListOfInvestmentDto, PlanDto, PlansClient, ProcessInvestmentCommand } from 'src/app/clients/investing-client';
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
  public pedning: InvestmentDto[] = []
  public accepted: InvestmentDto[] = []

  constructor(
    private dialog: MatDialog,
    private planClient: PlansClient,
    @Inject(MAT_DIALOG_DATA) public data: PlanDto) { }

  ngOnInit(): void {
    
    this.pendingInvestments$ = this.planClient.getPlanInvestments(
      this.data.id!, this.pageSize, this.pageNumber + 1, false)
      .pipe(
        repeatWhen(this.repeater.asObservable),
        tap({
          next: (data) => {
            if (data && data.items) {
              this.pedning = data.items;
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
            
            const index = this.pedning.indexOf(investment);
            this.pedning.splice(index, 1);
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

        if (this.pedning.length === 5) {
          this.pedning.splice(0, 1, data);
        }
        else {
          this.pedning.push(data);
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
