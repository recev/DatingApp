import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { DetailedUser } from '../models/detailed-user';
import { BehaviorSubject } from 'rxjs';
import { Photo } from '../models/photo';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class AuthorizationService {
  public photoUrl: BehaviorSubject<string> = new BehaviorSubject<string>('../../assets/user.png');

  constructor(
    private httpClient: HttpClient,
    private jwtHelper: JwtHelperService) { }

  public changeUserPhoto(photoUrl: string)
  {
    this.photoUrl.next(photoUrl);
  }

  public getUserMainPhoto(): string
  {
    const user = JSON.parse(localStorage.getItem('user')) as DetailedUser;
    const currentMainPhoto = user.photos.filter(p => p.isMain === true)[0];
    return currentMainPhoto.url;
  }

  public addNewPhoto(photo: Photo)
  {
    const user = JSON.parse(localStorage.getItem('user')) as DetailedUser;
    user.photos.push(photo);
    localStorage.setItem('user', JSON.stringify(user));
    this.changeUserPhoto(photo.url);
  }

  public deletePhoto(photo: Photo)
  {
    const user = JSON.parse(localStorage.getItem('user')) as DetailedUser;
    const deletePhoto = user.photos.findIndex(p => p.id === photo.id);
    user.photos.splice(deletePhoto, 1);
    localStorage.setItem('user', JSON.stringify(user));
    this.changeUserPhoto(photo.url);
  }

  public isLoggedIn() {
    return !this.jwtHelper.isTokenExpired();
  }

  getUser()
  {
    const user = JSON.parse(localStorage.getItem('user')) as DetailedUser;
    return user;
  }

  isUserAuthorized(allowedRoles: Array<string>): boolean{

    let isUserAuthorized = false;

    if (!allowedRoles){
      return isUserAuthorized;
    }

    const userRoles = this.getUserRoles();

    allowedRoles.forEach(allowedRole => {
      if (userRoles.includes(allowedRole)){
        isUserAuthorized = true;
        return;
      }
    });

    return isUserAuthorized;
  }

  getUserPhotos()
  {
    const user = JSON.parse(localStorage.getItem('user')) as DetailedUser;
    return user.photos;
  }

  getUserRoles()
  {
    const user = JSON.parse(localStorage.getItem('user')) as DetailedUser;
    return user.roles;
  }

  updateUserMainPhoto(photo: Photo)
  {
    const user = JSON.parse(localStorage.getItem('user')) as DetailedUser;
    const newMainPhoto: Photo = user.photos.filter(p => p.id === photo.id)[0];

    const currentMainPhoto = user.photos.filter(p => p.isMain === true)[0];

    currentMainPhoto.isMain = false;
    newMainPhoto.isMain = true;

    localStorage.setItem('user', JSON.stringify(user));

    this.changeUserPhoto(photo.url);
  }

  getToken()
  {
    return localStorage.getItem('access_token');
  }

  loggedInUserId()
  {
    const token = this.jwtHelper.decodeToken();
    return token.nameid;
  }

  public LoggedInUser() {
    const token = this.jwtHelper.decodeToken();
    return token.unique_name;
  }

  login(userName: string, password: string) {
    const loginUrl = environment.baseUrl + 'account/login';
    const user = {
      UserName: userName,
      Password: password
    };

    return this.httpClient.post(loginUrl, user)
      .pipe(
        map((value: any) => {
          console.log(value);
          if (!!value) {
            localStorage.setItem('access_token', value.token);
            localStorage.setItem('user', JSON.stringify(value.user));
            this.changeUserPhoto(value.user.photoUrl);
          }
        })
      );
  }

  logout() {
    localStorage.removeItem('access_token');
    localStorage.removeItem('user');
  }

  register(user: User) {
    const registerUrl = environment.baseUrl + 'account/register';
    return this.httpClient.post(registerUrl, user);
  }
}
