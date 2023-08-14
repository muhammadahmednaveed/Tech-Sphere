import { Component, Input } from '@angular/core';
import { Product } from '../../../models/Product';
import { CommonService } from 'src/app/services/common.service';
import { ApiService } from '../../../services/api.service';
import { Router } from '@angular/router';
@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.scss'],
})
export class ProductComponent {
  constructor(private common: CommonService, private router: Router) {}

  @Input() prod?: Product;

  value: number = 1;

  addToCart(product: Product) {
    console.log(product);
    //product.Quantity = product.Quantity + this.value;
    this.common.cartAddition(product, this.value);
    this.value = 1;
  }
  openProduct(product: Product) {
    this.common.openProduct(product);
    this.router.navigateByUrl('TopShop/products/product/' + product.Id);
  }
}
