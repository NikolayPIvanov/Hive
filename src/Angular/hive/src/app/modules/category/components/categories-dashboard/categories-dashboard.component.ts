import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { NgxSpinnerService } from 'ngx-spinner';
import { BehaviorSubject, Observable } from 'rxjs';
import { map, switchMap, takeUntil, tap } from 'rxjs/operators';
import { CategoriesClient, CategoriesType, CategoryDto, PaginatedListOfCategoryDto } from 'src/app/clients/gigs-client';
import { CategoryDetailsComponent } from '../category-details/category-details.component';

@Component({
  selector: 'app-categories-dashboard',
  templateUrl: './categories-dashboard.component.html',
  styleUrls: ['./categories-dashboard.component.scss']
})
export class CategoriesDashboardComponent implements OnInit {
  pageNumber = 0;
  pageSize = 10;
  pageOptions = [5, 10, 25];
  length = 0;
  searchKey: string | null = null;

  private categoriesSubject = new BehaviorSubject<PaginatedListOfCategoryDto | undefined>(undefined);
  public categories$ = this.categoriesSubject.asObservable();
  public categories: CategoryDto[] = [];

  constructor(
    public dialog: MatDialog,
    private spinner: NgxSpinnerService,
    private categoriesApiClient: CategoriesClient) { }

  ngOnInit(): void {
    this.fetchCategories()
  }

  selected(category: CategoryDto) {
    if (category) {
      this.categories = [category]
    }
    else {
      this.fetchCategories();
    }
  }
  delete(category: CategoryDto) {
    this.categoriesApiClient.deleteCategory(category.id!)
      .pipe(tap({
        next: () => {
          const index = this.categories.indexOf(category);
          if (index > -1) {
            this.categories.splice(index, 1);
          }
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
          const index = this.categories.indexOf(category);
          if (updatedCategory && index > -1) {
            this.categories.splice(index, 1, updatedCategory);
          }
        }
      }))
      .subscribe();
  }

  onClosedDialog(category: CategoryDto) {
    this.categories.push(category)
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
          },
          complete: () => { this.spinner.hide() }
        }
      ))
    .subscribe()
  }

}
