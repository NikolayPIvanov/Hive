import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AccountRoutingModule } from './account-routing.module';
import { AccountOverviewComponent } from './components/account-overview/account-overview.component';
import { ReactiveFormsModule } from '@angular/forms';
import { LayoutModule } from '../layout/layout.module';
import { AvatarModule } from 'ngx-avatar';

@NgModule({
  declarations: [
    AccountOverviewComponent
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
