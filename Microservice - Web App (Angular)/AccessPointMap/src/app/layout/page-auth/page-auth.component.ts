import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/auth/services/auth.service';
import { ErrorHandlingService } from 'src/app/services/error-handling.service';

@Component({
  selector: 'app-page-auth',
  templateUrl: './page-auth.component.html',
  styleUrls: ['./page-auth.component.css']
})
export class PageAuthComponent implements OnInit {
  public loginFormGroup: FormGroup;
  public registerFormGroup: FormGroup;
  public registerMessage: boolean;

  constructor(private authService: AuthService, private router: Router, private errorHandlingSerive: ErrorHandlingService) { }

  ngOnInit(): void {
    this.registerMessage = false;
    this.formsInit();
  }

  public loginSubmit(): void {
    if(this.loginFormGroup.valid || this.loginFormGroup.get('email').errors?.serverError) {
      this.authService.login(this.loginFormGroup.value)
        .subscribe(res => {
          console.log(res);
          if(res) {
            const permission = this.authService.getPerm();
            if(permission == "Admin") {
              this.router.navigate(['/admin']);
            } else {
              this.router.navigate(['/user']);
            }
          } else {
            this.loginFormGroup.get('email').setErrors({ serverError: 'Invalid email or password'});
          }
        },
        (error) => {
          this.errorHandlingSerive.setException(`${error.name} ${error.statusText}`);
        });
    }
  }

  public registerSubmit(): void {
    if(this.registerFormGroup.valid) {
      this.authService.register(this.registerFormGroup.value)
        .subscribe((response) => {
          console.log(response);
          this.registerMessage = true;
          this.registerFormGroup.reset();
        },
        (error) => {
          console.log(error);
          this.registerFormGroup.reset();
        });
    }
  }
    
  private formsInit(): void {
    this.loginFormGroup = new FormGroup({
      email: new FormControl('', [ Validators.required, Validators.email ]),
      password: new FormControl('', [ Validators.required ])
    });

    this.registerFormGroup = new FormGroup({
      email: new FormControl('', [ Validators.required, Validators.email ]),
      password: new FormControl('', [ Validators.required, Validators.minLength(5), Validators.maxLength(128)])
    });
  }
}
