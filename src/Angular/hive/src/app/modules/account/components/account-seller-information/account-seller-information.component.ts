import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MatChipInputEvent } from '@angular/material/chips';
import { COMMA, ENTER } from "@angular/cdk/keycodes";
import { ProfileService } from '../../services/profile.service';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { UserProfileDto } from 'src/app/clients/profile-client';

@Component({
  selector: 'app-account-seller-information',
  templateUrl: './account-seller-information.component.html',
  styleUrls: ['./account-seller-information.component.scss']
})
export class AccountSellerInformationComponent implements OnInit, OnDestroy {
  private subject = new Subject()
  readonly separatorKeysCodes: number[] = [ENTER, COMMA];
  form!: FormGroup;
  profile: UserProfileDto | undefined;

  constructor(
    private profileService: ProfileService
  ) { }

  ngOnInit(): void {
    this.form = new FormGroup({
      description: new FormControl(''),
      education: new FormControl(''),
      skills: new FormControl([]),
      languages: new FormControl([]),
      notificationSettings: new FormGroup({
        emailNotifications: new FormControl(true)
      })
    });

    this.profileService.getProfile()
      .pipe(takeUntil(this.subject))
      .subscribe();
    
    this.profileService.profile$
      .pipe(takeUntil(this.subject))
      .subscribe(profile => this.profile = profile);
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
}
