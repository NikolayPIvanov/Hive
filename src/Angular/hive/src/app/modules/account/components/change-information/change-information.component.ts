import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { pipe, Subject } from 'rxjs';
import { switchMap, takeUntil, tap } from 'rxjs/operators';
import { UpdateUserNamesCommand, UserProfileDto } from 'src/app/clients/profile-client';
import { ProfileService } from '../../services/profile.service';

@Component({
  selector: 'app-change-information',
  templateUrl: './change-information.component.html',
  styleUrls: ['./change-information.component.scss']
})
export class ChangeInformationComponent implements OnInit, OnDestroy {
  private subject = new Subject();

  @Input() profile!: UserProfileDto;

  form: FormGroup = this.fb.group({
    id: [this.profile?.id, Validators.required],
    givenName: [this.profile?.givenName, Validators.required],
    surname: [this.profile?.surname, Validators.required]
  });
  
  constructor(
    private fb: FormBuilder,
    private profileService: ProfileService) {
  }

  ngOnInit() {
    this.form.patchValue(this.profile!)
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
        switchMap(x => this.profileService.getProfile())
      )
      .subscribe();
  }
}
