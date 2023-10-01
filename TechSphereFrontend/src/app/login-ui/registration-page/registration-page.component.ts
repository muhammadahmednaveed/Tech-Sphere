import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { User } from 'src/app/models/User';
import { AuthService } from './../../services/auth.service';

@Component({
  selector: 'app-registration-page',
  templateUrl: './registration-page.component.html',
  styleUrls: ['./registration-page.component.scss'],
})
export class RegistrationPageComponent {
  constructor(private auth: AuthService) {}

  ngOnInit(): void {
    this.registerForm = new FormGroup({
      Email: new FormControl('', [Validators.required, Validators.email]),
      Username: new FormControl('', [Validators.required]),
      Fullname: new FormControl('', [Validators.required]),
      Password: new FormControl('', [Validators.required]),
      UserType: new FormControl('buyer', [Validators.required]),
    });
  }
  registerForm: FormGroup;
  user: User = new User();
  register() {
    if (this.registerForm.valid) {
      this.user = this.registerForm.value;

      // var formData: any = new FormData();
      // formData.append('Email', this.registerForm.get('Email').value);
      // formData.append('Username', this.registerForm.get('Username').value);
      // formData.append('Fullname', this.registerForm.get('Fullname').value);
      // formData.append('Password', this.registerForm.get('Password').value);
      // formData.append('UserType', this.registerForm.get('UserType').value);
    }
    this.auth.register(this.user);
  }

  registrationData() {
    return this.registerForm.value;
  }
}
