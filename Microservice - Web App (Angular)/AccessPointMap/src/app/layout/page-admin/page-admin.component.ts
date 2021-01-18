import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/auth/services/auth.service';
import { NavToggler } from 'src/app/models/nav-toggler.model';

const innerComponents: Array<NavToggler> = [
  { name: 'masterAp', visible: false },
  { name: 'queueAp', visible: false },
  { name: 'users', visible: false },
  { name: 'add', visible: false }
];

@Component({
  selector: 'page-admin',
  templateUrl: './page-admin.component.html',
  styleUrls: ['./page-admin.component.css']
})
export class PageAdminComponent implements OnInit {

  constructor(private router: Router, private authService: AuthService) { }

  ngOnInit(): void {
    this.toggleComp('masterAp');
  }

  public logout(): void {
    this.authService.logout();
    this.router.navigate(['/auth']);
  }

  public isToggled(name: string): boolean {
    return innerComponents.find(x => x.name == name).visible;
  }

  public toggleComp(name: string): void {
    innerComponents.forEach(x => {
      x.visible = false;
    });
    innerComponents.find(x => x.name == name).visible = true;
  }

}
