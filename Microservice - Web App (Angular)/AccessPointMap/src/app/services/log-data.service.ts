import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Log } from '../models/log.model';

@Injectable({
  providedIn: 'root'
})
export class LogDataService {
  private baseUrl: string = environment.apiUrl;
  private apiUrl: string = "/projects/accesspointmap/api/";

  constructor(private httpClient: HttpClient) { }

  public getLogs(token: string): Observable<Array<Log>> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });

    return this.httpClient.get<Array<Log>>(this.url('logs'), { headers });
  }

  private url(endpoint: string): string {
    return `http://${ this.baseUrl }${ this.apiUrl }${ endpoint }`;
  }
}
