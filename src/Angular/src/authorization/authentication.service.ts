import { Injectable } from '@angular/core';
import { UserManager, User, UserManagerSettings } from 'oidc-client';
import { Subject } from 'rxjs';
import { Constants } from './authorization.constants';
import jwt_decode from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  private _userManager: UserManager;
  private _user: User | null = null;
  private _loginChangedSubject = new Subject<boolean>();
  
  private get idpSettings() : UserManagerSettings {
    return {
      authority: Constants.idpAuthority,
      client_id: Constants.clientId,
      redirect_uri: `${Constants.clientRoot}/signin-callback`,
      scope: "openid profile gigs.read gigs.write gigs.delete",
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

  public isInRole(role: string): boolean {
    let user = this.getCurrentUser();
    if (!user) return false;

    let token = user.access_token;
    let decodedToken: any = jwt_decode(token!);
    
    let isInRole = (decodedToken.role as string[]).indexOf(role) > -1;
    return isInRole;
  }
  

  public loginChanged = this._loginChangedSubject.asObservable();

  public isAuthenticated = (): Promise<boolean> => {
    return this._userManager.getUser()
      .then(user => {
        if(this._user !== user){
          this._loginChangedSubject.next(this.checkUser(user!));
        }
        this._user = user!;
        return this.checkUser(user!);
    })
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

  public getCurrentUser = () => this._user;

  private checkUser = (user : User): boolean => {
    return !!user && !user.expired;
  }


}
