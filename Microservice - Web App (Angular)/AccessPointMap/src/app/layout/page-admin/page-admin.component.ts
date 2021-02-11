import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/auth/services/auth.service';
import { NavToggler } from 'src/app/models/nav-toggler.model';
import { CacheManagerService } from 'src/app/services/cache-manager.service';
import { environment } from 'src/environments/environment';

const innerComponents: Array<NavToggler> = [
  { name: 'masterAp', visible: false },
  { name: 'queueAp', visible: false },
  { name: 'users', visible: false },
  { name: 'add', visible: false },
  { name: 'log', visible: false }
];

@Component({
  selector: 'page-admin',
  templateUrl: './page-admin.component.html',
  styleUrls: ['./page-admin.component.css']
})
export class PageAdminComponent implements OnInit {

  constructor(private router: Router, private authService: AuthService, private cacheService: CacheManagerService) { }

  ngOnInit(): void {
    this.toggleComp('masterAp');
  }

  public logout(): void {
    this.authService.logout();
    this.router.navigate(['/auth']);
  }

  public reload(): void {
    this.cacheService.delete(environment.CACHE_ACCESSPOINTS);
    this.cacheService.delete(environment.CACHE_CHART_AREA);
    this.cacheService.delete(environment.CACHE_CHART_BRANDS);
    this.cacheService.delete(environment.CACHE_CHART_FREQUENCY);
    this.cacheService.delete(environment.CACHE_CHART_SECURITY);
    this.router.navigate(['/']);
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
