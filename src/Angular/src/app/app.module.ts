import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { filter } from 'rxjs/operators';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MenuComponent } from './menu/menu.component';
import { HomeComponent } from './home/home.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SharedModule } from './shared/shared.module';
import { CoreModule } from './core/core.module';
import { AuthorizationModule } from 'src/authorization/authorization.module';
import { DashboardModule } from './dashboard/dashboard.module';
import { API_BASE_URL } from './gigs-client';

@NgModule({
  declarations: [
    AppComponent,
    MenuComponent,
    HomeComponent,
    NotFoundComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    CoreModule,
    SharedModule,
    DashboardModule,
    AuthorizationModule,
    AppRoutingModule,
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