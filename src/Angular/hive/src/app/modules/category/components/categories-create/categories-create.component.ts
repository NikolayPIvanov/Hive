import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { CategoriesCreateModalComponent } from '../categories-create-modal/categories-create-modal.component';

@Component({
  selector: 'app-categories-create',
  templateUrl: './categories-create.component.html',
  styleUrls: ['./categories-create.component.scss']
})
export class CategoriesCreateComponent implements OnInit {
  @Output() onClosedDialog = new EventEmitter<number>();

  constructor(public dialog: MatDialog) {}

  openDialog(onlyParent: boolean = false) {
    const dia = this.dialog.open(CategoriesCreateModalComponent, { data: onlyParent });
    dia.afterClosed()
      .subscribe(result => this.onClosedDialog.emit(1));
  }

  ngOnInit(): void {
  }

}
