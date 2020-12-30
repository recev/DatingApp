import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { CompactUser } from '../models/compact-user';
import { LikeService } from '../services/like.service';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css']
})
export class ListComponent implements OnInit {
  showSendedLikes = false;
  users: CompactUser[];

  constructor(private userService: UserService, private likeService: LikeService , private toastr: ToastrService) { }

  ngOnInit(): void {
    this.loadUser();
  }

  loadUser(){
    if (this.showSendedLikes) {
      this.loadSended();
    }
    else {
      this.loadReceived();
    }
  }

  loadSended(){
    this.likeService.getLikeSendedUsers()
      .subscribe(
        v => this.users = v,
        e => this.toastr.error(e)
      );
  }

  loadReceived(){
    this.likeService.getLikeReceivedFromUsers()
      .subscribe(
        v => this.users = v,
        e => this.toastr.error(e)
      );
  }

}
