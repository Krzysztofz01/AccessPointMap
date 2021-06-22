import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { AccessPoint } from '../models/access-point.model';
import { AccessPointStatistics } from '../models/access-point-statistics.model';

@Injectable({
  providedIn: 'root'
})
export class AccessPointService {
  private readonly path: string = "accesspoint";
  private readonly server: string = environment.SERVER_URL;

  constructor(private httpClient: HttpClient) { }

  private preparePath(apiVersion: number): string {
    return `${ this.server }/api/v${ apiVersion }/${this.path}`;
  }

  public postMany(accessPoints: Array<AccessPoint>, apiVersion: number): Observable<void> {
    return this.httpClient.post<void>(this.preparePath(apiVersion), accessPoints);
  }

  public getAllPublic(apiVersion: number): Observable<Array<AccessPoint>> {
    return this.httpClient.get<Array<AccessPoint>>(this.preparePath(apiVersion));
  }

  public getByIdPublic(accessPointId: number, apiVersion: number): Observable<AccessPoint> {
    return this.httpClient.get<AccessPoint>(`${ this.preparePath(apiVersion) }/${ accessPointId }`);
  }

  public deleteById(accessPointId: number, apiVersion: number): Observable<void> {
    return this.httpClient.delete<void>(`${ this.preparePath(apiVersion) }/${ accessPointId }`);
  }

  public patchById(accessPoint: AccessPoint, apiVersion: number): Observable<void> {
    return this.httpClient.patch<void>(`${ this.preparePath(apiVersion) }/${ accessPoint.id }`, accessPoint);
  }

  public changeDisplayById(accessPointId: number, apiVersion: number): Observable<void> {
    return this.httpClient.patch<void>(`${ this.preparePath(apiVersion) }/${ accessPointId }/display`, {});
  }

  public mergeById(accessPointId: number, apiVersion: number): Observable<void> {
    return this.httpClient.patch<void>(`${ this.preparePath(apiVersion) }/${ accessPointId }/merge`, {});
  }

  public getAllMaster(apiVersion: number): Observable<Array<AccessPoint>> {
    return this.httpClient.get<Array<AccessPoint>>(`${ this.preparePath(apiVersion) }/master`);
  }

  public getByIdMaster(accessPointId: number, apiVersion: number): Observable<AccessPoint> {
    return this.httpClient.get<AccessPoint>(`${ this.preparePath(apiVersion) }/master/${ accessPointId }`);
  }

  public getAllQueue(apiVersion: number): Observable<Array<AccessPoint>> {
    return this.httpClient.get<Array<AccessPoint>>(`${ this.preparePath(apiVersion) }/queue`);
  }

  public getByIdQueue(accessPointId: number, apiVersion: number): Observable<AccessPoint> {
    return this.httpClient.get<AccessPoint>(`${ this.preparePath(apiVersion) }/queue/${ accessPointId }`);
  }

  public searchBySsid(ssidKeyword: string, apiVersion: number): Observable<Array<AccessPoint>> {
    // TODO: ssidKeyword http encode and sanitize
    return this.httpClient.get<Array<AccessPoint>>(`${ this.preparePath(apiVersion) }/search/${ ssidKeyword }`);
  }

  public getStatistics(apiVersion: number): Observable<AccessPointStatistics> {
    return this.httpClient.get<AccessPointStatistics>(`${ this.preparePath(apiVersion) }/statistics`);
  }

  public getUserAdded(apiVersion: number): Observable<Array<AccessPoint>> {
    return this.httpClient.get<Array<AccessPoint>>(`${ this.preparePath(apiVersion) }/user/added`);
  }

  public getUserModified(apiVersion: number): Observable<Array<AccessPoint>> {
    return this.httpClient.get<Array<AccessPoint>>(`${ this.preparePath(apiVersion) }/user/modified`);
  }
}
