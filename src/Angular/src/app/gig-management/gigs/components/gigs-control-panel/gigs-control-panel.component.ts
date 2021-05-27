import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { GigOverviewDto, GigsClient } from 'src/app/clients/gigs-client';

@Component({
  selector: 'app-gigs-control-panel',
  templateUrl: './gigs-control-panel.component.html',
  styleUrls: ['./gigs-control-panel.component.scss']
})
export class GigsControlPanelComponent implements OnInit {
  gigs$!: Observable<GigOverviewDto>;

  constructor(private gigsApiClient: GigsClient) { }

  ngOnInit(): void {
    this.gigs$ = this.gigsApiClient.getGigs(null, undefined, undefined);
  }

}
