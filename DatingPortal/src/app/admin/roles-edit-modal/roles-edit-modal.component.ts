import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { RoleState } from 'src/app/models/RoleState';
import { UserWithRole } from 'src/app/models/user-with-role';

@Component({
  selector: 'app-roles-edit-modal',
  templateUrl: './roles-edit-modal.component.html',
  styleUrls: ['./roles-edit-modal.component.css']
})
export class RolesEditModalComponent implements OnInit {

  @Output() updateSelectedRoutes = new EventEmitter<RoleState[]>();

  rolesForm: FormGroup;
  user: UserWithRole;
  availableRoles: RoleState[];

  constructor(private bsModalRef: BsModalRef, private formBuilder: FormBuilder) {}

  ngOnInit() {
    this.createForm();
  }

  createForm(){
    this.rolesForm = this.formBuilder.group({
      roles: this.getRolesFormArray()
    });
  }

  getRolesFormArray(): FormArray{
    const array = this.availableRoles.map(roleState => {
        return this.formBuilder.control({
          value: roleState.checked,
          disabled: this.isAdmin(roleState)
      });
    });
    return this.formBuilder.array(array);
  }

  get rolesFormArray() {
    return this.rolesForm.get('roles') as FormArray;
  }

  isAdmin(roleState: RoleState){
    return roleState.name === 'Admin' && this.user.username === 'Admin' ? true : false;
  }

  saveRoles(){

    const selectedRoles = this.findSelectedRoles();

    this.updateSelectedRoutes.emit(selectedRoles);
    this.closeModal();
  }

  closeModal(){
    this.bsModalRef.hide();
  }

  findSelectedRoles(){
    const roleSelections = this.rolesForm.value.roles as boolean[];

    const mappedRoles: RoleState[] = roleSelections.map((element, index) => {
      return {name: this.availableRoles[index].name, checked: element};
    });

    const selectedRoles = mappedRoles.filter(role => role.checked);

    return selectedRoles;
  }

}
