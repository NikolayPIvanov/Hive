import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { NgxSpinnerService } from 'ngx-spinner';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { CategoriesClient, CategoryDto, UpdateCategoryCommand } from 'src/app/clients/gigs-client';

export interface Section {
  name: string;
  updated: Date;
}

@Component({
  selector: 'app-category-details',
  templateUrl: './category-details.component.html',
  styleUrls: ['./category-details.component.scss']
})
export class CategoryDetailsComponent implements OnInit {
  folders: Section[] = [
    {
      name: 'Photos',
      updated: new Date('1/1/16'),
    },
    {
      name: 'Recipes',
      updated: new Date('1/17/16'),
    },
    {
      name: 'Work',
      updated: new Date('1/28/16'),
    }
  ];
  notes: Section[] = [
    {
      name: 'Vacation Itinerary',
      updated: new Date('2/20/16'),
    },
    {
      name: 'Kitchen Remodel',
      updated: new Date('1/18/16'),
    }
  ];


  public editMode: boolean = false;

  categoryForm = this.fb.group({
    id: ['', Validators.required],
    title: ['', Validators.required],
    description: ['', Validators.required],
    parentId: ['']
  });

  category$!: Observable<CategoryDto>;

  constructor(
    private fb: FormBuilder,
    private categoryApiClient: CategoriesClient,
    private spinner: NgxSpinnerService,
    public dialogRef: MatDialogRef<CategoryDetailsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: number) { }

  ngOnInit(): void {
    this.spinner.show();
    this.category$ = this.categoryApiClient.getCategoryById(this.data)
      .pipe(tap({
        next: (data) => {
          this.categoryForm.patchValue(data);
          this.categoryForm.patchValue({ parentId: data.parentOverview?.id})
        },
        complete: () => this.spinner.hide()
      }))
  }

  onSubmit(): void {
    this.spinner.show()

    const command = UpdateCategoryCommand.fromJS(this.categoryForm.value)
    this.categoryApiClient.updateCategory(this.data, command)
      .pipe(tap({ complete: () => this.spinner.hide() }))
      .subscribe();
  }

}
