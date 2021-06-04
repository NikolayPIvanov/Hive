import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { NGXLogger } from 'ngx-logger';
import { pipe, Subject } from 'rxjs';
import { switchMap, takeUntil } from 'rxjs/operators';
import { UpdateUserNamesCommand, UserProfileDto } from 'src/app/clients/profile-client';
import { AuthenticationService } from 'src/app/modules/core/services/auth.service';
import { NotificationService } from 'src/app/modules/core/services/notification.service';
import { SpinnerService } from 'src/app/modules/core/services/spinner.service';
import { AuthService } from 'src/app/modules/layout/services/auth.service';
import { ProfileService } from '../../services/profile.service';

@Component({
  selector: 'app-change-information',
  templateUrl: './change-information.component.html',
  styleUrls: ['./change-information.component.scss']
})
export class ChangeInformationComponent implements OnInit, OnDestroy {
  private subject = new Subject();

  form: FormGroup = this.fb.group({
    id: ['', Validators.required],
    firstName: [''],
    lastName: ['']
  });
  
  constructor(
    private fb: FormBuilder,
    private profileService: ProfileService) {
  }

  ngOnInit() {
    this.profileService.profile$
      .pipe(takeUntil(this.subject))
      .subscribe((profile: UserProfileDto | undefined) => {
        if (profile) {
          this.form.patchValue(profile!)
        }
      });
  }
  
  ngOnDestroy(): void {
    this.subject.next();
    this.subject.complete();
  }

  updateProfile() {
    const command = UpdateUserNamesCommand.fromJS(this.form.value);
    this.profileService.updateProfileNames(command.id!, command)
      .pipe(
        takeUntil(this.subject),
        switchMap(x => {
          return this.profileService.getProfile()
        })
      )
      .subscribe();
  }
}
