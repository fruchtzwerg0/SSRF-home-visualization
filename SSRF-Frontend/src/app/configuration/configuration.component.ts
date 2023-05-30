import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from './../user.service';

@Component({
  selector: 'app-configuration',
  templateUrl: './configuration.component.html',
  styleUrls: ['./configuration.component.css']
})
export class ConfigurationComponent implements OnInit {
  isAdmin: boolean = false;
  username: string = '';
  password: string = '';

  constructor(private router: Router, private userService: UserService) {
    this.username = this.userService.getUserRole();

    this.isAdmin = this.username === 'admin';
  }

  ngOnInit(): void {
  }

  goBack(): void {
    this.router.navigate(['/dashboard']);
  }
}
