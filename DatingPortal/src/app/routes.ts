import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MessagesComponent } from './messages/messages.component';
import { ListComponent } from './list/list.component';
import { MemberListComponent } from './member/member-list/member-list.component';
import { MemberDetailComponent } from './member/member-detail/member-detail.component';
import { AuthorizationGuard } from './guards/authorization.guard';
import { NotFoundComponent } from './not-found/not-found.component';

export let appRoutes: Routes = [
  { path : '', component : NotFoundComponent},
  { path : 'home', component: HomeComponent },
  {
    path: '',
    canActivate: [AuthorizationGuard],
    runGuardsAndResolvers: 'always',
    children: [
      { path : 'members', component: MemberListComponent},
      { path : 'members/:id', component: MemberDetailComponent },
      { path : 'messages', component: MessagesComponent},
      { path : 'list', component : ListComponent},
      { path : '**', component : NotFoundComponent}
    ]
  }
];
