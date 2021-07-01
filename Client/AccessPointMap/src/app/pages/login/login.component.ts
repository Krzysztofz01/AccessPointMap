import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/authentication/services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  public loginForm: FormGroup;

  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit(): void {
    this.loginForm = new FormGroup({
      email: new FormControl('', [ Validators.required, Validators.email ]),
      password: new FormControl('', [ Validators.required, Validators.minLength(6) ])
    });
  }

  public loginSubmit(): void {
    if(this.loginForm.valid) {
      this.authService.login({
        email: this.loginForm.get('email').value,
        password: this.loginForm.get('password').value
      }).subscribe((res) => {
        this.router.navigate(['']);
      },
      (error) => {
        //TODO: Notification
        console.error(error);
      });
    }
  }

  public register(): void {

  }
}
