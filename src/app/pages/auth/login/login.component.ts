import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { UserService } from '../../../core/services/userService/user.service';
import { LoginUserDto } from '../../../core/models/userModels/UserDtos';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { SignalrService } from '../../../core/services/signalRService/signalr-service.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  loginForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private toastR : ToastrService,
    private router : Router,
    private _signalRService : SignalrService
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(5)]]
    });
  }

  onSubmit() {
    if (this.loginForm.valid) {
      const payload : LoginUserDto = this.loginForm.value;
      this.userService.LoginUser$(payload).subscribe({
        next: (res) => {
          if (res.isSuccess) {
            localStorage.setItem("accessToken", res.data.accessToken);
            this.toastR.success(res.message);
            this.userService.resetLoggedUser();
            this.router.navigateByUrl("/Home");
          } else {
            this.toastR.error(res.message);
          }
        },
        error: (err: HttpErrorResponse) => {
          this.toastR.error(err.error.message);
        },
      })
    } else {
      this.loginForm.markAllAsTouched(); // show validation errors
    }
  }
}
