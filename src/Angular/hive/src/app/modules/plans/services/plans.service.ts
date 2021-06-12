import { Injectable } from '@angular/core';
import { CreatePlanCommand, InvestorsClient, PlansClient, UpdatePlanCommand } from 'src/app/clients/investing-client';

export class PaginationControl {
  pageSize: number = 20;
  pageNumber: number = 1;
  pageOptions: number[] = [5, 20, 100];
}

@Injectable({
  providedIn: 'root'
})
export class PlansService {
  private paginatorSettings = new PaginationControl();

  constructor(private investingClient: PlansClient) { }

  getPlan(planId: number) {
    return this.investingClient.getPlan(planId);
  }

  getPlansAsVendor(searchKey: string | undefined) {
    return this.investingClient.getPlans(
      this.paginatorSettings.pageNumber,
      this.paginatorSettings.pageSize,
      searchKey)
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
