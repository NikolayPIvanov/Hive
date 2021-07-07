import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LayoutModule } from '../layout/layout.module';
import { PlansRoutingModule } from './plans-routing.module';
import { PlansControlComponent } from './components/plans-control/plans-control.component';
import { PlansSearchFieldComponent } from './components/plans-search-field/plans-search-field.component';
import { PlanCreateComponent } from './components/plan-create/plan-create.component';
import { PlanEditComponent } from './components/plan-edit/plan-edit.component';
import { PlanCardComponent } from './components/plan-card/plan-card.component';
import { PlanInspectComponent } from './components/plan-inspect/plan-inspect.component';
import { MakeInvestmentComponent } from './components/make-investment/make-investment.component';
import { PlanAcceptedTableComponent } from './components/plan-accepted-table/plan-accepted-table.component';
import { PlanPendingTableComponent } from './components/plan-pending-table/plan-pending-table.component';

@NgModule({
  declarations: [
    PlansControlComponent,
    PlansSearchFieldComponent,
    PlanCreateComponent,
    PlanEditComponent,
    PlanCardComponent,
    PlanInspectComponent,
    MakeInvestmentComponent,
    PlanAcceptedTableComponent,
    PlanPendingTableComponent
  ],
  imports: [
    CommonModule,

    LayoutModule,

    PlansRoutingModule
  ],
  exports: [
    PlansSearchFieldComponent
  ]
})
export class PlansModule { }
