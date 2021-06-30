import { ENTER, COMMA } from '@angular/cdk/keycodes';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatChipInputEvent } from '@angular/material/chips';
import { Title } from '@angular/platform-browser';
import { NgxSpinnerService } from 'ngx-spinner';
import { Observable } from 'rxjs';
import { switchMap, tap } from 'rxjs/operators';
import { FileUpload, ProfileClient, UpdateUserProfileCommand, UserProfileDto } from 'src/app/clients/profile-client';
import { NotificationService } from 'src/app/modules/core/services/notification.service';
import { AuthService } from 'src/app/modules/layout/services/auth.service';
import { ProfileService } from '../../services/profile.service';

@Component({
  selector: 'app-account-overview',
  templateUrl: './account-overview.component.html',
  styleUrls: ['./account-overview.component.scss']
})
export class AccountOverviewComponent implements OnInit {
  public profile$!: Observable<UserProfileDto | undefined>;
  public isSeller = false;
  readonly separatorKeysCodes: number[] = [ENTER, COMMA];

  form: FormGroup = this.fb.group({
    id: ['', Validators.required],
    givenName: ['', Validators.required],
    surname: ['', Validators.required],
    bio: [''],
    education: [''],
    skills: [[]],
    languages: [[]]
  });

  public profile!: UserProfileDto;


  constructor(
    private fb: FormBuilder,
    private titleService: Title,
    private authService: AuthService,
    private spinnerService: NgxSpinnerService,
    private notificationService: NotificationService,
    private profileClient: ProfileClient,
    private profileService: ProfileService) { }

  ngOnInit(): void {
    this.titleService.setTitle('Hive - Profile');
    this.spinnerService.show();
    this.isSeller = (this.authService.user?.profile.role as string[]).includes('Seller')
    
    this.profile$! = this.profileService.getProfile()
      .pipe(tap({
        next: (profile) => {
          this.profile = profile!;
          this.form.patchValue(profile!)
        },
        complete: () => this.spinnerService.hide()
      }));
  }
  
  onUpload($event: any) {
    this.onFileSelected($event)
  }

  onFileSelected(event: any) {
    if (event.target.files && event.target.files[0]) {
      const reader = new FileReader();
      reader.readAsDataURL(event.target.files[0]);

      reader.onload = (event) => { // called once readAsDataURL is completed
        const x = <string>event.target!.result;
        var base64result = x.split(',')[1];
        const upload = FileUpload.fromJS({ fileData: base64result });

        this.profileClient.changeAvatar(this.profile.id!, upload)
          .pipe(switchMap(() => this.profileService.getProfile()),
            tap({
              next: (profile) => {
                this.profile = profile!;
                this.form.patchValue(profile!)
              },
              complete: () => this.spinnerService.hide()
            }))
          .subscribe();
      }
    }
  }

  update() {
    const command = UpdateUserProfileCommand.fromJS(this.form.value);
    this.profileService.updateProfile(command.id!, command)
      .pipe(tap({
        next: () => this.notificationService.openSnackBar('Successfully saved changes'),
        error: () => this.notificationService.openSnackBar('Saving changes failed'),
      }))
      .subscribe();
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
