import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Photo } from '../models/photo';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PhotoService {

  constructor(private httpClient: HttpClient) { }

  setMainPhoto(photo: Photo)
  {
    console.log(photo);

    const url = environment.baseUrl + 'users/' + photo.userId + '/photos/' + photo.id + '/setmain';
    console.log(url);
    return this.httpClient.post(url, {});
  }

  deletePhoto(photo: Photo)
  {
    console.log(photo);

    const url = environment.baseUrl + 'users/' + photo.userId + '/photos/' + photo.id;
    console.log(url);
    return this.httpClient.delete(url);
  }
}
