import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AccountRoutingModule } from './account-routing.module';
import { AccountOverviewComponent } from './components/account-overview/account-overview.component';
import { MatCardModule } from '@angular/material/card';
import { AccountDetailsComponent } from './components/account-details/account-details.component';
import { ChangePasswordComponent } from './components/change-password/change-password.component';
import {MatFormFieldModule} from '@angular/material/form-field';
import { ReactiveFormsModule } from '@angular/forms';
import { MatTabsModule } from '@angular/material/tabs';
import { CoreModule } from '../core/core.module';
import { LayoutModule } from '../layout/layout.module';
import { ChangeInformationComponent } from './components/change-information/change-information.component';
import { AccountSellerInformationComponent } from './components/account-seller-information/account-seller-information.component';

@NgModule({
  declarations: [
    AccountOverviewComponent,
    AccountDetailsComponent,
    ChangePasswordComponent,
    ChangeInformationComponent,
    AccountSellerInformationComponent
  ],
  imports: [
    CommonModule,
    
    ReactiveFormsModule,

    LayoutModule,

    AccountRoutingModule
  ]
})
export class AccountModule { }
