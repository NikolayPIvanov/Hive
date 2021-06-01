import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, Observer, of, throwError } from 'rxjs';
import { GigDto } from 'src/app/clients/gigs-client';
import { GigsService } from '../../services/gigs.service';
import { GigEditComponent } from '../gig-edit/gig-edit.component';

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
  public asyncTabs: Observable<ExampleTab[]>;

  constructor(
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private gigsService: GigsService,
    public dialog: MatDialog
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

    this.gig$ = of(GigDto.fromJS({}))
      // this.gigsService.getGigDetailsById(id)
  }

  checkout() {
    this.router.navigate(['orders/checkout/2'])
  }

  edit(gig: GigDto) {
    // only if owner and seller

    this.dialog.open(GigEditComponent, {
      width: '50%',
      data: gig
    })
  }

}
