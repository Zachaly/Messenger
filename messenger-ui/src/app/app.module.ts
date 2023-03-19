import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http'
import { RouterModule, Routes } from '@angular/router';

import { AppComponent } from './app.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { RegisterPageComponent } from './pages/register-page/register-page.component';
import { LoginPageComponent } from './pages/login-page/login-page.component';
import { MainPageComponent } from './pages/main-page/main-page.component';
import { FormsModule } from '@angular/forms';
import { FriendListPageComponent } from './pages/friend-list-page/friend-list-page.component';
import { UserComponent } from './components/user/user.component';
import { FriendRequestComponent } from './components/friend-request/friend-request.component';
import { TabComponent } from './components/tab/tab.component';
import { FriendRequestListPageComponent } from './pages/friend-request-list-page/friend-request-list-page.component';
import { FriendComponent } from './components/friend/friend.component';
import { NotificationListComponent } from './components/notification-list/notification-list.component';
import { NotificationComponent } from './components/notification/notification.component';
import { DirectChatPageComponent } from './pages/direct-chat-page/direct-chat-page.component';
import { DirectMessageComponent } from './components/direct-message/direct-message.component';

const route = (path: string, component: any) => (
  {
    path,
    component
  }
)

const routes: Routes = [
  route('', MainPageComponent),
  route('login', LoginPageComponent),
  route('register', RegisterPageComponent),
  route('friend-request', FriendRequestListPageComponent),
  route('friend', FriendListPageComponent),
  route('direct-chat/:userId', DirectChatPageComponent)
]

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    RegisterPageComponent,
    LoginPageComponent,
    MainPageComponent,
    FriendListPageComponent,
    UserComponent,
    FriendRequestComponent,
    TabComponent,
    FriendRequestListPageComponent,
    FriendComponent,
    NotificationListComponent,
    NotificationComponent,
    DirectChatPageComponent,
    DirectMessageComponent,
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    RouterModule.forRoot(routes),
    FormsModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
