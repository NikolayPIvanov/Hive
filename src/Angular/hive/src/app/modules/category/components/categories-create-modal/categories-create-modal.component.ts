import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-categories-create-modal',
  templateUrl: './categories-create-modal.component.html',
  styleUrls: ['./categories-create-modal.component.scss']
})
export class CategoriesCreateModalComponent implements OnInit {

  categoryForm = this.fb.group({
    id: ['', Validators.required],
    title: ['', Validators.required],
    description: ['', Validators.required],
    parentId: ['']
  });
  
  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<CategoriesCreateModalComponent>,
    @Inject(MAT_DIALOG_DATA) public onlyParent: boolean) { }

  ngOnInit(): void {
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  onSubmit(): void {
    
  }

}
