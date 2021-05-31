import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';

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
  public hasParent: boolean = true;
  public hasChildren: boolean = true;
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
    title: ['Web Design & Layout', Validators.required],
    description: ['Achieve the maximum speed possible on the Web Platform today, and take it further, via Web Workers and server-side rendering. Angular puts you in control over scalability. Meet huge data requirements by building data models on RxJS, Immutable.js or another push-model.', Validators.required],
    parentId: ['']
  });

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<CategoryDetailsComponent>) { }

  ngOnInit(): void {
  }

  onSubmit(): void {
    
  }

}
