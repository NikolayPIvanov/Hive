import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, Observer, throwError } from 'rxjs';
import { GigDto } from 'src/app/clients/gigs-client';
import { GigsService } from '../../services/gigs.service';

export interface ExampleTab {
  label: string;
  content: string;
}

@Component({
  selector: 'app-gig-details',
  templateUrl: './gig-details.component.html',
  styleUrls: ['./gig-details.component.scss']
})
export class GigDetailsComponent implements OnInit {
  public gig$!: Observable<GigDto>;

  asyncTabs: Observable<ExampleTab[]>;

  constructor(
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private gigsService: GigsService
  ) {
    this.asyncTabs = new Observable((observer: Observer<ExampleTab[]>) => {
      setTimeout(() => {
        observer.next([
          {label: 'First', content: 'Content 1'},
          {label: 'Second', content: 'Content 2'},
          {label: 'Third', content: 'Content 3'},
        ]);
      }, 1000);
    });
  }

  ngOnInit(): void {
    const idParam = this.activatedRoute.snapshot.paramMap.get('id');
    if (idParam == null)
      throwError('Empty id parameter');
    
    const id = +idParam!;

    this.gig$ = this.gigsService.getGigDetailsById(id)
  }

  checkout() {
    this.router.navigate(['orders/checkout/2'])
  }

}
