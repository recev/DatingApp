import { Component, OnInit } from '@angular/core';
import { DetailedUser } from 'src/app/models/detailed-user';
import { UserService } from 'src/app/services/user.service';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from 'ngx-gallery-9';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {

  user: DetailedUser = new DetailedUser();

  galleryOptions: NgxGalleryOptions[] = [
        {
            width: '500px',
            height: '500px',
            imagePercent: 100,
            thumbnailsColumns: 4,
            imageAnimation: NgxGalleryAnimation.Slide,
            preview: false
        }
    ];

  galleryImages: NgxGalleryImage[] = [];

  constructor(private userService: UserService, private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {

      const id = this.activatedRoute.snapshot.paramMap.get('id');
      this.userService.getUser(id).subscribe(user => {

        this.user = user;

        const image = new NgxGalleryImage({
          small: this.user.photoUrl,
          medium: this.user.photoUrl,
          big: this.user.photoUrl
        });

        this.galleryImages.push(image);
      });
  }
}
