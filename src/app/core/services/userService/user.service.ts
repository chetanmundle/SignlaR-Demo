import { inject, Injectable } from '@angular/core';
import { CommonService } from '../commonService/common-service.service';
import { BehaviorSubject, Observable } from 'rxjs';
import { AppResponse } from '../../models/genericResponse.model';
import { CreateUserDto, LoggedUserDto, LoginUserDto, LoginUserResponseDto, UserDto } from '../../models/userModels/UserDtos';
import { jwtDecode } from 'jwt-decode';
import { SignalrService } from '../signalRService/signalr-service.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private commonService = inject(CommonService);

  loggedUser$: BehaviorSubject<LoggedUserDto> =
    new BehaviorSubject<LoggedUserDto>(new LoggedUserDto());

  constructor(
    private _signalRService : SignalrService,
  ) {
    this, this.getLoggedUser();
  }


  private getLoggedUser(): void {
    const accessToken = localStorage.getItem('accessToken');

    if (accessToken) {
      const decodedToken: any = jwtDecode(accessToken);
      const userId = decodedToken.userId;

      // Fetch the user details from the API
      this.GetLoggedUserById$(userId).subscribe({
        next: (res: AppResponse<LoggedUserDto>) => {
          if (res.isSuccess) {
            this.loggedUser$.next(res.data); // Update BehaviorSubject with user data

            // start SignalR Service            
            this._signalRService.start();
          } else {
            this.loggedUser$.next(new LoggedUserDto()); // Emit empty user data if the API fails
          }
        },
        error: () => {
          this.loggedUser$.next(new LoggedUserDto()); // Handle API errors gracefully
        },
      });
    } else {
      this.loggedUser$.next(new LoggedUserDto()); // Emit empty user data if no token exists
    }
  }

  public resetLoggedUser(): void {
    this.getLoggedUser();
  }

  GetLoggedUserById$(userId: number): Observable<AppResponse<LoggedUserDto>> {
    return this.commonService.get<AppResponse<LoggedUserDto>>(
      `user/${userId}/GetUserById`
    );
  }

  // Create New User
  RegisterUser$(payload: CreateUserDto): Observable<AppResponse<null>> {
    return this.commonService.post<AppResponse<null>>(
      'user/createUser', payload
    )
  }

  //Login Service
  LoginUser$(payload: LoginUserDto): Observable<AppResponse<LoginUserResponseDto>> {
    return this.commonService.post<AppResponse<LoginUserResponseDto>>(
      'user/Login', payload
    )
  }

  GetAllUsers$(): Observable<AppResponse<UserDto[]>> {
    return this.commonService.get<AppResponse<UserDto[]>>(
      `user/getAllUsers`
    );
  }
}
