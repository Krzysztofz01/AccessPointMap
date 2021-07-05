import { Component } from '@angular/core';
import { AuthService } from 'src/app/authentication/services/auth.service';

@Component({
  selector: 'app-panel',
  templateUrl: './panel.component.html',
  styleUrls: ['./panel.component.css']
})
export class PanelComponent {
  private selectedComponent: number = 1;

  constructor(private authService: AuthService) { }

  //Metohd to select a child component to display
  public setComp(id: number): void {
    this.selectedComponent = id;
  }

  //Method that checks the condition to display a child component
  public getComp(id: number): boolean {
    return this.selectedComponent == id;
  }

  //Logout button method
  public logout(): void {
    this.authService.logout();
  }

  //Role check for admin
  public isAdmin(): boolean {
    return this.authService.userValue.role == 'Admin';
  }

  //Role check for admin or mod
  public isAdminOrMod(): boolean {
    return this.authService.userValue.role == 'Admin' || this.authService.userValue.role == 'Mod';
  }
}
