import { Component, OnInit, Input } from '@angular/core';
import { CompactUser } from 'src/app/models/compact-user';
import { faUser, faHeart, faEnvelope } from '@fortawesome/free-regular-svg-icons';
import { ToastrService } from 'ngx-toastr';
import { LikeService } from 'src/app/services/like.service';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {
  @Input() user: CompactUser;
  faUser = faUser;
  faHeart = faHeart;
  faEnvelope = faEnvelope;

  constructor(private likeService: LikeService, private toastr: ToastrService) { }

  ngOnInit(): void {
  }

  likeUser(receiverId: number){
    this.likeService.likeUser(receiverId)
      .subscribe(
        v => this.toastr.success('User liked successfully!', v),
        e => this.toastr.error('user could not like!', e)
    );
  }
}
