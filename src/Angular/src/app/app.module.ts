import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { filter } from 'rxjs/operators';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SharedModule } from './shared/shared.module';
import { CoreModule } from './core/core.module';
import { AuthorizationModule } from 'src/authorization/authorization.module';
import { DashboardModule } from './dashboard/dashboard.module';
import { API_BASE_URL } from './clients/gigs-client';
import { HomeModule } from './home/home.module';
import { ExploreModule } from './explore/explore.module';
import { GigManagementModule } from './gig-management/gig-management.module';

@NgModule({
  declarations: [
    AppComponent,
    NotFoundComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,

    CoreModule,
    SharedModule,

    HomeModule,
    DashboardModule,
    AuthorizationModule,
    ExploreModule,
    GigManagementModule,

    AppRoutingModule
  ],
  providers: [
    {
      provide: API_BASE_URL,
      useFactory: () => {
        return 'https://localhost:5057'
      }
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }