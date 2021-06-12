import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { tap } from 'rxjs/operators';
import { PlanDto } from 'src/app/clients/investing-client';
import { ProfileClient } from 'src/app/clients/profile-client';
import { PlansService } from '../../services/plans.service';
import { PlanEditComponent } from '../plan-edit/plan-edit.component';
import { PlanInspectComponent } from '../plan-inspect/plan-inspect.component';

export enum ChangeType {
  Delete = "delete",
  Update = "update"
}
export interface PlanListChangeEvent {
  type: ChangeType;
  item: PlanDto;
}

@Component({
  selector: 'app-plan-card',
  templateUrl: './plan-card.component.html',
  styleUrls: ['./plan-card.component.scss']
})
export class PlanCardComponent implements OnInit {
  @Input() plan!: PlanDto;
  @Output() change = new EventEmitter<PlanListChangeEvent>();

  constructor(
    private plansService: PlansService,
    private dialog: MatDialog,
    private profileClient: ProfileClient) { }

  ngOnInit(): void {
  }

  openInspectDialog(plan: PlanDto) {
    const dialogRef = this.dialog.open(PlanInspectComponent, { data: plan, width: '50%' });
    dialogRef.afterClosed().subscribe((result: PlanDto | undefined) => {
      if (result) {
        this.change.emit({ type: ChangeType.Update, item: result })
      }
    });
  }

  openEditDialog(plan: PlanDto) {
    const dialogRef = this.dialog.open(PlanEditComponent, { data: plan });
    dialogRef.afterClosed().subscribe((result: PlanDto | undefined) => {
      if (result) {
        this.change.emit({ type: ChangeType.Update, item: result })
      }
    });
  }

  delete(plan: PlanDto) {
    this.plansService.deletePlan(plan.id!)
      .pipe(tap({ next: () => this.change.emit({ type: ChangeType.Delete, item: plan })}))
      .subscribe();
  }

}
