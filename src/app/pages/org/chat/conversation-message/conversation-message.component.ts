import { CommonModule } from '@angular/common';
import { Component, Input, OnChanges, OnDestroy, OnInit, SimpleChanges } from '@angular/core';
import { ConversationDto } from '../../../../core/models/conversationModels/conversation.model';
import { SignalrService } from '../../../../core/services/signalRService/signalr-service.service';
import { UserService } from '../../../../core/services/userService/user.service';
import { LoggedUserDto } from '../../../../core/models/userModels/UserDtos';
import { FormsModule } from '@angular/forms';
import { MessageService } from '../../../../core/services/messageService/message.service';
import { MessageDto } from '../../../../core/models/messageDtos/messageDtos.model';

@Component({
  selector: 'app-conversation-message',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './conversation-message.component.html',
  styleUrl: './conversation-message.component.css'
})
export class ConversationMessageComponent implements OnInit, OnChanges, OnDestroy {
  @Input() currentConversation!: ConversationDto;
  loggedUser?: LoggedUserDto;
  messages : MessageDto[] = [];

  message: string = ""

  constructor(
    private _signalRService: SignalrService,
    private _userService: UserService,
    private _messageService :MessageService,
  ) {

  }

  ngOnInit(): void {
    this._userService.loggedUser$.subscribe({
      next: (res) => {
        this.loggedUser = res;
      }
    })

    // Listen for messages from the group
    this._signalRService.messages$.subscribe({
      next: (msg) => {
        if (msg) {
          console.log("Message Recive : ", msg);
          this.messages.push({
            messageContent: msg.message,
            conversationId : msg.conversationId,
            messageId : 0,  // static due to not generated yet
            userId : msg.userId,
            isMe: this.loggedUser?.userId == msg.userId ? true : false
          });
        }
      }
    });

  }

  async ngOnChanges(changes: SimpleChanges) {
    if (changes['currentConversation'] && this.currentConversation) {
      this.messages = [];
      const convId = this.currentConversation.conversationId; // Adjust if your DTO uses a different property name

      // Leave the previous group if there was one
      const prevConv: ConversationDto = changes['currentConversation'].previousValue;
      if (prevConv && prevConv.conversationId !== convId) {
        await this._signalRService.leaveConversation(prevConv.conversationId);
      }
      
      // get the previous messages
      this.GetMessagesByConversationId(convId);
      
      // Join the new conversation group
      await this._signalRService.joinConversation(convId);
    }
  }

  // send message to conversation group
  sendMessage() {
    if (!this.message.trim() || !this.currentConversation || !this.loggedUser) return;
    this._signalRService.sendMessageToConversation(this.currentConversation.conversationId, this.message, this.loggedUser.userId);
    this.message = '';
  }

  GetMessagesByConversationId(conversationId : number){
    this._messageService.GetMessagesByConversationId$(conversationId).subscribe({
      next : (res)=>{
        if(res.isSuccess){
          this.messages = res.data.map((msg) =>({
            ...msg,
            isMe : this.loggedUser?.userId == msg.userId ? true : false,
          }))
        }
      }
    })
  }

  async ngOnDestroy(): Promise<void> {
    if (this.currentConversation) {
      await this._signalRService.leaveConversation(this.currentConversation.conversationId);
    }

  }
}
