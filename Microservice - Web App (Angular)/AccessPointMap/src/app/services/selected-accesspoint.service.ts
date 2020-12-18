import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Accesspoint } from '../models/accesspoint.model';

@Injectable({
  providedIn: 'root'
})
export class SelectedAccesspointService {
  private accesspointSource = new BehaviorSubject<Accesspoint>(null);
  currentAccesspoint = this.accesspointSource.asObservable();

  constructor() { }

  changeAccesspoint(accesspoint: Accesspoint) {
    this.accesspointSource.next(accesspoint);
  }
}
