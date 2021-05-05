import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';
import { GigDto, GigsClient } from 'src/app/gigs-client';

@Component({
  selector: 'app-gigs-details',
  templateUrl: './gigs-details.component.html',
  styleUrls: ['./gigs-details.component.scss']
})
export class GigsDetailsComponent implements OnInit {
  gig$!: Observable<GigDto>;

  constructor(private route: ActivatedRoute, private gigsApiClient: GigsClient) { }

  ngOnInit(): void {
    this.gig$ = this.route.params
      .pipe(map(params => +params['id']),
            switchMap(id => this.gigsApiClient.getGigById(id)));
  }

}
