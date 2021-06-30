import { Component, Input, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { Observable } from 'rxjs';
import { filter, map, tap } from 'rxjs/operators';
import { InvestmentDto, PaginatedListOfInvestmentDto, PlanDto, PlansClient } from 'src/app/clients/investing-client';
import { ProfileClient } from 'src/app/clients/profile-client';
import { PlansService } from '../../services/plans.service';

@Component({
  selector: 'app-plan-accepted-table',
  templateUrl: './plan-accepted-table.component.html',
  styleUrls: ['./plan-accepted-table.component.scss']
})
export class PlanAcceptedTableComponent implements OnInit {
  @Input() plan: PlanDto | undefined;

  displayedColumns: string[] = [
    'amount', 'roiPercentage', 'effectiveDate', 'expirationDate', 'investorId'];

  // Accepted
  public acceptedDataSource!: MatTableDataSource<InvestmentDto>;
  public accepted: InvestmentDto[] = []
  public acceptedInvestments$: Observable<PaginatedListOfInvestmentDto> | undefined;
  
  constructor(private planClient: PlansClient, private plansService: PlansService, private profileClient: ProfileClient) { }

  ngOnInit(): void {
    this.plansService.acceptedInvestment$
      .pipe(filter(x => !!x))
      .subscribe(x => {
        this.accepted.push(x!);
        this.acceptedDataSource.data = this.accepted;
      });
    
     // Accepted
     this.acceptedDataSource = new MatTableDataSource<InvestmentDto>([])
     this.acceptedInvestments$ = this.planClient.getPlanInvestments(
       this.plan!.id!, 10, 1, true)
       .pipe(
         tap({
           next: (data) => {
             if (data && data.items) {
               this.accepted = data.items;
               this.acceptedDataSource.data = data.items;
             }
           }
         }));
  }

  getInvestorProfile(investorUserId: string) {
    return this.profileClient.getProfileById(investorUserId)
      .pipe(map(p => `${p.givenName} ${p.surname}`))
  }

}
