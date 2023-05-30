// login.component.ts
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from './../user.service';

@Component({
  selector: 'app-login-screen',
  templateUrl: './login-screen.component.html',
  styleUrls: ['./login-screen.component.css']
})
export class LoginScreenComponent {
  username: string = '';
  password: string = '';
  errorMessage: string = '';

  constructor(private router: Router, private userService: UserService) {}

  login() {
    if (this.userService.login(this.username, this.password)){
      console.log('Login successful');
      this.router.navigate(['/dashboard']);
    }
    else{
      console.log('Login failed');
      this.errorMessage = 'Invalid username or password';
    }
  }
}