import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/services/user.service';
import { CompactUser } from 'src/app/models/compact-user';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {

  users: CompactUser[];
  constructor(private userService: UserService, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.userService.getUsers()
    .subscribe(users => this.users = users,
      error => this.toastr.error(error));
  }
}
