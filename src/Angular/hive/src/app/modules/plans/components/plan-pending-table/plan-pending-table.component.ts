import { ChangeDetectorRef, Component, Input, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { Observable } from 'rxjs';
import { filter, map, tap } from 'rxjs/operators';
import { PlanDto, PaginatedListOfInvestmentDto, InvestmentDto, PlansClient, ProcessInvestmentCommand } from 'src/app/clients/investing-client';
import { ProfileClient } from 'src/app/clients/profile-client';
import { PlansService } from '../../services/plans.service';

@Component({
  selector: 'app-plan-pending-table',
  templateUrl: './plan-pending-table.component.html',
  styleUrls: ['./plan-pending-table.component.scss']
})
export class PlanPendingTableComponent implements OnInit {
  @Input() plan!: PlanDto;
  @Input() isOwner!: boolean;

  displayedColumns: string[] = ['amount',
    'roiPercentage', 'effectiveDate', 'expirationDate', 'investorUserId'];
  
  public investments$!: Observable<PaginatedListOfInvestmentDto>;
  public dataSource!: MatTableDataSource<InvestmentDto>;

  constructor(
    private cdr: ChangeDetectorRef,
    private profileClient: ProfileClient,
    private plansService: PlansService,
    private planClient: PlansClient) { }

  ngOnInit(): void {
    if (this.isOwner) {
      this.displayedColumns.push('actions')
    }
    this.dataSource = new MatTableDataSource<InvestmentDto>([])

    this.plansService.newInvestments$
      .pipe(filter(x => x != undefined))
      .subscribe(x => {
        const data = this.dataSource.data;
        data.push(x!);
        this.dataSource.data = data;
      })

    const planId = this.plan.id!;
    this.investments$ = this.planClient.getPlanInvestments(planId, 10, 1, false)
      .pipe(tap({
        next: (list) => {
          if (list && list.items) {
            this.dataSource.data = list.items;
          }
      }}))
  }

  getInvestorProfile(investorUserId: string) {
    return this.profileClient.getProfileById(investorUserId)
      .pipe(map(p => {
        this.cdr.detectChanges();
        return `${p.givenName} ${p.surname}`;
      }))
  }

  processInvestment(investment: InvestmentDto, accept = true) {
    const command = ProcessInvestmentCommand.fromJS(
      { planId: investment.planId!, investmentId: investment.id!, accept: accept })
    
    this.planClient.processInvestment(investment.planId!, investment.id!, command)
      .pipe(tap({
        next: (result) => {
          if (accept) {
            let copyOfData = this.dataSource.data
            const index = copyOfData.indexOf(investment);
            copyOfData.splice(index, 1);
            this.dataSource.data = copyOfData;

            this.plansService.onAcceptedInvestment(investment);
          }
        }
      }))
      .subscribe();
  }

}
