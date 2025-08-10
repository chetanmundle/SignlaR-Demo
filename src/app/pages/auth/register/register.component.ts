import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { CreateUserDto } from '../../../core/models/userModels/UserDtos';
import { UserService } from '../../../core/services/userService/user.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ ReactiveFormsModule, CommonModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
   registerForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private userService : UserService,
    private toastR  : ToastrService,
    private router : Router
  ) {
    this.registerForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(5)]]
    });
  }

  onSubmit() {
    if (this.registerForm.valid) {
      const payload : CreateUserDto = this.registerForm.value;
      
      this.userService.RegisterUser$(payload).subscribe({
        next : (res)=>{
          console.log("response : ", res);
          
          if(res.isSuccess){
            this.toastR.success(res.message);
            this.router.navigateByUrl("/Login");
          }else{
            this.toastR.error(res.message);
          }
        },
        error : (err : HttpErrorResponse) => {          
          this.toastR.error(err.error.message);
        },
      })
    } else {
      this.registerForm.markAllAsTouched();
    }
  }

}