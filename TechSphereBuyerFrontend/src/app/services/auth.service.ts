import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, Observable, ObservableInput, throwError } from 'rxjs';
import { User } from '../models/User';
import Swal from 'sweetalert2/dist/sweetalert2.js';
import { CookieService } from 'ngx-cookie-service';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(
    private httpClient: HttpClient,
    private router: Router,
    private cookies: CookieService
  ) {}
  Url: string = 'http://cmdlhrltx331/Auth/api/Auth';

  responseStatus: number;

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
  };

  someFunction(): void {}
  loginTry(UserLogin: User) {
    this.httpClient
      .post(this.Url + '/login', UserLogin)
      .pipe(catchError(this.handleError.bind(this)))
      .subscribe({
        next: (result: User) => {
          if (result.UserType == 'seller') {
            this.showMessage(
              'You have a seller Account',
              'Redirecting to seller login page',
              'info',
              false
            );
            setTimeout(
              () => window.location.replace('http://cmdlhrltx332:3000/'),
              2000
            );
          } else {
            this.showMessage('Logged In Successfully', '', 'success', false);
            localStorage.setItem('User', JSON.stringify(result));
            this.router.navigate(['/TopShop/home']);
          }
        },
        error: (err) => {
          console.log(err);
        },
      });
  }

  register(UserRegister: User) {
    this.httpClient
      .post(this.Url + '/register', UserRegister, this.httpOptions)
      .pipe(catchError(this.handleError.bind(this)))
      .subscribe(() => {
        this.showMessage('Registration successful', '', 'success', false);
        this.router.navigateByUrl('Account/login');
      });
  }

  isLoggenIn(): boolean {
    return !!localStorage.getItem('User');
  }

  private handleError(
    err: any,
    caught: Observable<unknown>
  ): ObservableInput<any> {
    if (err.status == 400) {
      this.showMessage('The credentials are incorrect', '', 'error', false);
      console.log(caught);
      throw "'The credentials are incorrect'";
    } else {
      this.showMessage('an error occured', '', 'error', false);
      throw 'an error occured';
    }
  }
  showMessage(title, message, icon = null, showCancelButton = false) {
    setTimeout(() => Swal.close(), 2000);
    return Swal.fire({
      title: title,
      text: message,
      icon: icon,
      showConfirmButton: false,
      showCancelButton: showCancelButton,
    });
  }
}
