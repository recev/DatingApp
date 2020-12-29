import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/services/user.service';
import { CompactUser } from 'src/app/models/compact-user';
import { ToastrService } from 'ngx-toastr';
import { PaginatedUserList } from 'src/app/models/paginated-user-list';
import { PaginationParams } from 'src/app/models/pagination-params';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {

  pageNumber = 1;
  pageSize = 10;
  minAge = 18;
  maxAge = 38;
  gender = 'male';
  orderBy = 'created';
  genders = [
    {value: 'all', display: 'All'},
    {value: 'male', display: 'Males'},
    {value: 'female', display: 'Females'}
  ];
  totalUserCount: number;
  totalPageCount: number;

  users: CompactUser[];
  constructor(private userService: UserService, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(){
    this.userService.getUsers(this.pageNumber, this.pageSize, this.minAge, this.maxAge, this.gender, this.orderBy)
    .subscribe(paginatedUserList =>
      {
        console.log(paginatedUserList);
        console.log(paginatedUserList.users);
        this.totalUserCount = paginatedUserList.totalUserCount;
        this.totalPageCount = paginatedUserList.totalPageCount;
        this.users = paginatedUserList.users;
      },
      error => this.toastr.error(error)
    );
  }

  pageChanged(params: PaginationParams){
    this.pageNumber = params.page;
    this.pageSize = params.itemsPerPage;
    this.loadUsers();
  }

  resetFilters(){
    console.log('reset');
    this.pageNumber = 1;
    this.pageSize = 10;
    this.minAge = 1;
    this.maxAge = 99;
    this.gender = 'male';
  }

}
