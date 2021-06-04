import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subject } from 'rxjs';
import { takeUntil, tap } from 'rxjs/operators';
import { CategoriesClient, CreateCategoryCommand } from 'src/app/clients/gigs-client';
import { NotificationService } from 'src/app/modules/core/services/notification.service';

@Component({
  selector: 'app-categories-create-modal',
  templateUrl: './categories-create-modal.component.html',
  styleUrls: ['./categories-create-modal.component.scss']
})
export class CategoriesCreateModalComponent implements OnInit {
  private unsubscribe = new Subject();

  categoryForm = this.fb.group({
    id: ['', Validators.required],
    title: ['', Validators.required],
    description: ['', Validators.required],
    parentId: ['']
  });
  
  constructor(
    private fb: FormBuilder,
    private categoriesClient: CategoriesClient,
    private notificationService: NotificationService,
    public dialogRef: MatDialogRef<CategoriesCreateModalComponent>,
    @Inject(MAT_DIALOG_DATA) public onlyParent: boolean) { }

  ngOnInit(): void {
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  onSubmit(): void {
    const command = CreateCategoryCommand.fromJS(this.categoryForm);
    this.categoriesClient.createCategory(command)
      .pipe(
        takeUntil(this.unsubscribe),
        tap({
          next: (id) => { this.notificationService.openSnackBar('Category created') },
          error: (error) => { this.notificationService.openSnackBar('Category creation failed')}
        })
      )
      .subscribe();
  }

}
