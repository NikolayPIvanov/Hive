import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatChipInputEvent } from '@angular/material/chips';
import { COMMA, ENTER } from "@angular/cdk/keycodes";
import { ProfileService } from '../../services/profile.service';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { UpdateUserProfileCommand, UserProfileDto } from 'src/app/clients/profile-client';

@Component({
  selector: 'app-account-seller-information',
  templateUrl: './account-seller-information.component.html',
  styleUrls: ['./account-seller-information.component.scss']
})
export class AccountSellerInformationComponent implements OnInit, OnDestroy {
  readonly separatorKeysCodes: number[] = [ENTER, COMMA];
  private subject = new Subject()
  
  form: FormGroup = this.fb.group({
    id: ['', Validators.required],
    description: [''],
    education: [''],
    skills: [[]],
    languages: [[]],
    notificationSettings: this.fb.group({
      emailNotifications: [false]
    })
  });

  constructor(
    private profileService: ProfileService,
    private fb: FormBuilder
  ) { }

  ngOnInit(): void {
    this.profileService.profile$
      .pipe(takeUntil(this.subject))
      .subscribe(profile => this.form.patchValue(profile!));
  }

  ngOnDestroy(): void {
    this.subject.next();
    this.subject.complete();
  }

  get skills(): FormControl { 
    return this.form.get('skills') as FormControl;
  }

  get languages(): FormControl {
    return this.form.get('languages') as FormControl;
  }

  add(control: FormControl, event: MatChipInputEvent) {
    const value = (event.value || '').trim();

    if (value) {
      control.value.push(value);
      control.updateValueAndValidity();
    }

    event.chipInput!.clear();
  } 

  remove(control: FormControl, value: any) {
    const index = (control.value as any[]).indexOf(value);
    if (index >= 0) {
      control.value.splice(index, 1);
      control.updateValueAndValidity();
    }
  }

  update() {
    const command = UpdateUserProfileCommand.fromJS(this.form.value);
    this.profileService.updateProfile(command.id!, command)
      .pipe(takeUntil(this.subject))
      .subscribe();
  }
}
