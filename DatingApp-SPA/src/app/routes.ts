import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { ListsComponent } from './lists/lists.component';
import { AuthGuard } from './Guards/Auth.guard';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberDetailResolver } from './Resolver/member-detail.resolver';
import { MemberListResolver } from './Resolver/member-list.resolver';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberEditResolver } from './Resolver/member-edit.resolver';
import { PreventUnSavedGaurd } from './Guards/prevent-unsaved.gaurd';

export const appRoutes: Routes = [
    {path: 'home', component: HomeComponent },
    {path: '', component: HomeComponent },
    {
        path: '',
        canActivate: [AuthGuard],
        runGuardsAndResolvers: 'always',
        children: [

                    {path: 'members', component: MemberListComponent, resolve: {users: MemberListResolver} },
                    {path: 'messages', component: MessagesComponent },
                    {path: 'members/edit', component: MemberEditComponent,
                    resolve: {currentUser: MemberEditResolver},
                canDeactivate: [PreventUnSavedGaurd] },

                    {path: 'members/:id', component: MemberDetailComponent, resolve: {user: MemberDetailResolver} },
                    {path: 'lists', component: ListsComponent }
                ]
    },
    {path: '**', redirectTo: '', pathMatch: 'full' }
];
