import { Component, OnInit } from '@angular/core';
import { AuthorizationService } from './services/authorization.service';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
    title = 'DatingPortal';
    constructor(private authorizationService: AuthorizationService) { }

    ngOnInit() {
        this.setCurrentMainPhoto();
    }

  private setCurrentMainPhoto() {
    const user = this.authorizationService.getUser();

    if (user) {
      this.authorizationService.changeUserPhoto(this.authorizationService.getUserMainPhoto());
    }
  }
}
