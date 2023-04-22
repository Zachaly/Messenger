import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http'
import { RouterModule, Routes } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';

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
import { ChatListPageComponent } from './pages/chat-list-page/chat-list-page.component';
import { DirectChatListItemComponent } from './components/direct-chat-list-item/direct-chat-list-item.component';
import { DirectChatComponent } from './components/direct-chat/direct-chat.component';
import { OnlineFriendsBarComponent } from './components/online-friends-bar/online-friends-bar.component';
import { UpdateProfilePageComponent } from './pages/update-profile-page/update-profile-page.component';
import { NavbarDropdownComponent } from './components/navbar-dropdown/navbar-dropdown.component';
import { NavbarDropdownItemComponent } from './components/navbar-dropdown-item/navbar-dropdown-item.component';
import { EmojiSelectComponent } from './components/emoji-select/emoji-select.component';
import { GroupChatListPageComponent } from './pages/group-chat-list-page/group-chat-list-page.component';
import { GroupChatComponent } from './components/group-chat/group-chat.component';
import { ChatMessageComponent } from './components/chat-message/chat-message.component';
import { ChatUserComponent } from './components/chat-user/chat-user.component';
import { AddChatComponent } from './components/add-chat/add-chat.component';
import { AddChatUserComponent } from './components/add-chat-user/add-chat-user.component';
import { GroupChatListItemComponent } from './components/group-chat-list-item/group-chat-list-item.component';
import { ChatUserListComponent } from './components/chat-user-list/chat-user-list.component';
import { UpdateChatComponent } from './components/update-chat/update-chat.component';
import { ChatMessageReactionComponent } from './components/chat-message-reaction/chat-message-reaction.component';

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
  route('direct-chat/:userId', DirectChatPageComponent),
  route('chats', ChatListPageComponent),
  route('update-profile', UpdateProfilePageComponent),
  route('group-chats', GroupChatListPageComponent)
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
    ChatListPageComponent,
    DirectChatListItemComponent,
    DirectChatComponent,
    OnlineFriendsBarComponent,
    UpdateProfilePageComponent,
    NavbarDropdownComponent,
    NavbarDropdownItemComponent,
    EmojiSelectComponent,
    GroupChatListPageComponent,
    GroupChatComponent,
    ChatMessageComponent,
    ChatUserComponent,
    AddChatComponent,
    AddChatUserComponent,
    GroupChatListItemComponent,
    ChatUserListComponent,
    UpdateChatComponent,
    ChatMessageReactionComponent,
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    RouterModule.forRoot(routes),
    FormsModule,
    FontAwesomeModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
