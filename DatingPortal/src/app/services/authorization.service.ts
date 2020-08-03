import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthorizationService {
  constructor(private httpClient: HttpClient) {}

  login(userName: string, password: string)
  {
    const loginUrl = environment.baseUrl + 'users/login';
    const user = {
      UserName: userName,
      Password: password
    };

    return this.httpClient.post(loginUrl, user)
      .pipe(
        map((value: any) =>
          {
            console.log(value);
            if (!!value )
            {
              localStorage.setItem('Token', value.token);
            }
          })
      );
  }

  register(userName: string, password: string)
  {
    const registerUrl = environment.baseUrl + 'users/register';
    const user = {
      UserName: userName,
      Password: password
    };

    return this.httpClient.post(registerUrl, user);
  }

  getAllUsers()
  {
    const url = environment.baseUrl + 'users/AllUsers';
    return this.httpClient.get(url);
  }
}
