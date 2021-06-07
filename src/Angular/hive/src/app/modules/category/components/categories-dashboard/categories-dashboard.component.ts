import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { NgxSpinnerService } from 'ngx-spinner';
import { Observable } from 'rxjs';
import { map, switchMap, takeUntil, tap } from 'rxjs/operators';
import { CategoriesClient, CategoryDto, PaginatedListOfCategoryDto } from 'src/app/clients/gigs-client';
import { CategoryDetailsComponent } from '../category-details/category-details.component';

@Component({
  selector: 'app-categories-dashboard',
  templateUrl: './categories-dashboard.component.html',
  styleUrls: ['./categories-dashboard.component.scss']
})
export class CategoriesDashboardComponent implements OnInit {
  pageNumber = 1;
  pageSize = 10;
  searchKey: string | null = null;

  public categories$!: Observable<PaginatedListOfCategoryDto>;

  constructor(
    public dialog: MatDialog,
    private spinner: NgxSpinnerService,
    private categoriesApiClient: CategoriesClient) { }

  ngOnInit(): void {
    this.fetchCategories();
  }

  selected(name: string) {
    this.searchKey = name;
    this.fetchCategories();
  }

  onClosedDialog(number: any) {
    this.fetchCategories();
  }

  delete(id: number) {
    this.categoriesApiClient.deleteCategory(id)
      .pipe(map(x => this.fetchCategories()))
      .subscribe()
  }

  openDialog(category: CategoryDto) {
    const dialogRef = this.dialog.open(CategoryDetailsComponent,
    {
      width: "50%",
      data: category
    });

    dialogRef.afterClosed()
      .pipe(tap({ complete: () => this.fetchCategories() }))
      .subscribe();
  }

  private fetchCategories() {
    this.spinner.show();

    this.categories$ = this.categoriesApiClient
      .getCategories(this.pageNumber, this.pageSize, undefined, this.searchKey)
      .pipe(tap(
        {
          complete: () => { this.spinner.hide() }
        }
      ));
  }

}
