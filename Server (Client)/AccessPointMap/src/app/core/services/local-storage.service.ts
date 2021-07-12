import { Injectable } from '@angular/core';
import { LocalStorageOptions } from '../models/local-storage-options.model';

@Injectable({
  providedIn: 'root'
})
export class LocalStorageService {
  constructor() { }

  public get(key: string): any {
    if(!this.checkService()) {
      console.error('Local storage service is unavailable!');
      return null;
    }

    const itemSerialized = localStorage.getItem(key);
    if(itemSerialized !== null) {
      const item = JSON.parse(itemSerialized);
      const currentTime = new Date().getTime();
      
      if(!item || (item.hasExpiration && item.expiration <= currentTime)) {
        return null;
      } else {
        return JSON.parse(item.value)
      }
    }
    return null;
  }

  public set(options: LocalStorageOptions): void {
    if(!this.checkService()) {
      console.error('Local storage service is unavailable!');
      return;
    }

    options.expirationMinutes = options.expirationMinutes || 0;
    const expirationMilisec = (options.expirationMinutes !== 0) ? options.expirationMinutes * 60 * 1000 : 0;

    const item = {
      value: (typeof options.value === 'string') ? options.value : JSON.stringify(options.value),
      expiration: (expirationMilisec !== 0) ? new Date().getTime() + expirationMilisec : null,
      hasExpiration: (expirationMilisec !== 0) ? true : false
    }

    localStorage.setItem(options.key, JSON.stringify(item));
  }

  public unset(key: string): void {
    if(!this.checkService()) {
      console.error('Local storage service is unavailable!');
      return;
    }

    localStorage.removeItem(key);
  }

  public drop(): void {
    if(!this.checkService()) {
      console.error('Local storage service is unavailable!');
      return;
    }

    localStorage.clear();
  }

  private checkService(): boolean {
    const testValue = 'LOCAL_STORAGE_TEST';
    try {
      localStorage.setItem(testValue, testValue);
      localStorage.removeItem(testValue);
      return true;
    } catch(e) {
      return false;
    }
  }

  public test(): boolean {
    if(this.checkService()) return true;
    console.error('Local storage service is unavailable!');
    return false;
  }
}
