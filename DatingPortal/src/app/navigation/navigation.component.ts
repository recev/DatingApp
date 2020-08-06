import { Component, OnInit } from '@angular/core';
import { AuthorizationService } from '../services/authorization.service';
import { faUser, faEdit } from '@fortawesome/free-regular-svg-icons';
import { ToastrService } from 'ngx-toastr';

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
    Password: string
  } = {
    UserName: '',
    Password : ''
  };

  constructor(
    public authService: AuthorizationService,
    private toastr: ToastrService) { }

  ngOnInit(): void {}

  public isLoggedIn()
  {
      return this.authService.isLoggedIn();
  }

  public loggedInUser()
  {
    return this.authService.LoggedInUser();
  }

  public login(){

    console.log(this.user);
    this.authService.login(this.user.UserName, this.user.Password)
    .subscribe(v => {
      this.toastr.success('Logged in successfuly!');
    },
    error => {
      this.toastr.error(error);
    });
  }

  logout(){
    this.authService.logout();
  }
}
