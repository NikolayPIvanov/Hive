import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { GigCreateComponent } from '../gig-create/gig-create.component';

@Component({
  selector: 'app-gigs-control',
  templateUrl: './gigs-control.component.html',
  styleUrls: ['./gigs-control.component.scss']
})
export class GigsControlComponent implements OnInit {
  elements = new Array(8);

  constructor(
    private router: Router,
    public dialog: MatDialog
  ) { }

  ngOnInit(): void {
  }

  onInspect() {
    this.router.navigate(['gigs', 2, 'details'])
  }

  onCreateNew() {
    this.dialog.open(GigCreateComponent, {
      width: '50%'
    });
  }

}
