// dashboard.component.ts
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from './../user.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { WeatherComponent } from '../weather/weather.component';

interface Sensor {
  id: number;
  label: string;
  value: number;
}

interface Actor {
  id: number;
  label: string;
  status: string;
}

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent {
  isAdmin: boolean = false;
  username: string = '';
  password: string = '';
  sensors: Sensor[] = [
    { id: 1, label: 'Sensor 1', value: 10 },
    { id: 2, label: 'Sensor 2', value: 20 },
    { id: 3, label: 'Sensor 3', value: 30 }
  ];
  actors: Actor[] = [
    { id: 1, label: 'Actor 1', status: 'On' },
    { id: 2, label: 'Actor 2', status: 'Off' },
    { id: 3, label: 'Actor 3', status: 'On' }
  ];

  constructor(private router: Router, private userService: UserService, private http: HttpClient) {
    this.username = this.userService.getUserRole();

    this.isAdmin = this.username === 'admin';
  }

  toggleActorStatus(actor: Actor) {
    actor.status = actor.status === 'On' ? 'Off' : 'On';
  }

  goToConfiguration() {
    this.router.navigate(['/configuration']);
  }

  goToHistory() {
    this.router.navigate(['/history']);
  }

  // Method to get temperature data from the temperature sensor
getTemperature(): void {
  this.http.get('https://localhost:7001/temperature').subscribe((response) => {
    // Handle the temperature sensor response
    alert(response);
  }, (error) => {
    // Handle any errors
    console.error('Error:', error);
  });
}

// Method to get light data from the light sensor
getLight(): void {
  this.http.get('https://localhost:7001/light').subscribe((response) => {
    // Handle the light sensor response
    alert(response);
  }, (error) => {
    // Handle any errors
    console.error('Error:', error);
  });
}

// Method to get refrigerator temperature data from the refrigerator temperature sensor
getRefrigeratorTemperature(): void {
  this.http.get('https://localhost:7001/refrigeratortemperature').subscribe((response) => {
    // Handle the refrigerator temperature sensor response
    alert(response);
  }, (error) => {
    // Handle any errors
    console.error('Error:', error);
  });
}

// Method to set the temperature value for the temperature regulator actor
setTemperature(event: any): void {
  const value = event.value as number;
  const payload = { temperature: value };

  this.http.post('https://localhost:7001/settemperature', payload).subscribe((response) => {
    // Handle the response
    console.log('Temperature Set:', response);
  }, (error) => {
    // Handle any errors
    console.error('Error:', error);
  });
}

// Method to set the light value for the light regulator actor
setLight(event: any): void {
  const value = event.value as number;
  const payload = { light: value };

  this.http.post('https://localhost:7001/setlight', payload).subscribe((response) => {
    // Handle the response
    console.log('Light Set:', response);
  }, (error) => {
    // Handle any errors
    console.error('Error:', error);
  });
}

// Method to set the temperature value for the refrigerator temperature actor
setRefrigeratorTemperature(event: any): void {
  const value = event.value as number;
  const payload = { temperature: value };
  const httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };

  this.http.post('https://localhost:7001/setrefrigeratortemperature', payload, httpOptions).subscribe((response) => {
    // Handle the response
    console.log('Refrigerator Temperature Set:', response);
  }, (error) => {
    // Handle any errors
    console.error('Error:', error);
  });
}

  logout() {
    // Placeholder logic for logout
    console.log('Logout successful');
    this.router.navigate(['/login']);
  }
}