import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ErrorHandlingService } from 'src/app/services/error-handling.service';

@Component({
  selector: 'app-page-error',
  templateUrl: './page-error.component.html',
  styleUrls: ['./page-error.component.css']
})
export class PageErrorComponent implements OnInit {
  public message: string;

  constructor(private errorHandlingService: ErrorHandlingService, private router: Router) { }

  ngOnInit(): void {
    this.errorHandlingService.getException()
      .subscribe((response) => {
        if(response == null) {
          this.router.navigate(['/']);
        } else {
          this.message = response;
        }
      });
  }
}
