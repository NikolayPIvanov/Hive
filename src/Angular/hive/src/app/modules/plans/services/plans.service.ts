import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { CreatePlanCommand, InvestmentDto, InvestorsClient, PlansClient, UpdatePlanCommand } from 'src/app/clients/investing-client';

export class PaginationControl {
  pageSize: number = 20;
  pageNumber: number = 1;
  pageOptions: number[] = [5, 20, 100];
}

@Injectable({
  providedIn: 'root'
})
export class PlansService {
  private newInvestmentsSubject = new BehaviorSubject<InvestmentDto | undefined>(undefined);
  public newInvestments$ = this.newInvestmentsSubject.asObservable();
  public addNewInvestmentToPending(investment: InvestmentDto) {
    this.newInvestmentsSubject.next(investment);
  }

  private acceptedInvestmentSubject = new BehaviorSubject<InvestmentDto | undefined>(undefined);
  public acceptedInvestment$ = this.acceptedInvestmentSubject.asObservable();
  public onAcceptedInvestment(investment: InvestmentDto) {
    this.acceptedInvestmentSubject.next(investment);
  }

  constructor(private investingClient: PlansClient) { }

  getPlan(planId: number) {
    return this.investingClient.getPlan(planId);
  }

  getPlansAsVendor(pageNumber: number, pageSize: number,  searchKey: string | undefined) {
    return this.investingClient.getPlans(
      pageNumber,
      pageSize,
      searchKey, false)
  }

  getPlansAsInvestor(pageNumber: number, pageSize: number, searchKey: string | undefined) {
    return this.investingClient.getPlans(
      pageNumber,
      pageSize,
      searchKey,
      true)
  }

  createPlan(value: any) {
    const command = CreatePlanCommand.fromJS(value)
    return this.investingClient.createPlan(command)
  }

  updatePlan(id: number, value: any) {
    const command = UpdatePlanCommand.fromJS(value)
    return this.investingClient.updatePlan(id, command)
  }

  deletePlan(id: number) {
    return this.investingClient.deletePlan(id)
  }
}
