import { Component, inject, OnInit } from '@angular/core';
import { UserService } from '../../../core/services/userService/user.service';
import { LoggedUserDto } from '../../../core/models/userModels/UserDtos';
import { ChatComponent } from '../chat/chat.component';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [ChatComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {

  loggedUser? : LoggedUserDto;
  _userService = inject(UserService);

  ngOnInit(): void {
    this._userService.loggedUser$.subscribe({
      next : (user)=>{
        this.loggedUser = user;
      }
    })
  }

}
