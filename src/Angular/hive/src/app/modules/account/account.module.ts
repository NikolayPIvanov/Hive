import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AccountRoutingModule } from './account-routing.module';
import { AccountOverviewComponent } from './components/account-overview/account-overview.component';
import { AccountDetailsComponent } from './components/account-details/account-details.component';
import { ChangePasswordComponent } from './components/change-password/change-password.component';
import { ReactiveFormsModule } from '@angular/forms';
import { LayoutModule } from '../layout/layout.module';
import { ChangeInformationComponent } from './components/change-information/change-information.component';
import { AccountSellerInformationComponent } from './components/account-seller-information/account-seller-information.component';
import { AccountAvatarComponent } from './components/account-avatar/account-avatar.component';
import { AngularFileUploaderModule } from 'angular-file-uploader';
import { AvatarModule } from 'ngx-avatar';

@NgModule({
  declarations: [
    AccountOverviewComponent,
    AccountDetailsComponent,
    ChangePasswordComponent,
    ChangeInformationComponent,
    AccountSellerInformationComponent,
    AccountAvatarComponent
  ],
  imports: [
    CommonModule,
    
    ReactiveFormsModule,

    LayoutModule,

    AccountRoutingModule,

    AvatarModule
  ]
})
export class AccountModule { }
