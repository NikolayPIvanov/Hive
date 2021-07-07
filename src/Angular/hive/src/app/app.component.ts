import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router, Route } from '@angular/router';
import { NgxSpinner, NgxSpinnerService } from 'ngx-spinner';
import { Subject } from 'rxjs';
import { catchError, map, switchMap, takeUntil } from 'rxjs/operators';
import { ChatService } from './modules/chat/services/chat.service';
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
  
  constructor(private _authService: AuthService, private chatService: ChatService) {
    this._authService.loginChanged
      .pipe(
        takeUntil(this.unsubscribe),
        map(userAuthenticated => {
          this.userAuthenticated = userAuthenticated;
          return userAuthenticated;
        }),
        switchMap(x => this.chatService.connect()),
        switchMap(x => {
          const userId = this._authService.user?.profile.sub!;
          return this.chatService.fetchUUID(userId, true)
            .pipe(catchError(() => this.chatService.generateUUID(userId)))
        })
      )
      .subscribe()
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
