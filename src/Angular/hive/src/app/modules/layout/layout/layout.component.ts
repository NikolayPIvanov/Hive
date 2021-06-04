import { MediaMatcher } from '@angular/cdk/layout';
import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription, timer } from 'rxjs';
import { AuthGuard } from '../../core/guards/auth.guard';
import { AuthenticationService } from '../../core/services/auth.service';
import { SpinnerService } from '../../core/services/spinner.service';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent implements OnInit, OnDestroy, AfterViewInit {

  private _mobileQueryListener: () => void;
  mobileQuery: MediaQueryList;
  showSpinner!: boolean;
  userName!: string | undefined;
  isAdmin!: boolean;

  private autoLogoutSubscription!: Subscription;

    constructor(private changeDetectorRef: ChangeDetectorRef,
      private router: Router,
      private media: MediaMatcher,
      public spinnerService: SpinnerService,
      private authService: AuthService,
      private authGuard: AuthGuard) {

      this.mobileQuery = this.media.matchMedia('(max-width: 1000px)');
      this._mobileQueryListener = () => changeDetectorRef.detectChanges();
      // tslint:disable-next-line: deprecation
      this.mobileQuery.addListener(this._mobileQueryListener);
  }

  ngOnInit(): void {
      const user = this.authService.user

      this.isAdmin = user?.profile.role.indexOf('Admin') > -1;
      this.userName = user?.profile.name;

      // Auto log-out subscription
      const watch = timer(2000, 5000);
      this.autoLogoutSubscription = watch.subscribe(t => {
          this.authGuard.canActivate();
      });
  }

  ngOnDestroy(): void {
      // tslint:disable-next-line: deprecation
      this.mobileQuery.removeListener(this._mobileQueryListener);
      this.autoLogoutSubscription.unsubscribe();
  }

  ngAfterViewInit(): void {
      this.changeDetectorRef.detectChanges();
  }
    
  navigate() {
    if (this.authService.user) {
        this.router.navigate(['/home'])
    }
    else {
        this.router.navigate(['/auth/login'])
    }
  }

  logout() {
    this.authService.logout();
  }

}
