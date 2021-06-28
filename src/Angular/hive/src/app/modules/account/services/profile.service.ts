import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { BehaviorSubject, Observable } from 'rxjs';
import { finalize, map, startWith, switchMap, tap } from 'rxjs/operators';
import { ProfileClient, UpdateUserNamesCommand, UpdateUserProfileCommand, UserProfileDto } from 'src/app/clients/profile-client';

@Injectable({
  providedIn: 'root'
})
export class ProfileService {
  private currentProfile = new BehaviorSubject<UserProfileDto | undefined>(undefined);
  public profile$: Observable<UserProfileDto | undefined> = this.currentProfile.asObservable();

  constructor(
    private profileApiClient: ProfileClient,
    private spinnerService: NgxSpinnerService
  ) { }

  getProfile() {
    return this.profileApiClient.getMyProfile()
      .pipe(
        switchMap(profile => {
          this.currentProfile.next(profile!);
          return this.currentProfile;
        }),
        tap({ complete: () => this.spinnerService.hide() })
      )
  }

  updateProfile(profileId: number, command: UpdateUserProfileCommand) {
    return this.profileApiClient.updateProfile(profileId, command)
      .pipe(switchMap(() => this.getProfile()))
  }

  updateProfileNames(profileId: number, command: UpdateUserNamesCommand) {
    return this.profileApiClient.updateProfileNames(profileId, command);
  }
}
