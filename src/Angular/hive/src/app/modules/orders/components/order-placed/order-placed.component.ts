import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-order-placed',
  templateUrl: './order-placed.component.html',
  styleUrls: ['./order-placed.component.scss']
})
export class OrderPlacedComponent implements OnInit {
  orderNumber!: string;

  constructor(private activatedRoute: ActivatedRoute, private router: Router) { }

  ngOnInit(): void {
    this.orderNumber = this.activatedRoute.snapshot.paramMap.get('id')!;
  }

  navigateToGigs() {
    this.router.navigate(['gigs/dashboard']);
  }

}
