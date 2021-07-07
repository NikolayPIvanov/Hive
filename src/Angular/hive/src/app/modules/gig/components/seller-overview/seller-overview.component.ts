import { Component, Input, OnInit } from '@angular/core';
import { Observable, of } from 'rxjs';
import { mergeMap, tap } from 'rxjs/operators';
import { FileResponse, SellersClient } from 'src/app/clients/gigs-client';
import { ProfileClient, UserProfileDto } from 'src/app/clients/profile-client';
import { ProfileService } from 'src/app/modules/account/services/profile.service';

@Component({
  selector: 'app-seller-overview',
  templateUrl: './seller-overview.component.html',
  styleUrls: ['./seller-overview.component.scss']
})
export class SellerOverviewComponent implements OnInit {
  @Input() profile!: UserProfileDto;

  public download!: Observable<FileResponse>;

  url = '/assets/user.png';

  constructor(private profileClient: ProfileClient) { }

  ngOnInit(): void {
    this.download = this.profileClient.getAvatar(this.profile.id!);
  }

}
