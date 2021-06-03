import { Injectable } from '@angular/core';
import { UserManager, User, UserManagerSettings } from 'oidc-client';
import { Subject } from 'rxjs';
import { Constants } from './auth.constants';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private _loginChangedSubject = new Subject<boolean>();

  private _userManager: UserManager;
  private _user: User | null = null;

  public loginChanged = this._loginChangedSubject.asObservable();

  private get idpSettings() : UserManagerSettings {
    return {
      authority: Constants.idpAuthority,
      client_id: Constants.clientId,
      redirect_uri: `${Constants.clientRoot}/auth/signin-callback`,
      scope: "openid profile",
      response_type: "code",
      post_logout_redirect_uri: `${Constants.clientRoot}/signout-callback`
    }
  }
  constructor() { 
    this._userManager = new UserManager(this.idpSettings);
  }

  public login = () => {
    return this._userManager.signinRedirect();
  }

  public finishLogin = (): Promise<User> => {
    return this._userManager.signinRedirectCallback()
    .then(user => {
      this._user = user;
      this._loginChangedSubject.next(this.checkUser(user));
      return user;
    })
  }

  public logout = () => {
    this._userManager.signoutRedirect();
  }

  public finishLogout = () => {
    this._user = null;
    return this._userManager.signoutRedirectCallback();
  }

  public isAuthenticated = (): Promise<boolean> => {
    return this._userManager.getUser()
      .then(user => {
        if(this._user !== user){
          this._loginChangedSubject.next(this.checkUser(user));
        }
        this._user = user;
        return this.checkUser(user);
      })
  }

  public getAccessToken = (): Promise<string | null> => {
    return this._userManager.getUser()
      .then( (user: User | null) => {
         return !!user && !user.expired ? user.access_token : null;
    })
  }

  public get user(): User | null {
    return this._user;
  }

  private checkUser = (user : User | null): boolean => {
    return !!user && !user.expired;
  }
}
