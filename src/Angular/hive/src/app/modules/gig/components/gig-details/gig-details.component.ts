import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, Observer, of, throwError } from 'rxjs';
import { tap } from 'rxjs/operators';
import { FileResponse, GigDto, GigsClient } from 'src/app/clients/gigs-client';
import { UserProfileDto } from 'src/app/clients/profile-client';
import { ProfileService } from 'src/app/modules/account/services/profile.service';
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
  public default = '/assets/no_image.png'
  public download!: Observable<FileResponse>;

  public gig$!: Observable<GigDto>;
  public profile$!: Observable<UserProfileDto | undefined>;

  public canModify: boolean = true;

  constructor(
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private gigsService: GigsService,
    private gigsClient: GigsClient,
    private userProfileService: ProfileService,
    public dialog: MatDialog
  ) {
  }

  ngOnInit(): void {
    const idParam = this.activatedRoute.snapshot.paramMap.get('id');
    if (idParam == null)
      throwError('Empty id parameter');
    const id = +idParam!;

    debugger;

    this.profile$ = this.userProfileService.getProfile();
    this.gig$ = this.gigsService.getGigDetailsById(id)
    this.download = this.gigsClient.getAvatar(id);
  }

  checkout() {
    this.router.navigate(['orders/checkout/2'])
  }

  edit(gig: GigDto) {
    const dialogRef = this.dialog.open(GigEditComponent, {
      width: '50%',
      data: gig
    });
  }

}
