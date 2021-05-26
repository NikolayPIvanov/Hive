import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { map, switchMap, takeUntil } from 'rxjs/operators';
import { NotificationSettingDto, ProfileClient, UpdateUserProfileCommand, UserProfileDto } from 'src/app/clients/profile-client';

@Component({
  selector: 'app-account-overview',
  templateUrl: './account-overview.component.html',
  styleUrls: ['./account-overview.component.scss']
})
export class AccountOverviewComponent implements OnInit, OnDestroy {
  private subject = new Subject();
  form!: FormGroup;
  profile$!: Observable<boolean>;
  id: number | undefined;

  constructor(
    private profileApiClient: ProfileClient,
    private formBuilder: FormBuilder) { }

  ngOnInit(): void {
    this.form = this.formBuilder.group({
      id: new FormControl(null, Validators.required),
      userId: new FormControl(null, Validators.required),
      firstName: new FormControl('', Validators.maxLength(50)),
      lastName: new FormControl('', Validators.maxLength(50)),
      description: new FormControl(),
      education: new FormControl(),
      isTransient: new FormControl(false),
      notificationSettings: new FormGroup({
        emailNotifications: new FormControl(false)
      }),
      skills: new FormControl([]),
      languages: new FormControl([])
    })

    this.profile$ = this.profileApiClient.getProfile()
      .pipe(map(
        (profile: UserProfileDto) => {
          this.id = profile.id;
            this.form.patchValue(profile);
            return true;
          }))
  }

  get notificationForm(): FormGroup {
    return this.form.get('notificationSettings') as FormGroup;
  }

  get name(): string {
    const firstName = this.form.get('firstName')?.value;
    const lastName = this.form.get('lastName')?.value;
    if (firstName || lastName) {
      return (firstName || '') + " " + (lastName || '');
    }
    return 'User';
  }

  save() {
    let command = UpdateUserProfileCommand.fromJS(this.form.value);
    command.userProfileId = this.id;
    this.profileApiClient.updateProfile(this.id!, command)
      .pipe(
        takeUntil(this.subject),
        switchMap(_ => this.profileApiClient.getProfile()))
      .subscribe(profile => {
        debugger;
        this.form.patchValue(profile)
        // this.form.get('languages')?.value = profile.languages?.map(l => l.value);
      })
  }

  ngOnDestroy(): void {
    this.subject.next();
    this.subject.complete();
  }
}
