import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './components/login/login.component';

import { AuthRoutingModule } from './auth-routing.module';
import { LayoutModule } from '../layout/layout.module';
import { RegisterComponent } from './components/register/register.component';
import { SigninRedirectCallbackComponent } from './components/signin-redirect-callback/signin-redirect-callback.component';
import { SignoutRedirectCallbackComponent } from './components/signout-redirect-callback/signout-redirect-callback.component';

@NgModule({
  declarations: [
    LoginComponent,
    RegisterComponent,
    SigninRedirectCallbackComponent,
    SignoutRedirectCallbackComponent
  ],
  imports: [
    CommonModule,
    LayoutModule,
    AuthRoutingModule
  ]
})
export class AuthModule { }
