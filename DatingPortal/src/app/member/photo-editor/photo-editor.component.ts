import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import {FileUploader} from 'ng2-file-upload';
import { Photo } from 'src/app/models/photo';
import { environment } from 'src/environments/environment';
import { AuthorizationService } from 'src/app/services/authorization.service';
import { faTrashAlt, faFile} from '@fortawesome/free-regular-svg-icons';
import { faUpload, faStop} from '@fortawesome/free-solid-svg-icons';
import { PhotoService } from 'src/app/services/photo.service';
import { ToastrService } from 'ngx-toastr';

@Component({
    selector: 'app-photo-editor',
    templateUrl: './photo-editor.component.html',
    styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
    @Input() photos: Photo[] = [];
    // Output is used just for demostration perpuse
    @Output() mainPhotoChanged = new EventEmitter<string>();
    public uploader: FileUploader;
    public hasBaseDropZoneOver = false;
    public faTrashAlt = faTrashAlt;
    public faFile = faFile;
    public faUpload = faUpload;
    public faStop = faStop;

    constructor(
      private authorizationService: AuthorizationService,
      private photoService: PhotoService,
      private toastr: ToastrService) { }

    ngOnInit(): void {
      this.initializeUploader();
    }

    fileOverBase(e: any): void {
      this.hasBaseDropZoneOver = e;
    }

    initializeUploader(){
      this.uploader = new FileUploader(
        {
          url: environment.baseUrl + 'users/' + this.authorizationService.loggedInUserId() + '/photos',
          authToken: 'Bearer ' + this.authorizationService.getToken(),
          isHTML5: true,
          maxFileSize: 3 * 1023 * 1024,
          autoUpload: false,
          removeAfterUpload: true,
          allowedFileType: ['image']
        });

      this.uploader.onAfterAddingFile = (file) => file.withCredentials = false;
      this.uploader.onSuccessItem = (item, response, status, Headers) => {
        const photo = JSON.parse(response) as Photo;
        this.photos.push(photo);
        this.authorizationService.addNewPhoto(photo);
      };
    }

    setMain(newMainPhoto: Photo){

      this.photoService.setMainPhoto(newMainPhoto).subscribe(
        v => {
          this.authorizationService.updateUserMainPhoto(newMainPhoto);

          const currentMainPhoto = this.photos.filter(p => p.isMain === true)[0];
          currentMainPhoto.isMain = false;
          newMainPhoto.isMain = true;
        },
        e => this.toastr.error(e)
      );
    }

    deletePhoto(photo: Photo) {

    const deleteIt = confirm('Do you want to delete the Photo?');

    if (deleteIt === true) {
      this.photoService.deletePhoto(photo)
        .subscribe(
          v => {
            const photoIndex = this.photos.findIndex(p => p.id === photo.id);
            this.photos.splice(photoIndex, 1);
            this.authorizationService.deletePhoto(photo);
            this.toastr.success('Image deleted successfuly!');
          },
          e => this.toastr.error(e)
        );
    }
  }
}
