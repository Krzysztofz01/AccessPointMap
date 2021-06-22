import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DateParserService {
  constructor() { }

  public daysAgo(date: Date): number {
    const today = Date.now();
    const differenceInTime = today - new Date(date).getTime();
    return Math.round(differenceInTime / (1000 * 3600 * 24));
  }

  public parseDate(date: Date, time: boolean = true): string {
    const d = new Date(date);
    if(time) {
      return `${d.getFullYear()}-${ (d.getMonth() < 10) ? `0${d.getMonth()}` : d.getMonth() }-${ (d.getDate() < 10) ? `0${d.getDate()}` : d.getDate() } ${ (d.getHours() < 10) ? `0${d.getHours()}` : d.getHours() }:${ (d.getMinutes() < 10) ? `0${d.getMinutes()}` : d.getMinutes() }`;
    }
    return `${d.getFullYear()}-${ (d.getMonth() < 10) ? `0${d.getMonth()}` : d.getMonth() }-${ (d.getDate() < 10) ? `0${d.getDate()}` : d.getDate() }`;
  }

  public inDays(date: Date): number {
    const today = Date.now();
    const differenceInTime = new Date(date).getTime() - today;
    return Math.round(differenceInTime / (1000 * 3600 * 24));
  }
}
