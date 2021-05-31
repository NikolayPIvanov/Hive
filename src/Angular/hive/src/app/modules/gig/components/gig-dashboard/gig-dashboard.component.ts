import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-gig-dashboard',
  templateUrl: './gig-dashboard.component.html',
  styleUrls: ['./gig-dashboard.component.scss']
})
export class GigDashboardComponent implements OnInit {
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
