import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Product } from 'src/app/models/Product';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.scss'],
})
export class ProductListComponent implements OnInit {
  prod: Product[] = [];
  products: Product[];
  category: string;
  inputText: string = '';

  constructor(private common: CommonService, private route: ActivatedRoute) {
    console.log('allproducts constructor');
  }

  ngOnInit() {
    this.showProducts();
    this.common.textChange.subscribe((text) => (this.inputText = text));
    console.log('all products ngoninit');

    this.route.params.subscribe((params) => {
      this.category = params['category'];

      if (this.category == 'allproducts') {
        console.log('if');
        this.products = this.prod;
        this.category = 'All Products';
      } else {
        console.log('else');

        this.filterProducts(this.category);
      }
    });
  }

  async showProducts() {
    this.prod = await this.common.getProductList();
    this.products = this.prod;
    this.products = this.filterProducts(this.category);
  }

  search(value: string): void {
    this.products = this.prod.filter((val) =>
      val.Title.toLowerCase().includes(value)
    );
  }

  filterProducts(category: string) {
    if (category != 'All Products') {
      this.products = this.prod;
      this.products = this.prod.filter(
        (product) => product.Category == category
      );
      return this.products;
    }
    return this.products;
  }
}
