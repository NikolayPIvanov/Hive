import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';
import { GigOverviewDto, GigsClient } from 'src/app/clients/gigs-client';

@Component({
  selector: 'app-gig-dashboard',
  templateUrl: './gig-dashboard.component.html',
  styleUrls: ['./gig-dashboard.component.scss']
})
export class GigDashboardComponent implements OnInit {
  private gigs: GigOverviewDto[] = [];
  private gigsSubject = new BehaviorSubject<GigOverviewDto[]>(this.gigs);
  public gigs$ = this.gigsSubject.asObservable();

  constructor(
    private router: Router,
    private gigsClient: GigsClient
  ) { }

  ngOnInit(): void {
    this.gigsClient.getRandom(10)
      .pipe(tap({
        next: (gigs) => {
          this.gigs = gigs.items!;
          this.gigsSubject.next(this.gigs);
        }
      }))
      .subscribe();
  }

}
