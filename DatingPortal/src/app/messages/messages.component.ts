import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Message } from '../models/message';
import { MessageService } from '../services/message.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {
  messages: Message[] = [];
  selectedMessageBox = 'unread';
  constructor(private messageService: MessageService, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.LoadUnreadMessages();
  }

  LoadInboxMessages(){
    this.messageService.getInboxMessages()
      .subscribe(
        messages => {
          this.messages = messages;
          this.selectedMessageBox = 'inbox';
          console.log(messages);
        },
        error => this.toastr.error(error)
      );
  }

  LoadOutboxMessages(){
    this.messageService.getOutboxMessages()
      .subscribe(
        messages => {
          this.messages = messages;
          this.selectedMessageBox = 'outbox';
          console.log(messages);
        },
        error => this.toastr.error(error)
      );
  }

  LoadUnreadMessages(){
    this.messageService.getUnreadMessages()
      .subscribe(
        messages => {
          this.messages = messages;
          this.selectedMessageBox = 'unread';
          console.log(messages);
        },
        error => this.toastr.error(error)
      );
  }

  deleteMessage(messageId: number){
    this.messageService.deleteMessage(messageId).subscribe(v => {
      console.log(messageId);
      const deletedMessageIndex = this.messages.findIndex(m => m.id === messageId);
      this.messages.splice(deletedMessageIndex, 1);
      this.toastr.success('deleted successfully!');
    },
    error => this.toastr.error(error));
  }

}
