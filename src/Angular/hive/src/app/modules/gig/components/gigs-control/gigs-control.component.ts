import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { UserProfileDto } from 'src/app/clients/profile-client';
import { ProfileService } from 'src/app/modules/account/services/profile.service';
import { AuthService } from 'src/app/modules/layout/services/auth.service';
import { GigCreateComponent } from '../gig-create/gig-create.component';

@Component({
  selector: 'app-gigs-control',
  templateUrl: './gigs-control.component.html',
  styleUrls: ['./gigs-control.component.scss']
})
export class GigsControlComponent implements OnInit {
  public profile$!: Observable<UserProfileDto | undefined>;

  elements = new Array(8);

  constructor(
    private profileService: ProfileService,
    private authService: AuthService,

    private router: Router,
    public dialog: MatDialog
  ) { }

  ngOnInit(): void {
    this.profile$ = this.profileService.getProfile()
  }

  onInspect() {
    this.router.navigate(['gigs', 2, 'details'])
  }

  onCreateNew() {
    this.dialog.open(GigCreateComponent, {
      width: '50%'
    });
  }

  displayName(profile: UserProfileDto) {
    const display = `${profile.firstName} ${profile.lastName}`;
    return display.trim() != '' ? display : this.authService.user?.profile.email;
  }

  get email() {
    return this.authService.user?.profile.email;
  }

}
