import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { map, repeatWhen, shareReplay, tap } from 'rxjs/operators';
import { PaginatedListOfPlanDto, PlanDto } from 'src/app/clients/investing-client';
import { PlansService } from '../../services/plans.service';
import { ChangeType, PlanListChangeEvent } from '../plan-card/plan-card.component';
import { PlanCreateComponent } from '../plan-create/plan-create.component';

@Component({
  selector: 'app-plans-control',
  templateUrl: './plans-control.component.html',
  styleUrls: ['./plans-control.component.scss']
})
export class PlansControlComponent implements OnInit, AfterViewInit  {
  private plansSubject = new BehaviorSubject<PaginatedListOfPlanDto | undefined>(undefined);
  plans$ = this.plansSubject.asObservable();
  searchKey: string | undefined = undefined;

  displayedColumns: string[] = [
    'title', 'description', 'startDate', 'endDate', 'fundingNeeded', 'isFunded', 'isPublic', 'actions'];
  dataSource!: MatTableDataSource<PlanDto>;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private plansService: PlansService,
    private dialog: MatDialog) { }
  
  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  ngOnInit(): void {
    this.dataSource = new MatTableDataSource<PlanDto>([])
    this.fetchPlans();
  }

  onChange(event: PlanListChangeEvent) {
    debugger;
    const index = this.dataSource.data.findIndex(p => p.id == event.item.id!)
    let copy = this.dataSource.data;
    if (event.type == ChangeType.Update) {
      copy.splice(index, 1, event.item);
    } else if (event.type == ChangeType.Delete) {
      copy.splice(index, 1);
    }

    this.dataSource.data = copy;
  }

  onPlanSelected(plan: PlanDto | undefined) {
    if (plan) {
      this.dataSource.filter = plan?.title!.trim().toLowerCase()!;
      this.dataSource.data = [plan!];
    }
    else {
      this.fetchPlans();
    }
  }

  openCreateDialog() {
    const dialogRef = this.dialog.open(PlanCreateComponent);
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.dataSource.data.push(result);
      }
    });
  }

  private fetchPlans() {
    this.plansService.getPlansAsVendor(this.searchKey)
      .pipe(
        map(list => {
          if (list) {
            this.dataSource.data = list.items || [];
          }
          this.plansSubject.next(list);
          return list;
        })
      )
      .subscribe();
  }

}
