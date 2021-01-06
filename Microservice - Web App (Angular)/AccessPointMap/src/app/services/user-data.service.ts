import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { environment } from './../../environments/environment';
import { User } from './../models/user.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserDataService {
  private baseUrl: string = environment.apiUrl;
  private apiUrl: string = '/projects/accesspointmap/api/'

  constructor(private httpClient: HttpClient) { }

  public getAllUsers(token: string) : Observable<Array<User>> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });

    return this.httpClient.get<Array<User>>(this.url('auth/user'), { headers });
  }

  public getUserById(id: number, token: string) : Observable<User> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });

    return this.httpClient.get<User>(this.url(`auth/user/${ id }`), { headers });
  }

  public deleteUser(id: number, token: string) : Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });

    return this.httpClient.delete(this.url(`auth/user/${ id }`), { headers });
  }

  public activateUser(id: number, active: boolean, token: string) : Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });

    return this.httpClient.post(this.url('auth/user/activation'), { id, active }, { headers });
  }

  public updateUser(user: User, token: string): Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });

    return this.httpClient.put(this.url('auth/user'), user, { headers });
  }

  private url(endpoint: string): string {
    return 'http://' + this.baseUrl + this.apiUrl + endpoint;
  }
}
