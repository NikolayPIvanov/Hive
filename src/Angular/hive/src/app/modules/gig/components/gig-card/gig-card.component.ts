import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { FileResponse, GigDto, GigOverviewDto, GigsClient } from 'src/app/clients/gigs-client';
import { ProfileClient, UserProfileDto } from 'src/app/clients/profile-client';
import { AuthService } from 'src/app/modules/layout/services/auth.service';

@Component({
  selector: 'app-gig-card',
  templateUrl: './gig-card.component.html',
  styleUrls: ['./gig-card.component.scss']
})
export class GigCardComponent implements OnInit {

  @Input() gig!: GigOverviewDto;
  @Input() profile: UserProfileDto | undefined;

  private profileSubject = new BehaviorSubject<UserProfileDto | undefined>(undefined);
  public profile$ = this.profileSubject.asObservable();

  @Output() onDeleted = new EventEmitter<number>();

  public default = '/assets/no_image.png'

  constructor(
    private router: Router,
    private authService: AuthService,
    private profileClient: ProfileClient,
    private gigsClient: GigsClient) { }
  
  public download!: Observable<FileResponse>;

  ngOnInit(): void {
    this.download = this.gigsClient.getAvatar(this.gig.id!);
    if (this.profile) {
      this.profileSubject.next(this.profile);
    } else {
      this.profileClient.getProfileById(this.gig!.sellerUserId!)
        .pipe(tap({
          next: (profile) => {
            this.profileSubject.next(profile);
          }
        }))
        .subscribe();
    }
  }
  
  onDetails() {
    this.router.navigate(['gigs', this.gig.id!, 'details'])
  }

  get isOwner() {
    return this.gig.sellerUserId === this.authService.user?.profile.sub;
  }

  deleteGig(id: number) {
    this.onDeleted.emit(id);
  }

}
