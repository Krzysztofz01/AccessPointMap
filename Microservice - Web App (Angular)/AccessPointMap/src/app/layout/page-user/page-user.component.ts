import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/auth/services/auth.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-page-user',
  templateUrl: './page-user.component.html',
  styleUrls: ['./page-user.component.css']
})
export class PageUserComponent implements OnInit {
  public downloadLink: string;

  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit(): void {
    this.downloadLink = environment.APK_DOWNLOAD_URL;
  }

  public logout(): void {
    this.authService.logout();
    this.router.navigate(['/auth']);
  }

}
