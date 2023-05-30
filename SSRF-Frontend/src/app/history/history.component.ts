// Import required dependencies
import { Component } from '@angular/core';
import { Router } from '@angular/router';

// Define an interface for activity
interface Activity {
  timestamp: Date;
  description: string;
}

@Component({
  selector: 'app-history',
  templateUrl: './history.component.html',
  styleUrls: ['./history.component.css']
})
export class HistoryComponent {
  activities: Activity[] = [];

  constructor(private router: Router) {
    // Initialize some example activities
    this.activities.push({
      timestamp: new Date(),
      description: 'Activity 1'
    });

    this.activities.push({
      timestamp: new Date(),
      description: 'Activity 2'
    });

    // Add more activities as needed
  }

  goBack(): void {
    this.router.navigate(['/dashboard']);
  }
}
