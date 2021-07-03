import { Injectable } from '@angular/core';
import { UserManager, User, UserManagerSettings } from 'oidc-client';
import { Subject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Constants } from './auth.constants';


export enum UserRole {
  Admin = "Admin",
  Buyer = "Buyer",
  Seller = "Seller",
  Investor = "Investor"
}


@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private _loginChangedSubject = new Subject<boolean>();

  private _userManager: UserManager;
  private _user: User | null = null;

  public role: UserRole | undefined;

  public loginChanged = this._loginChangedSubject.asObservable();
  public get token() {
    return this._user?.access_token;
  }

  private get idpSettings(): UserManagerSettings {
    const redi = environment.production ? `${Constants.clientRoot}/#` : Constants.clientRoot
    return {
      authority: Constants.idpAuthority,
      client_id: Constants.clientId,
      redirect_uri: `${redi}/auth/signin-callback`,
      scope: "openid profile",
      response_type: "code",
      post_logout_redirect_uri: `${redi}/signout-callback`
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
      this.role = UserRole[user?.profile.role as keyof typeof UserRole];
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
        if (this._user) {
          this.role = UserRole[this._user?.profile.role as keyof typeof UserRole];
        }
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
