import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import { AuthGuard } from './_guard/auth.guard';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { memberDetailResolver } from './_resolvers/member-detail.resolver';
import { memberListResolver } from './_resolvers/member-list.resolver';

export const appRouts: Routes = [
    { path: '', component: HomeComponent },
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            { path: 'members', component: MemberListComponent, resolve: { users: memberListResolver } },
            { path: 'members/:id', component: MemberDetailComponent, resolve: { user: memberDetailResolver } },
            { path: 'messages', component: MessagesComponent },
            { path: 'lists', component: ListsComponent },
        ]
    },
    { path: '**', redirectTo: '', pathMatch: 'full' },
];
