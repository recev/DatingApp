import { Component, OnInit } from '@angular/core';
import { AuthorizationService } from '../services/authorization.service';
import { faUser, faEdit } from '@fortawesome/free-regular-svg-icons';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.css']
})
export class NavigationComponent implements OnInit {
  faUser = faUser;
  faEdit = faEdit;
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

  public login(){

    console.log(this.user);
    this.authService.login(this.user.UserName, this.user.Password)
    .subscribe(v => {
      this.user.IsLoggedIn = true;
    },
    error => {
      this.user.IsLoggedIn = false;
      console.log(error);
    });
  }

  logout(){
    localStorage.removeItem('Token');
    this.user.IsLoggedIn = false;
  }

  public register(){
    this.authService.register(this.user.UserName, this.user.Password);
  }

}
