import { Component } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';

// Add http options for API CALL
const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
};

@Component({
  selector: 'app-weather',
  templateUrl: './weather.component.html',
  styleUrls: ['./weather.component.css']
})
export class WeatherComponent {

  weatherApiUrl: string = '';
  weatherData: any;

  constructor(private http: HttpClient) { }

  getWeatherData() {
    // Make a POST request to the server API
    const url = 'https://localhost:7001/weatherapi';

    const payload = { url: this.weatherApiUrl };

    this.http.post(url, payload, httpOptions).subscribe(
      (response) => {
        console.log('Weather API URL saved successfully.');
        this.weatherData = JSON.stringify(response);
      },
      (error) => {
        console.error('Error using Weather API URL:', error);
      }
    );
  }
}
