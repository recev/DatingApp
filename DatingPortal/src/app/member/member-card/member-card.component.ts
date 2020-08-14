import { Component, OnInit, Input } from '@angular/core';
import { CompactUser } from 'src/app/models/compact-user';
import { faUser, faHeart, faEnvelope } from '@fortawesome/free-regular-svg-icons';

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

  constructor() { }

  ngOnInit(): void {
  }

}
