import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { AuthorizationService } from '../services/authorization.service';

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

  constructor(private authService: AuthorizationService) { }

  ngOnInit(): void {
  }

  register(){
    this.authService.register(this.user.UserName, this.user.Password)
    .subscribe(value => {
      console.log(value);
    }, error => {
      console.log(error);
    });
  }

  cancel()
  {
    this.RegisterationCancelled.emit(true);
  }

}
