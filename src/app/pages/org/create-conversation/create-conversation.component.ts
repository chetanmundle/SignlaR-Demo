import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { UserService } from '../../../core/services/userService/user.service';
import { LoggedUserDto, UserDto } from '../../../core/models/userModels/UserDtos';
import { ToastrService } from 'ngx-toastr';
import { ConversationService } from '../../../core/services/conversationService/conversation.service';
import { CreateConversationRequest } from '../../../core/models/conversationModels/conversation.model';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-create-conversation',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './create-conversation.component.html',
  styleUrl: './create-conversation.component.css'
})
export class CreateConversationComponent implements OnInit {
  loggedUser?: LoggedUserDto;
  inputValue: string = '';
  selectedUsers: UserDto[] = [];
  conversationName: string = '';

  allUsers: UserDto[] = []

  filterdUsers: UserDto[] = []

  constructor(
    private _userService: UserService,
    private _toastR: ToastrService,
    private _conversationService: ConversationService,
  ) { }
  ngOnInit(): void {
    this._userService.loggedUser$.subscribe({
      next : (res)=>{
        this.loggedUser = res;
        this.GetAllUsers();
      }
    })

  }

  GetAllUsers() {
    this._userService.GetAllUsers$().subscribe({
      next: (res) => {
        if (res.isSuccess) {
          this.allUsers = res.data.filter(u => u.userId != this.loggedUser?.userId);
          this.filterdUsers = this.allUsers;
        } else {
          this._toastR.error(res.message);
        }
      },
      error: (err) => {
        this._toastR.error(err.error.message);
      }
    })
  }

  filterUsers() {
    const query = this.inputValue.toLowerCase();
    this.filterdUsers = this.allUsers.filter(
      user =>
        user.name.toLowerCase().includes(query) &&
        !this.selectedUsers.includes(user)
    );
  }


  addUser(user: UserDto) {
    if (!this.selectedUsers.includes(user)) {
      this.selectedUsers.push(user);

      this.filterdUsers = this.filterdUsers.filter(u => u != user)
    }
    this.inputValue = '';
  }

  removeUser(index: number) {
    this.selectedUsers.splice(index, 1);
    this.filterdUsers = this.allUsers.filter(
      u => !this.selectedUsers.some(su => su.userId === u.userId)
    );
  }


  // Method to create the new Conversation
  CreateConversation(){
    if(this.selectedUsers.length == 0 || !this.loggedUser) {
      this._toastR.warning("Select at least one User");
      return;
    }
    if(this.selectedUsers.length != 1 && !this.conversationName){
      this._toastR.warning("Please Enter Conversation Name");
      return;
    }
    let selectedUserIds = this.selectedUsers.map((u) => u.userId)
    // insert the loggedUser
    selectedUserIds.push(this.loggedUser?.userId);
    const payload : CreateConversationRequest = {
      conversationName: this.conversationName,
      userIds: selectedUserIds,
    }
    
    this._conversationService.CreateConversation$(payload).subscribe({
      next : (res)=>{
        if(res.isSuccess){
          this._toastR.success(res.message);
          this.selectedUsers = [];
          this.conversationName = '';
          this.filterdUsers = this.allUsers;
        }else{
          this._toastR.error(res.message);
        }
      },
      error : (err : HttpErrorResponse) => {
        this._toastR.error(err.error.message)
      }
    })
  }
}
