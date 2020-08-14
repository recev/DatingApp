import { Injectable } from '@angular/core';
import { HttpClient} from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { CompactUser } from '../models/compact-user';
import { DetailedUser } from '../models/detailed-user';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private httpClient: HttpClient) { }

  getUsers()
  {
    return this.httpClient.get<CompactUser[]>(environment.baseUrl + 'users/UserList');
  }

  getUser(id: string)
  {
    console.log(id);
    return this.httpClient.get<DetailedUser>(environment.baseUrl + 'users/' + id);
  }
}
