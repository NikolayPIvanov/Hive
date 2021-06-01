import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-gigs-control',
  templateUrl: './gigs-control.component.html',
  styleUrls: ['./gigs-control.component.scss']
})
export class GigsControlComponent implements OnInit {
  elements = new Array(8);

  constructor(
    private router: Router
  ) { }

  ngOnInit(): void {
  }

  onInspect() {
    this.router.navigate(['gigs', 2, 'details'])
  }

}
