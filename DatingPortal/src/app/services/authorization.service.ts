import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class AuthorizationService {
  constructor(
    private httpClient: HttpClient,
    private jwtHelper: JwtHelperService) { }

  public isLoggedIn() {
    return !this.jwtHelper.isTokenExpired();
  }

  public LoggedInUser() {
    const token = this.jwtHelper.decodeToken();
    return token.unique_name;
  }

  login(userName: string, password: string) {
    const loginUrl = environment.baseUrl + 'users/login';
    const user = {
      UserName: userName,
      Password: password
    };

    return this.httpClient.post(loginUrl, user)
      .pipe(
        map((value: any) => {
          console.log(value);
          if (!!value) {
            localStorage.setItem('access_token', value.token);
            const token = this.jwtHelper.decodeToken();
            console.log(token);
          }
        })
      );
  }

  logout() {
    localStorage.removeItem('access_token');
  }

  register(userName: string, password: string) {
    const registerUrl = environment.baseUrl + 'users/register';
    const user = {
      UserName: userName,
      Password: password
    };

    return this.httpClient.post(registerUrl, user);
  }

  getAllUsers() {
    const url = environment.baseUrl + 'users/AllUsers';
    return this.httpClient.get(url);
  }
}
