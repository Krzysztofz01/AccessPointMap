import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DateFormatingService {

  constructor() { }

  public single(date: Date): string {
    date = new Date(date);
    const d = date.getDate();
    const m = date.getMonth() + 1;
    const y = date.getFullYear();
    const H = date.getHours();
    const M = date.getMinutes();

    return `${ (d <= 9) ? '0' + d.toString() : d }.${ (m <= 9) ? '0' + m.toString() : m }.${ y } ${ (H <= 9) ? '0' + H.toString() : H }:${ (M <= 9) ? '0' + M.toString() : M }`;
  }

  public pair(firstDate: Date, secondDate: Date): string {
    firstDate = new Date(firstDate);
    const fd = firstDate.getDate();
    const fm = firstDate.getMonth() + 1;
    const fy = firstDate.getFullYear();
    const firstDateString = `${ (fd <= 9) ? '0' + fd.toString() : fd }.${ (fm <= 9) ? '0' + fm.toString() : fm }.${ fy }`;

    secondDate = new Date(secondDate);
    const sd = secondDate.getDate();
    const sm = secondDate.getMonth() + 1;
    const sy = secondDate.getFullYear();
    const secondDateString = `${ (sd <= 9) ? '0' + sd.toString() : sd }.${ (sm <= 9) ? '0' + sm.toString() : sm }.${ sy }`;

    return `${ firstDateString } / ${ secondDateString }`;
  }

  public pairSep(firstDate: Date, secondDate: Date): Array<string> {
    const dateArray: Array<string> = [];
    
    firstDate = new Date(firstDate);
    const fd = firstDate.getDate();
    const fm = firstDate.getMonth() + 1;
    const fy = firstDate.getFullYear();
    dateArray.push(`${ (fd <= 9) ? '0' + fd.toString() : fd }.${ (fm <= 9) ? '0' + fm.toString() : fm }.${ fy }`);

    secondDate = new Date(secondDate);
    const sd = secondDate.getDate();
    const sm = secondDate.getMonth() + 1;
    const sy = secondDate.getFullYear();
    dateArray.push(`${ (sd <= 9) ? '0' + sd.toString() : sd }.${ (sm <= 9) ? '0' + sm.toString() : sm }.${ sy }`);

    return dateArray;
  }
}
