import { Injectable } from '@angular/core';
import { LocalStorageOptions } from '../models/local-storage-options.model';
import { AccesspointDataService } from './accesspoint-data.service';
import { CacheManagerService } from './cache-manager.service';

const CACHE_KEY_ACCESSPOINTS = "VECTOR_LAYER_ACCESSPOINTS";
const CACHE_KEY_BRANDS = "BRANDS_ACCESSPOINTS"

@Injectable({
  providedIn: 'root'
})
export class DataFetchService {

  constructor(private accesspointDataService: AccesspointDataService, private cacheService: CacheManagerService) { }

  public localDataCheck(): void {
    this.fetchAccessPoints();
    this.fetchBrands();
  }

  private fetchAccessPoints(): void {
    const localStorageData = this.cacheService.load(CACHE_KEY_ACCESSPOINTS);
    if(localStorageData == null) {
      this.accesspointDataService.getAllAccessPoints("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6ImZyb250ZW5kQGFwbS5jb20iLCJyb2xlIjoiUmVhZCIsIm5iZiI6MTYwODY2MzkwMCwiZXhwIjoxNjA4NjcxMTAwLCJpYXQiOjE2MDg2NjM5MDB9.AIaiWbJsWJ2jtTO9bx7lCcb08OprE83h2GUXbcCRdVg").toPromise()
      .then(data => {
        const options: LocalStorageOptions = { key: CACHE_KEY_ACCESSPOINTS, data: data , expirationMinutes: 60 };
        this.cacheService.delete(CACHE_KEY_ACCESSPOINTS);
        this.cacheService.save(options);
      })
      .catch(error => {
        console.log(error);
      });
    }
  }

  private fetchBrands(): void {
    const localStorageData = this.cacheService.load(CACHE_KEY_BRANDS);
    if(localStorageData == null) {
      this.accesspointDataService.getBrands("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6ImZyb250ZW5kQGFwbS5jb20iLCJyb2xlIjoiUmVhZCIsIm5iZiI6MTYwODY2MzkwMCwiZXhwIjoxNjA4NjcxMTAwLCJpYXQiOjE2MDg2NjM5MDB9.AIaiWbJsWJ2jtTO9bx7lCcb08OprE83h2GUXbcCRdVg").toPromise()
      .then(data => {
        const options: LocalStorageOptions = { key: CACHE_KEY_BRANDS, data: data , expirationMinutes: 120 };
        this.cacheService.delete(CACHE_KEY_BRANDS);
        this.cacheService.save(options);
      })
      .catch(error => {
        console.log(error);
      });
    }
  }
}
