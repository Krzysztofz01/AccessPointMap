import { Injectable } from '@angular/core';
import { CanActivate, CanLoad, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class PageUserGuard implements CanActivate, CanLoad {
  
  constructor(private authService: AuthService, private router: Router) {}
  
  canLoad(): boolean {
    if(!this.authService.isLoggedIn() || this.authService.getPerm() == 'Admin') {
      this.router.navigate(['/auth']);
      return false;
    }
    return true;
  }
  
  canActivate(): boolean {
    return this.canLoad();
  }  
  
}
