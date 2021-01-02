import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/auth/services/auth.service';

@Component({
  selector: 'app-page-auth',
  templateUrl: './page-auth.component.html',
  styleUrls: ['./page-auth.component.css']
})
export class PageAuthComponent implements OnInit {
  public loginFormGroup: FormGroup;
  public registerFormGroup: FormGroup;

  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit(): void {
    this.formsInit();
  }

  public loginSubmit(): void {
    if(this.loginFormGroup.valid) {
      this.authService.login(this.loginFormGroup.value)
        .subscribe(res => {
          if(res) {
            const permission = this.authService.getPerm();
            if(permission == "Admin") {
              this.router.navigate(['/admin']);
            } else {
              this.router.navigate(['/user']);
            }
          }
        });
    }
  }

  public registerSubmit(): void {
    if(this.registerFormGroup.valid) {

    }
  }

  private formsInit(): void {
    this.loginFormGroup = new FormGroup({
      email: new FormControl('', [ Validators.required, Validators.email ]),
      password: new FormControl('', [ Validators.required ])
    });

    this.registerFormGroup = new FormGroup({
      email: new FormControl('', [ Validators.required, Validators.email ]),
      password: new FormControl('', [ Validators.required, Validators.minLength(5), Validators.maxLength(128)]),
      passwordRepeat: new FormControl('')
    });
  }

}
