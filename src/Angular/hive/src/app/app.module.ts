import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LoggerModule } from 'ngx-logger';
import { environment } from 'src/environments/environment';
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
    { provide: profileClient.API_BASE_URL, useValue: 'http:localhost:5001' },
    { provide: categoriesClient.API_BASE_URL, useValue: 'http:localhost:5057' },
    { provide: billingClient.API_BASE_URL, useValue: 'http:localhost:5051' },
    { provide: orderingClient.API_BASE_URL, useValue: 'http:localhost:5041' },
    { provide: investingClient.API_BASE_URL, useValue: 'http:localhost:5031' },
  ],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class AppModule { }
