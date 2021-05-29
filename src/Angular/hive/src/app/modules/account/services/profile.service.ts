import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { finalize, map, startWith, switchMap } from 'rxjs/operators';
import { ProfileClient, UpdateUserProfileCommand, UserProfileDto } from 'src/app/clients/profile-client';
import { SpinnerService } from '../../core/services/spinner.service';

@Injectable({
  providedIn: 'root'
})
export class ProfileService {
  private currentProfile = new BehaviorSubject<UserProfileDto | undefined>(undefined);
  public profile$: Observable<UserProfileDto | undefined> = this.currentProfile.asObservable();

  constructor(
    private profileApiClient: ProfileClient,
    private spinnerService: SpinnerService
  ) { }

  getProfile() {
    return this.profileApiClient.getProfile()
      .pipe(
        // startWith(() => this.spinnerService.show()),
        switchMap(profile => {
          this.currentProfile.next(profile!);
          return this.currentProfile;
        }),
        finalize(() => this.spinnerService.hide())
      )
  }

  updateProfile(profileId: number, command: UpdateUserProfileCommand) {
    return this.profileApiClient.updateProfile(profileId, command)
      .pipe(
        startWith(() => this.spinnerService.show()),
        switchMap(() => this.getProfile()),
        finalize(() => this.spinnerService.hide())
      )
  }
}
