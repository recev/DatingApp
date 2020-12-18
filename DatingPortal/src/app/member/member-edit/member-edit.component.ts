import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { DetailedUser } from 'src/app/models/detailed-user';
import { AuthorizationService } from 'src/app/services/authorization.service';
import { UserService } from 'src/app/services/user.service';
import { ToastrService } from 'ngx-toastr';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  user: DetailedUser = new DetailedUser();
  photoUrl = '';
  @ViewChild('userForm') userForm: NgForm;
  @HostListener('window:beforeunload', ['$event'])
  WindowBeforeUnload($event: any){

    if ( this.userForm.dirty)
    {
      $event.returnValue = 'Your data will be lost!';
    }
  }

  constructor(
    private authService: AuthorizationService,
    private userService: UserService,
    private toastr: ToastrService) { }

  ngOnInit(): void {
    this.getUser();
    this.authService.photoUrl.subscribe(p => this.photoUrl = p);
  }


  private getUser() {
    this.userService
      .getUser(this.authService.loggedInUserId())
      .subscribe(
        user => this.user = user,
        errorMessage => this.toastr.error(errorMessage)
      );
  }

  saveChanges(){
    console.log(this.user);
    this.userService.updateUser(this.user)
    .subscribe(
        v => {
        this.toastr.success('Saved Successfuly!');
        this.userForm.reset(this.user);
      },
      e => this.toastr.error(e)
    );
  }

  mainPhotoChanged(newUrl){
    console.log('new main url ' + newUrl);
  }
}
