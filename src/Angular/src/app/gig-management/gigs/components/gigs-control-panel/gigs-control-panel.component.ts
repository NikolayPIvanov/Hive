import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { GigOverviewDto, GigsClient } from 'src/app/clients/gigs-client';
import {
  
} from '@angular/animations';
import { ProfileClient, UserProfileDto } from 'src/app/clients/profile-client';
import { map, startWith } from 'rxjs/operators';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-gigs-control-panel',
  templateUrl: './gigs-control-panel.component.html',
  styleUrls: ['./gigs-control-panel.component.scss']
})
export class GigsControlPanelComponent implements OnInit {
  gigs$!: Observable<GigOverviewDto>;
  profile$!: Observable<UserProfileDto>;

  private pageSize = 10;
  private pageNumber = 1;
  private searchKey = null;

  constructor(
    private gigsApiClient: GigsClient,
    private profileApiClient: ProfileClient) { }

  ngOnInit(): void {
    this.gigs$ = this.gigsApiClient.getGigs(this.pageNumber, this.pageSize, this.searchKey);
    this.profile$ = this.profileApiClient.getProfile();
  }
}
