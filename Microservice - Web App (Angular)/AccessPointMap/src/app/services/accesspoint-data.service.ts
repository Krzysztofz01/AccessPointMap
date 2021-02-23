import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { environment } from './../../environments/environment';
import { Accesspoint } from './../models/accesspoint.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccesspointDataService {
  private baseUrl: string = environment.apiUrl;
  private apiUrl: string = "/projects/accesspointmap/api/";
  
  constructor(private httpClient: HttpClient) { }

  public getAllAccessPoints() : Observable<Array<Accesspoint>> {
    return this.httpClient.get<Array<Accesspoint>>(this.url('accesspoints/master'));
  }

  public getAllAccessPointsAdmin(token: string) : Observable<Array<Accesspoint>> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });

    return this.httpClient.get<Array<Accesspoint>>(this.url('accesspoints/master/all'), { headers });
  }

  public getAccessPointById(id: number, token: string) : Observable<Accesspoint> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });

    return this.httpClient.get<Accesspoint>(this.url(`accesspoints/master/${id}`), { headers });
  }

  public getAccessPointByBssid(bssid: string, token: string) : Observable<Accesspoint> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });

    return this.httpClient.get<Accesspoint>(this.url(`accesspoints/master/bssid/${bssid}`), { headers });
  }

  public searchAccessPoints(token:string, ssid: string = null, freq: number = null, brand: string = null, security: string = null) : Observable<Array<Accesspoint>> {
    const params = new HttpParams()
      .set("ssid", ssid)
      .set("freq", freq.toString())
      .set("brands", brand)
      .set("security", security);

      const headers = new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${token}`
      });

    return this.httpClient.get<Array<Accesspoint>>(this.url('accesspoints/master/search'), { params, headers });
  }

  public displayAccesspoint(id: number, display: boolean, token: string) : Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });

    return this.httpClient.post(this.url('accesspoints/master/visibility'), { id, display }, { headers });
  }

  public mergeAccesspoints(ids: Array<number>, token: string) : Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });

    return this.httpClient.post(this.url('accesspoints/master/merge'), { ids }, { headers });
  }

  public mergeAllAccesspoints(token): Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });

    return this.httpClient.get(this.url('accesspoints/master/merge/all'), { headers });
  }

  public getBrands() : Observable<Array<string>> {
    return this.httpClient.get<Array<string>>(this.url('accesspoints/master/brands'));
  }

  public deleteAccesspoint(id: number, token: string) : Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });

    return this.httpClient.delete(this.url(`accesspoints/master/${id}`), { headers });
  }

  public addOrUpdateAccesspoint(accesspoint: Accesspoint, token: string) : Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });

    return this.httpClient.post(this.url('accesspoints/master'), [accesspoint], { headers });
  }

  public addOrUpdateAccesspointsMaster(data: object, token: string) : Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });

    return this.httpClient.post(this.url('accesspoints/master'), data, { headers });
  }

  public addOrUpdateAccesspointsQueue(data: object, token: string) : Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });

    return this.httpClient.post(this.url('accesspoints/queue'), data, { headers });
  }

  public searchBySsid(ssid: string): Observable<Array<Accesspoint>> {
    const params = new HttpParams().set("ssid", ssid);

    return this.httpClient.get<Array<Accesspoint>>(this.url('accesspoints/master/check'), { params });
  }

  private url(endpoint: string): string {
    return 'http://' + this.baseUrl + this.apiUrl + endpoint;
  }
}
