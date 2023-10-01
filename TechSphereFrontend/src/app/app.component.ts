import { Component, OnInit } from '@angular/core';
import { LoginPageComponent } from './login-ui/login-page/login-page.component';
import {
  ActivatedRoute,
  NavigationEnd,
  NavigationStart,
  Router,
} from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  constructor(private router: Router) {}

  ngOnInit(): void {
    this.router.events.subscribe((event: any) => {
      this.isLoggedIn = localStorage.getItem('User') != undefined;
      if (event instanceof NavigationStart) {
        if (
          (event.url == '/Account/login' || event.url == '/') &&
          this.isLoggedIn == true
        ) {
          this.router.navigateByUrl('TopShop/home');
        }
        console.log(event.url);
      }
    });

    // this.router.events.subscribe(val => {
    //   if (this.router.url != "/Account") {
    //       // do something
    //   } else {
    //      // do something else
    //   }
    // });
  }

  title = 'TopShopBuyerFrontend';

  isLoggedIn: boolean = false;
}
