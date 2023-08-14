import {
  ChangeDetectionStrategy,
  Component,
  OnChanges,
  OnInit,
  SimpleChanges,
} from '@angular/core';
import { Router } from '@angular/router';
import { pipe, map } from 'rxjs';
import { Product } from '../../models/Product';
import { CommonService } from '../../services/common.service';
import { JwtHelperService } from '@auth0/angular-jwt';
import { User } from 'src/app/models/User';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
})
export class HeaderComponent implements OnInit {
  username: string;
  categories: string[] = [];
  product: Product[] = [];
  
  constructor(private common: CommonService, private router: Router) {}

  ngOnInit(): void {
    this.showProducts();
    this.username = JSON.parse(localStorage.getItem('User')).Username;
  }
  async showProducts() {
    this.product = await this.common.getProductList();
    this.categories = [...new Set(this.product.map((value) => value.Category))];
  }

  search(event) {
    this.common.searchTerm(event.target.value);
  }

  logout() {
    localStorage.removeItem('User');
    this.router.navigateByUrl('Account/login');
  }
}
