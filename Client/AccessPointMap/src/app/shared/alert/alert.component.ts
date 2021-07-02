import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-alert',
  templateUrl: './alert.component.html',
  styleUrls: ['./alert.component.css']
})
export class AlertComponent {
  @Input() type: string;
  @Input() text: string;
  @Input() show: boolean;

  public getType(): string {
    return `alert-${ this.type }`;
  }
}
