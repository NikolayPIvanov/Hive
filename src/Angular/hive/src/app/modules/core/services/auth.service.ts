import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { of, EMPTY } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
    providedIn: 'root'
})
export class AuthenticationService {

    constructor(private http: HttpClient,
        @Inject('LOCALSTORAGE') private localStorage: Storage) {
    }

    register(name: string, email: string, password: string) {
        return of(true).pipe(
            map(() => true));
    }

    login(email: string, password: string) {
        return of(true).pipe(
            map((/*response*/) => {
                // set token property
                // const decodedToken = jwt_decode(response['token']);

                // store email and jwt token in local storage to keep user logged in between page refreshes
                this.localStorage.setItem('currentUser', JSON.stringify({
                    token: 'aisdnaksjdn,axmnczm',
                    isAdmin: true,
                    email: 'john.doe@gmail.com',
                    id: '12312323232',
                    alias: 'john.doe@gmail.com'.split('@')[0],
                    expiration: 9999999999999999999999,
                    fullName: 'John Doe'
                }));

                return true;
            }));
    }

    logout(): void {
        // clear token remove user from local storage to log user out
        this.localStorage.removeItem('currentUser');
    }

    getCurrentUser(): any {
        // TODO: Enable after implementation
        // return JSON.parse(this.localStorage.getItem('currentUser'));
        return {
            token: 'aisdnaksjdn,axmnczm',
            isAdmin: true,
            email: 'john.doe@gmail.com',
            id: '12312323232',
            alias: 'john.doe@gmail.com'.split('@')[0],
            expiration: 9999999999999999999999,
            fullName: 'John Doe'
        };
    }

    passwordResetRequest(email: string) {
        return of(true)
    }

    changePassword(email: string, currentPwd: string, newPwd: string) {
        return of(true)
    }

    passwordReset(email: string, token: string, password: string, confirmPassword: string): any {
        return of(true)
    }
}