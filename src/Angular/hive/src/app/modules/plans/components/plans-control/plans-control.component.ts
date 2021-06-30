import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { BehaviorSubject, config, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { PaginatedListOfPlanDto, PlanDto } from 'src/app/clients/investing-client';
import { AuthService } from 'src/app/modules/layout/services/auth.service';
import { PlansService } from '../../services/plans.service';
import { ChangeType, PlanListChangeEvent } from '../plan-card/plan-card.component';
import { PlanCreateComponent } from '../plan-create/plan-create.component';

@Component({
  selector: 'app-plans-control',
  templateUrl: './plans-control.component.html',
  styleUrls: ['./plans-control.component.scss']
})
export class PlansControlComponent implements OnInit, AfterViewInit  {
  private plansListSubject = new BehaviorSubject<PaginatedListOfPlanDto | undefined>(undefined);
  public plans$ = this.plansListSubject.asObservable();

  searchKey: string | undefined = undefined;

  displayedColumns: string[] = [
    'title', 'description', 'startDate', 'endDate', 'fundingNeeded', 'isFunded', 'isPublic', 'actions'];
  
  private isSeller = false;
  
  dataSource!: MatTableDataSource<PlanDto>;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private plansService: PlansService,
    private authService: AuthService,
    private dialog: MatDialog) { }
  
  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  ngOnInit(): void {
    this.isSeller = this.authService.user?.profile.role.includes('Seller')!;
    this.dataSource = new MatTableDataSource<PlanDto>([])
    this.fetchPlans();
  }

  onChange(event: PlanListChangeEvent) {
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
    const dialogRef = this.dialog.open(PlanCreateComponent, { width: '50%' });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        let x = this.dataSource.data;
        x.push(result);
        this.dataSource.data = x;
      }
    });
  }

  private fetchPlans() {
    let source: Observable<PaginatedListOfPlanDto>
    if (this.isSeller) {
      source = this.plansService.getPlansAsVendor(this.searchKey)
    }
    else {
      source = this.plansService.getPlansAsInvestor(this.searchKey)
    }
    source
      .pipe(
        map(list => {
          if (list) {
            this.dataSource.data = list.items || [];
          }
          this.plansListSubject.next(list);
          return list;
        })
      )
      .subscribe();
  }

}
