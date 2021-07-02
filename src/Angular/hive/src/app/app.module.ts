import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LoggerModule } from 'ngx-logger';
import { CoreModule } from './modules/core/core.module';
import { LayoutModule } from '@angular/cdk/layout';
import { MaterialModule } from './modules/material/material.module';
import { AuthModule } from './modules/auth/auth.module';
import { HomeModule } from './modules/home/home.module';
import { AccountModule } from './modules/account/account.module';
import { CategoryModule } from './modules/category/category.module';
import { GigModule } from './modules/gig/gig.module';
import { OrdersModule } from './modules/orders/orders.module';
import { NgxSpinnerModule } from "ngx-spinner";
import * as profileClient from './clients/profile-client';
import * as categoriesClient from './clients/gigs-client';
import * as billingClient from './clients/billing-client';
import * as orderingClient from './clients/ordering-client';
import * as investingClient from './clients/investing-client';

import { environment } from 'src/environments/environment';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,

    CoreModule,
    LayoutModule,

    MaterialModule.forRoot(),

    AuthModule,
    HomeModule,
    AccountModule,
    CategoryModule,
    GigModule,
    OrdersModule,

    AppRoutingModule,

    LoggerModule.forRoot({
      serverLoggingUrl: `http://my-api/logs`,
      level: environment.logLevel,
      serverLogLevel: environment.serverLogLevel
    }),
    NgxSpinnerModule
  ],
  providers: [
    { provide: profileClient.API_BASE_URL, useValue: environment.userManagementUrl },
    { provide: categoriesClient.API_BASE_URL, useValue: environment.gigsManagementUrl },
    { provide: billingClient.API_BASE_URL, useValue: environment.billingManagementUrl },
    { provide: orderingClient.API_BASE_URL, useValue: environment.orderingManagementUrl },
    { provide: investingClient.API_BASE_URL, useValue: environment.investingManagement },
  ],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class AppModule { }
