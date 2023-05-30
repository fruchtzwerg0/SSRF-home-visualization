import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private authenticated: boolean = false;
  private userRole?: string;

  login(username: string, password: string): boolean {
    // Perform authentication logic (e.g., API call, check credentials)
    // If authentication is successful, set the user as authenticated and store the user role
    if (username === 'user' && password === 'user') {
      this.authenticated = true;
      this.userRole = 'user';
      return true;
    } else if (username === 'admin' && password === 'admin') {
      this.authenticated = true;
      this.userRole = 'admin';
      return true;
    } else {
      return false;
    }
  }

  isAuthenticated(): boolean {
    return this.authenticated;
  }

  getUserRole(): string {
    return this.userRole!;
  }

  logout(): void {
    // Perform logout logic (e.g., clear tokens, session)
    this.authenticated = false;
    this.userRole = undefined;
  }
}
