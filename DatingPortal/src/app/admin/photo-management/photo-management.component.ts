import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { PhotoForUser } from 'src/app/models/photoForUser';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-photo-management',
  templateUrl: './photo-management.component.html',
  styleUrls: ['./photo-management.component.scss']
})
export class PhotoManagementComponent implements OnInit {

  userPhotos: PhotoForUser[] = [];

  constructor(private userService: UserService, private toastr: ToastrService) { }

  ngOnInit() {
    this.getUnapprovedUserPhotos();
  }

  getUnapprovedUserPhotos(){
    this.userService.getUnapprovedUserPhotos().subscribe(
      v => this.userPhotos = v,
      e => this.toastr.error(e)
    );
  }

  approveUserPhoto(photo: PhotoForUser){
    console.log(photo);
    this.userService.approveUserPhoto(photo)
    .subscribe(
      v => {
        const photoIndex = this.userPhotos.findIndex(p => p.photoId === photo.photoId);
        this.userPhotos.splice(photoIndex, 1);

        this.toastr.success('Photo approved');
      },
      e => this.toastr.error(e)
      );
  }

  rejectUserPhoto(photo: PhotoForUser){
    console.log(photo);
    this.userService.rejectUserPhoto(photo)
    .subscribe(
      v => {
        const photoIndex = this.userPhotos.findIndex(p => p.photoId === photo.photoId);
        this.userPhotos.splice(photoIndex, 1);

        this.toastr.success('Photo rejected!');
      },
      e => this.toastr.error(e)
      );
  }

}
