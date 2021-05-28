import { NgModule } from '@angular/core';
import { filter } from 'rxjs/operators';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { SharedModule } from './shared/shared.module';
import { CoreModule } from './core/core.module';
import { AuthorizationModule } from 'src/authorization/authorization.module';
import { DashboardModule } from './dashboard/dashboard.module';
import * as gigsClient from './clients/gigs-client';
import * as profileClient from './clients/profile-client';
import * as billingClient from './clients/billing-client';
import { HomeModule } from './home/home.module';
import { ExploreModule } from './explore/explore.module';
import { GigManagementModule } from './gig-management/gig-management.module';
import { AccountModule } from './account/account.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BrowserModule } from '@angular/platform-browser';

@NgModule({
  declarations: [
    AppComponent,
    NotFoundComponent
  ],
  imports: [
    CoreModule,
    SharedModule,

    BrowserModule,
    BrowserAnimationsModule,

    HomeModule,
    DashboardModule,
    AuthorizationModule,
    ExploreModule,
    GigManagementModule,
    AccountModule,

    AppRoutingModule
  ],
  providers: [
    {
      provide: gigsClient.API_BASE_URL,
      useFactory: () => {
        return 'https://localhost:5057'
      }
    },
    {
      provide: profileClient.API_BASE_URL,
      useFactory: () => {
        return 'https://localhost:5001'
      }
    },
    {
      provide: billingClient.API_BASE_URL,
      useFactory: () => {
        return 'https://localhost:5051'
      }
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }