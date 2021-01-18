import { Component, EventEmitter, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { RoleState } from 'src/app/models/RoleState';
import { UserWithRole } from 'src/app/models/user-with-role';
import { UserService } from 'src/app/services/user.service';
import { RolesEditModalComponent } from '../roles-edit-modal/roles-edit-modal.component';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.scss']
})
export class UserManagementComponent implements OnInit {
  bsModalRef: BsModalRef;
  users: UserWithRole[];
  constructor(private userService: UserService, private toastr: ToastrService, private bsModalService: BsModalService) { }

  ngOnInit() {
    this.getUsers();
  }

  getUsers(){
    this.userService.getAllUsersWithRoles().subscribe(
      users => this.users = users,
    error => this.toastr.error(error)
    );
  }

  editRoles(user: UserWithRole){
    console.log(user);

    const availableRoles = this.getAvailableRoles();
    this.adjustAvailableRoles(user, availableRoles);

    const initialState = {
      user,
      availableRoles
    };
    this.bsModalRef = this.bsModalService.show(RolesEditModalComponent, {initialState});
    const updateSelectedRoutes = (this.bsModalRef.content.updateSelectedRoutes as EventEmitter<RoleState[]>);
    updateSelectedRoutes.subscribe((roleStates: RoleState[]) => {
      console.log('returned', roleStates);
      const roles = roleStates.map(v => v.name);

      this.userService.updateUserRoles(user.username, roles).subscribe(
        value => {
          user.roles = roles;
          this.toastr.success('Roles updated successfully!');
        },
        error => this.toastr.error(error));
    });

  }

  adjustAvailableRoles(user: UserWithRole, availableRoles: RoleState[]){
    availableRoles.forEach(role => {
      user.roles.forEach(userRole => {

        if (userRole === role.name){
          role.checked = true;
          return;
        }
      });
    });
  }

  getAvailableRoles(): RoleState[]
  {
    return [
      {
        name: 'Admin',
        checked: false
      },
      {
        name: 'Member',
        checked: false
      },
      {
        name: 'Moderator',
        checked: false
      },
      {
        name: 'VIP',
        checked: false
      }
    ];
  }

}
