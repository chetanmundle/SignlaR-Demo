import { Component } from '@angular/core';
import { SideConversationsComponent } from "./side-conversations/side-conversations.component";
import { ConversationMessageComponent } from "./conversation-message/conversation-message.component";
import { ConversationDto } from '../../../core/models/conversationModels/conversation.model';

@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [SideConversationsComponent, ConversationMessageComponent],
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.css'
})
export class ChatComponent {
  currentConversation!: ConversationDto;

  currentConversationEmmiter(conversation: ConversationDto) {
    this.currentConversation = conversation;
  }
}
