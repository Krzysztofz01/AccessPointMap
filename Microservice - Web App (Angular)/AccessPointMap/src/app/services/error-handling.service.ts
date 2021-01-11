import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ErrorHandlingService {
  private exMessage = new BehaviorSubject<string>(null);

  constructor(private router: Router) { }

  public setException(message: string): void {
    this.exMessage.next(message);
    this.router.navigate(['/error']);
  }
  
  public getException(): Observable<string> {
    return this.exMessage.asObservable();
  }
}
