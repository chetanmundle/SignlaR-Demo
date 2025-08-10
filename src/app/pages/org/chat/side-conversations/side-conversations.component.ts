import { CommonModule } from '@angular/common';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CreateConversationComponent } from "../../create-conversation/create-conversation.component";
import Modal from 'bootstrap/js/dist/modal';
import { ConversationService } from '../../../../core/services/conversationService/conversation.service';
import { UserService } from '../../../../core/services/userService/user.service';
import { LoggedUserDto } from '../../../../core/models/userModels/UserDtos';
import { ToastrService } from 'ngx-toastr';
import { HttpErrorResponse } from '@angular/common/http';
import { ConversationDto } from '../../../../core/models/conversationModels/conversation.model';

@Component({
  selector: 'app-side-conversations',
  standalone: true,
  imports: [CommonModule, CreateConversationComponent],
  templateUrl: './side-conversations.component.html',
  styleUrl: './side-conversations.component.css'
})
export class SideConversationsComponent implements OnInit {
  @Output() currentConversationEmmiter = new EventEmitter<ConversationDto>();   // for sending data to parent
  currentConversation? : ConversationDto;
  loggedUser? : LoggedUserDto;
  showCreateConversation = false;

  myConversations : ConversationDto[]  = []

  constructor(
    private _userService : UserService,
    private _conversationService : ConversationService,
    private _toasR : ToastrService,
  ){

  }
  ngOnInit(): void {
    this._userService.loggedUser$.subscribe({
      next : (res) =>{
        this.loggedUser = res;
        this.GetMyConversation(res.userId);
      }
    })
  }
  
  openModal() {
    this.showCreateConversation = true;
    const modalElement = document.getElementById('createConversationModal');
    if (modalElement) {
      const modal = new Modal(modalElement);
      modal.show();
    }
  }

  GetMyConversation(userId : number){
    this._conversationService.GetMyConversation$(userId).subscribe({
      next : (res)=>{
        if(res.isSuccess){
          this.myConversations = res.data;
        }else{
          this._toasR.error(res.message);
        }
      },
      error : (err : HttpErrorResponse) =>{
        this._toasR.error(err.error.message);
      }
    })
  }

  sendCurrentConversationData(conversation : ConversationDto){
    this.currentConversation = conversation;
    this.currentConversationEmmiter.emit(conversation);    
  }
}
