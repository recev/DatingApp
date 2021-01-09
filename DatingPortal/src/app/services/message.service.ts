import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Message } from '../models/message';
import { MessageForCreation } from '../models/message-for-creation';
import { AuthorizationService } from './authorization.service';

@Injectable({
  providedIn: 'root'
})
export class MessageService {

constructor(private httpClient: HttpClient, private authorizationService: AuthorizationService) { }

  getInboxMessages(){
    const url = environment.baseUrl + 'users/' + this.authorizationService.loggedInUserId() + '/messages/inbox';
    return this.httpClient.get<Message[]>(url);
  }

  getOutboxMessages(){
    const url = environment.baseUrl + 'users/' + this.authorizationService.loggedInUserId() + '/messages/outbox';
    return this.httpClient.get<Message[]>(url);
  }

  getUnreadMessages(){
    const url = environment.baseUrl + 'users/' + this.authorizationService.loggedInUserId() + '/messages/unread';
    return this.httpClient.get<Message[]>(url);
  }

  getMessageThread(recipientId: number)
  {
    const url = environment.baseUrl + 'users/' + this.authorizationService.loggedInUserId() + '/messages/thread/' + recipientId;
    return this.httpClient.get<Message[]>(url);
  }

  sendMessage(recipientId: number, content: string){
    const url = environment.baseUrl + 'users/' + this.authorizationService.loggedInUserId() + '/messages';

    const messageForCreation: MessageForCreation = {
      SenderId : this.authorizationService.loggedInUserId(),
      RecipientId: recipientId,
      Content : content
    };
    return this.httpClient.post(url, messageForCreation);
  }

  deleteMessage(messageId: number){
    const url = environment.baseUrl + 'users/' + this.authorizationService.loggedInUserId() + '/messages/' + messageId;
    return this.httpClient.delete(url);
  }

  markAsRead(messageId: number){
    const url = environment.baseUrl + 'users/' + this.authorizationService.loggedInUserId() + '/messages/' + messageId + '/read';
    return this.httpClient.post(url, {});
  }
}
