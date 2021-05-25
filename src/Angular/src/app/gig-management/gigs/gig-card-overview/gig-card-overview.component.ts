import { Component, Input, OnInit } from '@angular/core';
import { GigOverviewDto } from 'src/app/clients/gigs-client';

@Component({
  selector: 'app-gig-card-overview',
  templateUrl: './gig-card-overview.component.html',
  styleUrls: ['./gig-card-overview.component.scss']
})
export class GigCardOverviewComponent implements OnInit {
  @Input('gig') gig!: GigOverviewDto;

  constructor() { }

  ngOnInit(): void {
  }

  get imageLocation(): string {
    if (this.gig.pictureUri) {
      return this.gig.pictureUri;
    }

    return './assets/icons/missing.png';
  }

}
