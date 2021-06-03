import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router, Route } from '@angular/router';
import { NgxSpinner, NgxSpinnerService } from 'ngx-spinner';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { AuthService } from './modules/layout/services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy {
  private unsubscribe = new Subject();
  title = 'hive';

  public userAuthenticated = false;
  
  constructor(private _authService: AuthService){
    this._authService.loginChanged
      .pipe(takeUntil(this.unsubscribe))
      .subscribe(userAuthenticated => {
        this.userAuthenticated = userAuthenticated;
      })
  }
  
  ngOnInit(): void {
    this._authService.isAuthenticated()
      .then(userAuthenticated => {
        this.userAuthenticated = userAuthenticated;
      })
  }

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }
}
