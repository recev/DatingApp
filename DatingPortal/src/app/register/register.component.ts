import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { AuthorizationService } from '../services/authorization.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() RegisterationCancelled = new EventEmitter<boolean>();
  user: {
    UserName: string,
    Password: string,
    IsLoggedIn: boolean
  } = {
    UserName: '',
    Password : '',
    IsLoggedIn: false
  };

  constructor(
    private authService: AuthorizationService,
    private toastr: ToastrService) { }

  ngOnInit(): void {
  }

  register(){
    this.authService.register(this.user.UserName, this.user.Password)
    .subscribe(value => {
      this.toastr.success('User registred successfuly');
    }, error => {
      this.toastr.error(error);
    });
  }

  cancel()
  {
    this.RegisterationCancelled.emit(true);
  }

}
