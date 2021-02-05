import { Component, OnInit, ViewChild } from '@angular/core';
import { DetailedUser } from 'src/app/models/detailed-user';
import { UserService } from 'src/app/services/user.service';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from 'ngx-gallery-9';
import { TabsetComponent } from 'ngx-bootstrap/tabs';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {

  user: DetailedUser = new DetailedUser();

  @ViewChild('detailTabs', { static: true }) detailTabs: TabsetComponent;

  galleryOptions: NgxGalleryOptions[] = [
        {
            width: '500px',
            height: '500px',
            imagePercent: 100,
            thumbnailsColumns: 4,
            imageAnimation: NgxGalleryAnimation.Slide,
            preview: false,
            imageArrows: true,
            thumbnailsArrows: true
        }
    ];

  galleryImages: NgxGalleryImage[] = [];

  constructor(private userService: UserService, private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.loadUser();
    this.selectMessagesTab();
  }

  loadUser(){
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    this.userService.getUser(id).subscribe(user => {

      this.user = user;

      user.photos.forEach(photo => {
        const image = new NgxGalleryImage({
          small: photo.url,
          medium: photo.url,
          big: photo.url
        });

        this.galleryImages.push(image);
      });
    });
  }

  selectMessagesTab(){
    this.activatedRoute.queryParamMap.subscribe(params => {
      const tabId = params.get('tabs');
      if (tabId){
        this.selectTab( parseInt(tabId, 10) );
      }
    });
  }

  selectTab(tabId: number) {
    this.detailTabs.tabs[tabId].active = true;
  }
}
