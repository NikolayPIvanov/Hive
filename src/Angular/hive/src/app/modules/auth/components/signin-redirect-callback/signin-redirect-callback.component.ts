import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/modules/layout/services/auth.service';

@Component({
  selector: 'app-signin-redirect-callback',
  template: `<div></div>`
})
export class SigninRedirectCallbackComponent implements OnInit {

  constructor(private _authService: AuthService, private _router: Router) { }
  
  ngOnInit(): void {
    debugger;
    this._authService.finishLogin()
    .then(_ => {
      this._router.navigate(['/home'], { replaceUrl: true });
    })
  }

}
