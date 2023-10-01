import { Component, OnDestroy, OnInit } from '@angular/core';
import { Cart } from 'src/app/models/Cart';
import { Product } from 'src/app/models/Product';
import { CommonService } from 'src/app/services/common.service';
import { ApiService } from './../../services/api.service';
import { SignalrService } from 'src/app/services/signalr.service';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.scss'],
})
export class CartComponent implements OnInit {
  constructor(
    private common: CommonService,
    private api: ApiService,
    private signalr: SignalrService
  ) {}

  async ngOnInit() {
    await this.getCart();
  }
  allCarts: Cart[] = [];
  cart: Cart = new Cart();
  cartNotEmpty: boolean;
  allCartsClosed: boolean = true;
  noPurchaseHistory: boolean = false;
  quantity: number = 1;

  async getCart() {
    this.cart = await this.common.getCart();
    this.totalUpdate();
    this.cartCheck();
  }

  remove(Id: number) {
    this.cart.Products = this.cart.Products.filter((p) => p.Id != Id);
    this.totalUpdate();
    this.cartCheck();

    this.api.removeCartProduct(Id, this.cart.CartId);
  }

  cartCheck() {
    this.cartNotEmpty =
      typeof this.cart != 'undefined' && this.cart.Products.length > 0;
  }

  totalUpdate() {
    this.cart.TotalPrice = this.cart.Products.reduce(
      (sum, current) => sum + current.Price * current.Quantity,
      0
    );
  }
  async checkout() {
    await this.api.checkout(this.cart.CartId);
    await this.signalr.requesttoServer('checkout', this.cart.Products.map((p) => p.Id));
    //TODO clear cart when redirecting to all products
  }

  changeQuantity(product: Product) {
    this.api.addToCart(product, 3);
  }
  async getAllCarts() {
    if (this.allCartsClosed) {
      this.allCarts = await this.common.getAllCarts();
      if (this.allCarts[0].Products.length == 0) {
        this.noPurchaseHistory = true;
      } else {
        this.allCartsClosed = false;
        this.noPurchaseHistory = false;
      }
    } else {
      this.allCartsClosed = true;
    }
    //this.allCartsClosed = true ? false : true;       //not working I don't know why

    console.log(this.allCarts);
  }

  plus(product: Product) {
    if (product.Quantity <= product.Inventory) {
      product.Quantity++;
    }
  }
  minus(product: Product) {
    if (product.Quantity > 1) {
      product.Quantity--;
    }
  }
}
