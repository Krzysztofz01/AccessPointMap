import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError, mapTo, tap } from 'rxjs/operators';
import { Jwtoken } from 'src/app/models/jwtoken.model';
import { environment } from 'src/environments/environment';
import jwt_decode  from 'jwt-decode';
import { CacheManagerService } from 'src/app/services/cache-manager.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private cacheKey = environment.CACHE_JWT;

  private userEmail: string;
  private userPermission: string;

  constructor(private httpClient: HttpClient, private cacheService: CacheManagerService) { }

  public login(data: any): Observable<boolean> {
    return this.httpClient.post<Jwtoken>(this.url('auth/login'), data)
      .pipe(
        tap(token => this.doLogin(token.bearerToken)),
        mapTo(true),
        catchError(err => {
          console.error(err);
          return of(false);
        })
      );
  }

  public logout(): void {
    this.doLogout();
  }

  public isLoggedIn(): boolean {
    if(this.cacheService.load(this.cacheKey) == null) {
      this.cacheService.delete(this.cacheKey);
      return false;
    }
    return true;
  }

  public getPerm(): string {
    const { token } = this.cacheService.load(this.cacheKey)
    if(token == null) {
      this.cacheService.delete(this.cacheKey);
      return null;
    } else {
      const payload: any = jwt_decode(token);
      const role = payload.role;
      if(role != null) {
        return role;
      }
      return null;
    }
  }

  private doLogin(token: string): void {
    const payload: any = jwt_decode(token);
    this.userEmail = payload.email;
    this.userPermission = payload.role;

    this.cacheService.delete(this.cacheKey);
    this.cacheService.save({ key: this.cacheKey, data: { token }, expirationMinutes: 5 });
  }

  private doLogout(): void {
    this.userEmail = null;
    this.userPermission = null;
    this.cacheService.delete(this.cacheKey);
  }

  private url(endpoint: string):string {
    const apiUrl = '/projects/accesspointmap/api/';
    return `http://${ environment.apiUrl }${ apiUrl }${endpoint}`;
  }
}
