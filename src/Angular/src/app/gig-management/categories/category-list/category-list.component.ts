import { Component, OnDestroy, OnInit } from '@angular/core';
import { Observable, of, Subscription } from 'rxjs';
import { CategoriesClient, CategoryDto, CreateCategoryCommand, PaginatedListOfCategoryDto, UpdateCategoryCommand } from 'src/app/clients/gigs-client';
import { NotificationService } from 'src/app/core/services/notification.service';
import {PageEvent} from '@angular/material/paginator';
import { map, switchMap } from 'rxjs/operators';
import { MatDialog } from '@angular/material/dialog';
import { CategoryCreateComponent } from '../category-create/category-create.component';
import { FormGroup } from '@angular/forms';
import { CategoryUpdateComponent } from '../category-update/category-update.component';

@Component({
  selector: 'app-category-list',
  templateUrl: './category-list.component.html',
  styleUrls: ['./category-list.component.scss']
})
export class CategoryListComponent implements OnInit {
  private deleteSubscription!: Subscription;
  private gigsSubscription!: Subscription;

  pageIndex: number = 1;
  pageSize: number = 10;
  onlyParents: boolean = false;

  paginatedList$!: PaginatedListOfCategoryDto;

  constructor(
    private categoriesApiClient: CategoriesClient,
    private notificationService: NotificationService,
    public dialog: MatDialog) { }

  ngOnInit(): void {
    this._load();
  }

  openUpdateDialog(id: number): void {
    const dialogRef = this.dialog.open(CategoryUpdateComponent, {
      width: '250px',
      data: {id: id}
    });

    dialogRef.afterClosed().subscribe((updatedCategory => {
      console.log('The dialog was closed');
      if (updatedCategory) {
        debugger;
        const index = this.paginatedList$.items?.findIndex((value, index, a) => value.id == updatedCategory.id)
        this.paginatedList$.items?.splice(index!, 1)
        this.paginatedList$.items?.push(updatedCategory);
      }
    }))
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(CategoryCreateComponent, {
      width: '250px',
      data: null
    });

    dialogRef.afterClosed().subscribe((result => {
      console.log('The dialog was closed');
      if (result) {
        this.onSubmit(result);
      }
    }))
  }

  onSubmit(form: FormGroup) {
    let data: any = {};
    for (let key in form.controls) {
      data[key] = form.controls[key].value
    }
    const command = CreateCategoryCommand.fromJS(data)

    this.categoriesApiClient.createCategory(command)
      .pipe(switchMap((id: number) => this.categoriesApiClient.getCategoryById(id)))
      .subscribe((category: CategoryDto) => this.setCategories([...this.paginatedList$.items!, category]));
  }

  setCategories($event: CategoryDto[]) {
    this.paginatedList$.items = $event
  }

  handlePageEvent(event: PageEvent) {
    this.pageSize = event.pageSize;
    this.pageIndex = event.pageIndex;
    this._load();
  }

  clear() {
    this._load();
  }

  deleteCategory(categoryId: number) {
    this.deleteSubscription = this.categoriesApiClient.deleteCategory(categoryId)
      .pipe(
        map(_ => {
          const index = this.paginatedList$.items?.findIndex((value, index, a) => value.id == categoryId)
          this.paginatedList$.items?.splice(index!, 1)
        }),
        map(x => (response: any) => this.notificationService.openSnackBar('Category Deleted')),
      )
      .subscribe();
  }

  private _load() {
    this.gigsSubscription =
      this.categoriesApiClient.getCategories(this.pageIndex, this.pageSize, this.onlyParents)
        .subscribe((x: any) => this.paginatedList$ = x);
  }
}
