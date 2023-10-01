import { EventEmitter, Injectable, OnInit, Output } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { Product } from '../models/Product';
import { ApiService } from './api.service';
import { Cart } from '../models/Cart';
import Swal from 'sweetalert2';

@Injectable({
  providedIn: 'root',
})
export class CommonService implements OnInit {
  textChange = new EventEmitter<string>();
  productList: Product[] = [];
  cart: Cart = new Cart();
  allCarts: Cart[] = [];
  addedProduct: number = 1;
  currentProduct: Product;

  constructor(private api: ApiService) {}

  async ngOnInit() {}

  async getCart() {
    this.cart = await firstValueFrom(this.api.getCart());
    console.log('getcart');
    console.log(this.cart);
    return this.cart;
  }

  async getAllCarts() {
    this.allCarts = await firstValueFrom(this.api.getAllCarts());
    return this.allCarts;
  }

  async getProductList() {
    this.productList = await firstValueFrom(this.api.getProductList());
    return this.productList;
  }

  //#region  21 -----------------------------------------------------------------------------------------------

  cartAddition(product: Product, value: number): void {
    console.log('cartAddition: ', product);
    if (!this.cart.Products.find((s) => s.Id == product.Id)) {
      product.Quantity = value;
      this.cart.Products.push(product);
      this.addedProduct = 1;
      console.log(this.cart);
    } else {
      product = this.cart.Products.find((s) => s.Id == product.Id);
      if (
        !(
          value > product.Inventory ||
          value + product.Quantity > product.Inventory
        )
      ) {
        product.Quantity += value;
        this.addedProduct = 0;
      } else {
        alert(
          'product quantity exceeds the available inventory. The available inventory is ' +
            product.Inventory
        );
        this.addedProduct = 2;
      }
      console.log(product);
    }

    this.api.addToCart(product, this.addedProduct);
  }

  openProduct(product: Product) {
    this.currentProduct = product;
  }

  //-----------------------------------------------------------------------------------------------

  searchTerm(text: string) {
    this.textChange.emit(text);
  }

  //-----------------------------------------------------------------------------------------------
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
//#endregion
