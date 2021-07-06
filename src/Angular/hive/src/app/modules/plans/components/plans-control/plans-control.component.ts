import { AfterViewInit, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { NgxSpinner, NgxSpinnerService } from 'ngx-spinner';
import { BehaviorSubject, config, Observable, Subject } from 'rxjs';
import { map, takeUntil, tap } from 'rxjs/operators';
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
export class PlansControlComponent implements OnInit, AfterViewInit, OnDestroy  {
  private unsubscribe = new Subject();
  private plansListSubject = new BehaviorSubject<PaginatedListOfPlanDto | undefined>(undefined);
  public plans$ = this.plansListSubject.asObservable();

  public pageSizeOptions = [10, 25, 50];
  public pageSize = 10;
  public pageIndex = 0;
  public length = 0;
  
  setPage(ev: PageEvent) {
    this.pageIndex = ev.pageIndex;
    this.pageSize = ev.pageSize;
    
    this.fetchPlans();
  }

  searchKey: string | undefined = undefined;

  displayedColumns: string[] = [
    'title', 'description', 'startDate', 'endDate', 'fundingNeeded',
     'isFunded', 'isPublic', 'actions'];
  
  isSeller = false;
  
  dataSource!: MatTableDataSource<PlanDto>;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private plansService: PlansService,
    private authService: AuthService,
    private spinner: NgxSpinnerService,
    private dialog: MatDialog) { }
  
  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  ngOnInit(): void {
    this.isSeller = this.authService.user?.profile.role.includes('Seller')!;
    this.dataSource = new MatTableDataSource<PlanDto>([])
    this.fetchPlans().subscribe();
  }

  onChange(event: PlanListChangeEvent) {
    const index = this.dataSource.data.findIndex(p => p.id == event.item.id!)
    let copy = this.dataSource.data;
    if (event.type == ChangeType.Update) {
      copy.splice(index, 1, event.item);
    } else if (event.type == ChangeType.Delete) {
      copy.splice(index, 1);
      this.length -= 1;
    }

    this.dataSource.data = copy;
  }

  onPlanSelected(plan: PlanDto | undefined) {
    if (plan) {
      this.dataSource.filter = plan?.title!.trim().toLowerCase()!;
      this.dataSource.data = [plan!];
    }
    else {
      this.fetchPlans().subscribe();
    }
  }

  openCreateDialog() {
    const dialogRef = this.dialog.open(PlanCreateComponent, { width: '50%' });
    dialogRef.afterClosed()
      .subscribe(result => {
        if (result) {
          let x = this.dataSource.data;
          x.push(result);
          this.dataSource.data = x;
        }
    });
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.searchKey = filterValue.trim().toLowerCase();
    this.dataSource.filter = filterValue.trim().toLowerCase();
    this.fetchPlans()
      .pipe(tap({
        next: () => {
        }
      }))
      .subscribe();
    
  }

  private fetchPlans() {
    this.spinner.show();

    let source: Observable<PaginatedListOfPlanDto>
    if (this.isSeller) {
      source = this.plansService.getPlansAsVendor(this.pageIndex + 1, this.pageSize, this.searchKey)
    }
    else {
      source = this.plansService.getPlansAsInvestor(this.pageIndex + 1, this.pageSize, this.searchKey)
    }
    return source
      .pipe(
        takeUntil(this.unsubscribe),
        map(list => {
          if (list) {
            this.dataSource.data = list.items || [];
          }
          this.plansListSubject.next(list);

          this.length = this.dataSource.data.length;
          return list;
        }),
        tap({ complete: () => this.spinner.show() })
      )
  }

}
