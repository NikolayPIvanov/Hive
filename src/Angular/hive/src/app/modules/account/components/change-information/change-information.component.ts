import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { NGXLogger } from 'ngx-logger';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { UpdateUserProfileCommand, UserProfileDto } from 'src/app/clients/profile-client';
import { AuthenticationService } from 'src/app/modules/core/services/auth.service';
import { NotificationService } from 'src/app/modules/core/services/notification.service';
import { SpinnerService } from 'src/app/modules/core/services/spinner.service';
import { ProfileService } from '../../services/profile.service';

@Component({
  selector: 'app-change-information',
  templateUrl: './change-information.component.html',
  styleUrls: ['./change-information.component.scss']
})
export class ChangeInformationComponent implements OnInit, OnDestroy {
  private subject = new Subject();
  private userProfile: UserProfileDto | undefined;

  form!: FormGroup;
  disableSubmit!: boolean;

  constructor(private authService: AuthenticationService,
    private logger: NGXLogger,
    private spinnerService: SpinnerService,
    private profileService: ProfileService,
    private notificationService: NotificationService) {
  }

  ngOnInit() {
    this.form = new FormGroup({
      firstName: new FormControl('', Validators.required),
      lastName: new FormControl('', Validators.required),
    });

    this.profileService.profile$
      .pipe(takeUntil(this.subject))
      .subscribe((profile: UserProfileDto | undefined) => this.userProfile = profile);

    this.spinnerService.visibility.subscribe((value) => {
      this.disableSubmit = value;
    });
  }
  
  ngOnDestroy(): void {
    this.subject.next();
    this.subject.complete();
  }

  updateProfile() {
    const fullName =
      this.form.get('firstName')!.value.trim() +
      ' ' +
      this.form.get('lastName')!.value.trim()

    const mockId = -1;
    const mockCommand = UpdateUserProfileCommand.fromJS({});

    this.profileService.updateProfile(mockId, mockCommand)
      .pipe(takeUntil(this.subject))
      .subscribe((profile: UserProfileDto | undefined) => this.userProfile = profile);
  }
}
