import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { NGXLogger } from 'ngx-logger';
import { NotificationService } from 'src/app/modules/core/services/notification.service';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss']
})
export class ChangePasswordComponent implements OnInit {

  form!: FormGroup;
  hideCurrentPassword: boolean;
  hideNewPassword: boolean;
  currentPassword!: string;
  newPassword!: string;
  newPasswordConfirm!: string;
  disableSubmit!: boolean;

  constructor(
    private logger: NGXLogger,
    private notificationService: NotificationService) {

    this.hideCurrentPassword = true;
    this.hideNewPassword = true;
  }

  ngOnInit() {
    this.form = new FormGroup({
      currentPassword: new FormControl('', Validators.required),
      newPassword: new FormControl('', Validators.required),
      newPasswordConfirm: new FormControl('', Validators.required),
    });

    this.form.get('currentPassword')!.valueChanges
      .subscribe(val => { this.currentPassword = val; });

    this.form.get('newPassword')!.valueChanges
      .subscribe(val => { this.newPassword = val; });

    this.form.get('newPasswordConfirm')!.valueChanges
      .subscribe(val => { this.newPasswordConfirm = val; });
  }

  changePassword() {
  }

}
