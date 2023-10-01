import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/models/User';
import {
  FormBuilder,
  FormGroup,
  FormControl,
  Validators,
  FormArray,
} from '@angular/forms';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.scss'],
})
export class LoginPageComponent implements OnInit {
  constructor(private auth: AuthService) {}

  ngOnInit(): void {
    this.loginForm = new FormGroup({
      Username: new FormControl('', [Validators.required]),
      Password: new FormControl('', [Validators.required]),
    });
  }
  loginForm: FormGroup;
  user: User = new User();
  loginTry() {
    if (this.loginForm.valid) {
      this.user = this.loginForm.value;

      // var formData: any = new FormData();
      // formData.append('Username', this.loginForm.get('Username').value);
      // formData.append('Password', this.loginForm.get('Password').value);
    }
    this.auth.loginTry(this.user);
    // .subscribe(
    //   (response) => {
    //     console.log('Response status:', response.status);
    //     console.log('Response body:', response.body);
    //   },
    //   (error) => {
    //     console.error('Error:', error);
    //   }
    // );
  }

  _v() {
    return this.loginForm.value;
  }
}
