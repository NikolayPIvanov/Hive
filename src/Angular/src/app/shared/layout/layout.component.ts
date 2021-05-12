import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription, timer } from 'rxjs';
import { AuthenticationService } from 'src/authorization/authentication.service';
import { MediaMatcher } from '@angular/cdk/layout';
import { AuthGuard } from 'src/app/core/guards/auth.guard';
import { ActivatedRoute, ActivatedRouteSnapshot, Route, Router } from '@angular/router';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent implements OnInit, OnDestroy, AfterViewInit {

  private _mobileQueryListener: () => void;
  mobileQuery: MediaQueryList;
  showSpinner: boolean | undefined;
  userName: string | undefined;
  isAdmin: boolean | undefined;
  title: string = 'Hive'

  private autoLogoutSubscription: Subscription | undefined;

  constructor(private changeDetectorRef: ChangeDetectorRef,
    private media: MediaMatcher,
      //public spinnerService: SpinnerService,
      private authService: AuthenticationService,
      private authGuard: AuthGuard,
      private activatedRoute: ActivatedRoute,
      private router: Router
  ) {
      this.mobileQuery = this.media.matchMedia('(max-width: 1000px)');
      this._mobileQueryListener = () => changeDetectorRef.detectChanges();
    this.mobileQuery.addEventListener("change", this._mobileQueryListener);
  }

  public logout = () => {
    this.authService.logout();
  }
  
  public isInRole(role: string) {
    return this.authService.isInRole(role)
  }

  ngOnInit(): void {
      const user = this.authService.getCurrentUser();

      this.isAdmin = true;
      this.userName = user?.profile.name;

      // Auto log-out subscription
      // const timer$ = timer(2000, 5000)
      // this.autoLogoutSubscription = timer$.subscribe(t => {
      //   this.authGuard.canActivate(this.activatedRoute.snapshot, this.router.routerState.snapshot)
      // });
  }

  ngOnDestroy(): void {
      this.mobileQuery.removeEventListener("change", this._mobileQueryListener);
      this.autoLogoutSubscription?.unsubscribe();
  }

  ngAfterViewInit(): void {
      this.changeDetectorRef.detectChanges();
  }

}
