import { Component, OnInit } from '@angular/core';
import { Product } from 'src/app/models/Product';
import { CommonService } from 'src/app/services/common.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-product-page',
  templateUrl: './product-page.component.html',
  styleUrls: ['./product-page.component.scss'],
})
export class ProductPageComponent implements OnInit {
  constructor(private common: CommonService, private route: ActivatedRoute) {}
  async ngOnInit() {
    this.product = this.common.currentProduct;
    if (this.product == null) {
      console.log(this.product);
      this.productID = parseInt(this.route.snapshot.paramMap.get('id'));
      this.allProducts = await this.common.getProductList();
      this.product = this.allProducts.find(
        (Product) => Product.Id == this.productID
      );
    }
  }
  allProducts: Product[];
  productID: number;
  product: Product;
  value: number = 1;

  addToCart(product: Product) {
    console.log(product);
    //product.Quantity = product.Quantity + this.value;
    this.common.cartAddition(product, this.value);
    this.value = 1;
  }
}
