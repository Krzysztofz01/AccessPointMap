import { Injectable } from '@angular/core';
import { LocalStorageOptions } from './../models/local-storage-options.model';

@Injectable({
  providedIn: 'root'
})
export class CacheManagerService {

  constructor() { }

  public save(options: LocalStorageOptions) {
    options.expirationMinutes = options.expirationMinutes || 0;
    const expirationMs = options.expirationMinutes !== 0 ? options.expirationMinutes * 60 * 1000 : 0;
    
    const record = {
      value: typeof options.data === 'string' ? options.data : JSON.stringify(options.data),
      expiration: expirationMs !== 0 ? new Date().getTime() + expirationMs : null,
      hasExpiration: expirationMs !== 0 ? true : false
    }

    localStorage.setItem(options.key, JSON.stringify(record));
  }

  public load(key: string) {
    const item = localStorage.getItem(key);
    if(item !== null) {
      const record = JSON.parse(item);
      const now = new Date().getTime();

      if(!record || (record.hasExpiration && record.expiration <= now)) {
        return null
      } else {
        return JSON.parse(record.value);
      }
    }
    return null;
  }

  public delete(key: string) {
    localStorage.removeItem(key);
  }

  public drop() {
    localStorage.clear();
  }
}