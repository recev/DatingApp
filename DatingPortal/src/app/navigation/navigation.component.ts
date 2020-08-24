import { Component, OnInit } from '@angular/core';
import { AuthorizationService } from '../services/authorization.service';
import { faUser, faEdit } from '@fortawesome/free-regular-svg-icons';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { DetailedUser } from '../models/detailed-user';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.css']
})
export class NavigationComponent implements OnInit {
  faUser = faUser;
  faEdit = faEdit;
  user: DetailedUser = new DetailedUser();
  photoUrl = '';

  constructor(
    public authService: AuthorizationService,
    private toastr: ToastrService,
    private router: Router) { }

  ngOnInit(): void {
    this.authService.photoUrl.subscribe(photoUrl => this.photoUrl = photoUrl);
  }

  public isLoggedIn()
  {
      return this.authService.isLoggedIn();
  }

  public loggedInUser()
  {
    return this.authService.getUser().username;
  }

  public login(){

    console.log(this.user);
    this.authService.login(this.user.username, this.user.password)
    .subscribe(v => {
      this.toastr.success('Logged in successfuly!');
      this.router.navigate(['/members']);
      this.user = this.authService.getUser();
    },
    error => {
      this.toastr.error(error);
    });
  }

  logout(){
    this.authService.logout();
    this.router.navigate(['/home']);
  }
}
