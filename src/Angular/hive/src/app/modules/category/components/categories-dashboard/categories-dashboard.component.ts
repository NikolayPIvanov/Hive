import { DataSource } from '@angular/cdk/collections';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { NgxSpinnerService } from 'ngx-spinner';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { tap } from 'rxjs/operators';
import { CategoriesClient, CategoriesType, CategoryDto, PaginatedListOfCategoryDto } from 'src/app/clients/gigs-client';
import { CategoryDetailsComponent } from '../category-details/category-details.component';

@Component({
  selector: 'app-categories-dashboard',
  templateUrl: './categories-dashboard.component.html',
  styleUrls: ['./categories-dashboard.component.scss']
})
export class CategoriesDashboardComponent implements OnInit, OnDestroy {
  private unsubscribe = new Subject();

  pageNumber = 0;
  pageSize = 10;
  pageOptions = [10, 25, 50];
  length = 0;
  searchKey: string | null = null;

  private categoriesSubject = new BehaviorSubject<PaginatedListOfCategoryDto | undefined>(undefined);
  public categories$ = this.categoriesSubject.asObservable();
  public categories: CategoryDto[] = [];
  public dataSource!: MatTableDataSource<CategoryDto>;
  displayedColumns: string[] = ['title', 'description', 'parent', 'children', 'actions'];

  constructor(
    public dialog: MatDialog,
    private spinner: NgxSpinnerService,
    private categoriesApiClient: CategoriesClient,) { }

  ngOnInit(): void {
    this.dataSource = new MatTableDataSource<CategoryDto>([])
    this.fetchCategories()
  }

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  setPage(e: PageEvent) {
    this.pageNumber = e.pageIndex;
    this.pageSize = e.pageSize;
    this.fetchCategories();
  }

  selected(category: CategoryDto) {
    if (category) {
      this.categories = [category]
      this.dataSource.data = [category];
    }
    else {
      this.fetchCategories();
    }
  }
  delete(category: CategoryDto) {
    this.categoriesApiClient.deleteCategory(category.id!)
      .pipe(tap({
        next: () => {
          const copy = this.dataSource.data
          const index = copy.indexOf(category);
          if (index > -1) {
            copy.splice(index, 1);
          }
          this.dataSource.data = copy;
      }}))
      .subscribe()
  }

  openDialog(category: CategoryDto) {
    const dialogRef = this.dialog.open(CategoryDetailsComponent,
    {
      width: "30%",
      data: category
    });

    dialogRef.afterClosed()
      .pipe(tap({
        next: (updatedCategory) => {
          const copy = this.dataSource.data;
          const index = copy.indexOf(category);
          if (updatedCategory && index > -1) {
            copy.splice(index, 1, updatedCategory);
            this.dataSource.data = copy;
          }
        }
      }))
      .subscribe();
  }

  onClosedDialog(category: CategoryDto) {
    this.categories.push(category)

    const copy = this.dataSource.data
    copy.push(category);
    this.dataSource.data = copy;

    this.length += 1;
  }


  private fetchCategories() {
    this.spinner.show();

    this.categoriesApiClient
      .getCategories(this.pageNumber + 1, this.pageSize, CategoriesType.All, this.searchKey)
      .pipe(tap(
        {
          next: (list) => {
            this.categoriesSubject.next(list);
            this.categories = list.items!
            this.length = list.totalCount!;
            this.dataSource.data = list.items!;
          },
          complete: () => { this.spinner.hide() }
        }
      ))
    .subscribe()
  }

}
