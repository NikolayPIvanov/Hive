import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChatComponent } from './components/chat/chat.component';
import { LayoutModule } from '../layout/layout.module';
import { ChatRoutingModule } from './chat-routing.module';

@NgModule({
  declarations: [
    ChatComponent
  ],
  imports: [
    CommonModule,

    LayoutModule,

    ChatRoutingModule
  ]
})
export class ChatModule { }
