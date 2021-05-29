import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './components/login/login.component';

import { AuthRoutingModule } from './auth-routing.module';
import { LayoutModule } from '../layout/layout.module';
import { RegisterComponent } from './components/register/register.component';

@NgModule({
  declarations: [
    LoginComponent,
    RegisterComponent
  ],
  imports: [
    CommonModule,
    LayoutModule,
    AuthRoutingModule
  ]
})
export class AuthModule { }
