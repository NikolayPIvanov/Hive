import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { CategoriesCreateModalComponent } from '../categories-create-modal/categories-create-modal.component';

@Component({
  selector: 'app-categories-create',
  templateUrl: './categories-create.component.html',
  styleUrls: ['./categories-create.component.scss']
})
export class CategoriesCreateComponent implements OnInit {
  constructor(public dialog: MatDialog) {}

  openDialog(onlyParent: boolean = false) {
    this.dialog.open(CategoriesCreateModalComponent, { data: onlyParent });
  }

  ngOnInit(): void {
  }

}
