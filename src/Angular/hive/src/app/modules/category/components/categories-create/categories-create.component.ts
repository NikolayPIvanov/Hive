import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { CategoryDto } from 'src/app/clients/gigs-client';
import { CategoriesCreateModalComponent } from '../categories-create-modal/categories-create-modal.component';

@Component({
  selector: 'app-categories-create',
  templateUrl: './categories-create.component.html',
  styleUrls: ['./categories-create.component.scss']
})
export class CategoriesCreateComponent implements OnInit {
  @Output() onClosedDialog = new EventEmitter<CategoryDto>();

  constructor(public dialog: MatDialog) {}

  openDialog(onlyParent: boolean = false) {
    const dialog = this.dialog.open(CategoriesCreateModalComponent, { data: onlyParent, width: '30%' });
    dialog.afterClosed()
      .subscribe(newCategory => {
        if (newCategory) {
          this.onClosedDialog.emit(newCategory)
        }
      });
  }

  ngOnInit(): void {
  }

}
