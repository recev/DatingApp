import { Injectable } from '@angular/core';
import { HttpClient, HttpParams} from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { DetailedUser } from '../models/detailed-user';
import { PaginatedUserList } from '../models/paginated-user-list';
import { UserWithRole } from '../models/user-with-role';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private httpClient: HttpClient) { }

  getUsers(pageNumber: number, pageSize: number , minAge: number, maxAge: number, gender: string, orderBy: string)
  {
    const params = new HttpParams()
    .set('pageNumber', pageNumber.toString())
    .set('pageSize', pageSize.toString())
    .set('minAge', minAge.toString())
    .set('maxAge', maxAge.toString())
    .set('gender', gender)
    .set('orderBy', orderBy);

    return this.httpClient.get<PaginatedUserList>(environment.baseUrl + 'users/UserList', { params });
  }

  getAllUsersWithRoles(){
    return this.httpClient.get<UserWithRole[]>(environment.baseUrl + 'users/UserRoles');
  }

  updateUserRoles(userName: string, roles: string[]){
    console.log('roles', roles);
    return this.httpClient.post(environment.baseUrl + 'users/editRole/' + userName, {roles});
  }

  getUser(id: string)
  {
    return this.httpClient.get<DetailedUser>(environment.baseUrl + 'users/' + id);
  }

  updateUser(updateUser: DetailedUser)
  {
    return this.httpClient.put(environment.baseUrl + 'users/' + updateUser.id, updateUser);
  }
}
