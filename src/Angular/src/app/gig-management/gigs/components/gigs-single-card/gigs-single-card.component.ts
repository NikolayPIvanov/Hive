import { Component, Input, OnInit } from '@angular/core';
import { GigOverviewDto } from 'src/app/clients/gigs-client';

@Component({
  selector: 'app-gigs-single-card',
  templateUrl: './gigs-single-card.component.html',
  styleUrls: ['./gigs-single-card.component.scss']
})
export class GigsSingleCardComponent implements OnInit {
  @Input('gig') gig!: GigOverviewDto;

  constructor() { }

  ngOnInit(): void {
  }

}
