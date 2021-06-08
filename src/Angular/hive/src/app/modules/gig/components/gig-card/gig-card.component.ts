import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { FileResponse, GigDto, GigOverviewDto, GigsClient } from 'src/app/clients/gigs-client';
import { UserProfileDto } from 'src/app/clients/profile-client';
import { AuthService } from 'src/app/modules/layout/services/auth.service';

@Component({
  selector: 'app-gig-card',
  templateUrl: './gig-card.component.html',
  styleUrls: ['./gig-card.component.scss']
})
export class GigCardComponent implements OnInit {
  @Input() gig!: GigOverviewDto;
  @Input() profile!: UserProfileDto;

  @Output() onDeleted = new EventEmitter<number>();

  public default = '/assets/no_image.png'

  constructor(
    private router: Router,
    private authService: AuthService,
    private gigsClient: GigsClient) { }
  
  public download!: Observable<FileResponse>;

  ngOnInit(): void {
    this.download = this.gigsClient.getAvatar(this.gig.id!);
  }
  
  onDetails() {
    this.router.navigate(['gigs', this.gig.id!, 'details'])
  }

  get isOwner() {
    return true;
  }

  deleteGig(id: number) {
    this.onDeleted.emit(id);
  }

}
