import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { PlanDto } from 'src/app/clients/investing-client';
import { ProfileClient, UserProfileDto } from 'src/app/clients/profile-client';
import { AuthService } from 'src/app/modules/layout/services/auth.service';
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

  public profile$: Observable<UserProfileDto> | undefined;
  public isOwner!: boolean;

  constructor(
    private plansService: PlansService,
    private dialog: MatDialog,
    private authService: AuthService) { }

  ngOnInit(): void {
    this.isOwner = this.plan.vendorUserId == this.authService.user?.profile.sub!
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
        debugger;
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
