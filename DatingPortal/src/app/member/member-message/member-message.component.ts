import { Component, Input, OnChanges, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { tap } from 'rxjs/operators';
import { Message } from 'src/app/models/message';
import { MessageService } from 'src/app/services/message.service';

@Component({
  selector: 'app-member-message',
  templateUrl: './member-message.component.html',
  styleUrls: ['./member-message.component.css']
})
export class MemberMessageComponent implements OnInit, OnChanges {
  messages: Message[];
  messageContent: string;
  @Input() recipient = 0;

  constructor(private messageService: MessageService, private toastr: ToastrService) { }

  ngOnInit() {

  }

  ngOnChanges(){
    this.loadMessages();
  }

  loadMessages(){
    this.messageService.getMessageThread(this.recipient)
      .pipe(
        tap(messages => this.markMessagesAsRead(messages))
      )
      .subscribe(
        messages => {
          this.messages = messages;
          console.log(messages);
        },
        error => this.toastr.error(error)
      );
  }

  sendMessage(){

    this.messageService.sendMessage(this.recipient, this.messageContent).subscribe((value: Message) => {
      this.messages.push(value);
    },
    error => this.toastr.error(error)
    );
  }

  markMessagesAsRead(messages: Message[]){
    messages.forEach(message => {

      if ( message.isRead === false && message.senderId === this.recipient )
      {
        this.messageService.markAsRead(message.id).subscribe(
          v => {
            message.isRead = true;
            this.toastr.success('message marked as read!');
          },
          e => this.toastr.error('message can not be set as read! ' + e)
        );
      }
    });
  }

}
