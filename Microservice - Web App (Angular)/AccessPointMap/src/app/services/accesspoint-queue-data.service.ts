import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { AccesspointQueue } from '../models/acesspoint-queue.model';

@Injectable({
  providedIn: 'root'
})
export class AccesspointQueueDataService {
  private baseUrl: string = environment.apiUrl;
  private apiUrl: string = "/projects/accesspointmap/api/";

  constructor(private httpClient: HttpClient) { }

  public getAllAccessPoints(token: string): Observable<Array<AccesspointQueue>> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });

    return this.httpClient.get<Array<AccesspointQueue>>(this.url('accesspoints/queue'), { headers });
  }

  public getAccessPointById(id: number, token: string): Observable<AccesspointQueue> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });

    return this.httpClient.get<AccesspointQueue>(this.url(`accesspoints/queue/${id}`), { headers });
  }

  public deleteAccesspoint(id: number, token: string): Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });

    return this.httpClient.delete(this.url(`accesspoints/queue/${id}`), { headers });
  }

  private url(endpoint: string): string {
    return `http://${this.baseUrl}${this.apiUrl}${endpoint}`;
  }
}
