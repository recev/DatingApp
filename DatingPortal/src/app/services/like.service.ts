import { Injectable } from '@angular/core';
import { HttpClient, HttpParams} from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { CompactUser } from '../models/compact-user';
import { AuthorizationService } from './authorization.service';

@Injectable({
  providedIn: 'root'
})
export class LikeService {

  constructor(private authorizationService: AuthorizationService, private httpClient: HttpClient) { }

  likeUser(receiverId: string){
    const url = environment.baseUrl + 'likes/' + this.authorizationService.loggedInUserId() + '/' + receiverId;
    return this.httpClient.post<string>(url, {});
  }

  getLikeSendedUsers()
  {
    const url = environment.baseUrl + 'likes/' + this.authorizationService.loggedInUserId() + '/LikeSendedUsers';
    return this.httpClient.get<CompactUser[]>(url);
  }

  getLikeReceivedFromUsers()
  {
    const url = environment.baseUrl + 'likes/' + this.authorizationService.loggedInUserId() + '/LikeReceivedFromUsers';
    return this.httpClient.get<CompactUser[]>(url);
  }
}
