import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, ObservableInput, catchError, Subscription } from 'rxjs';
import { Product } from 'src/app/models/Product';
import { Cart } from 'src/app/models/Cart';

import { Router } from '@angular/router';

import Swal from 'sweetalert2';

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  constructor(private httpClient: HttpClient, private router: Router) {
    this.router = router;
  }

  Url: string = 'http://localhost/TopShopBuyer/Product';
  subs: Subscription;
  //-----------------------------------------------------------------------------------------------

  getProductList(): Observable<Product[]> {
    return this.httpClient
      .get<Product[]>(this.Url + '/allproducts')
      .pipe(catchError(this.handleError.bind(this)));
  }

  //-----------------------------------------------------------------------------------------------

  addToCart(product: Product, addedProduct: number) {
    this.httpClient
      .post(
        this.Url + '/addtocart',
        {
          ProductID: product.Id,
          ProductQuantity: product.Quantity,
        },
        { responseType: 'text' }
      )
      .pipe(catchError(this.handleError.bind(this)))
      .subscribe(() => {
        if (addedProduct == 1) {
          this.showMessage('Product added to cart', '', 'success', false);
        } else if (addedProduct == 0) {
          this.showMessage(
            'Product Quantity updated in cart',
            '',
            'success',
            false
          );
        }
      });
  }

  //-----------------------------------------------------------------------------------------------

  removeCartProduct(productId: number, cartId: number) {
    this.httpClient
      .put(
        this.Url + `/removecartproduct/${productId}?cartId=${cartId}`,
        {},
        {
          responseType: 'text',
        }
      )
      .pipe(catchError(this.handleError.bind(this)))
      .subscribe();
  }

  //-----------------------------------------------------------------------------------------------

  async checkout(cartId: number) {
    this.subs = this.httpClient
      .put(this.Url + `/checkout/${cartId}`, {}, { responseType: 'text' })
      .pipe(catchError(this.handleError.bind(this)))
      .subscribe((result) => {
        this.showMessage(result, '', 'success', false);
        this.router.navigateByUrl('/TopShop/products/allproducts');
      });
  }

  //-----------------------------------------------------------------------------------------------

  getCart(): Observable<Cart> {
    return this.httpClient
      .get<Cart>(this.Url + '/getcart')
      .pipe(catchError(this.handleError.bind(this)));
  }

  getAllCarts(): Observable<Cart[]> {
    return this.httpClient
      .get<Cart[]>(this.Url + '/getallcart')
      .pipe(catchError(this.handleError.bind(this)));
  }

  private handleError(
    err: any,
    caught: Observable<unknown>
  ): ObservableInput<any> {
    if (err.status == 401) {
      localStorage.removeItem('User');
      this.showMessage('Session Expired Please Login again', '', 'info', false);
      this.router.navigateByUrl('Account/login');
      throw 'Session Expired Please Login again';
    } else if (err.status == 400) {
      this.showMessage(
        'Some error occured.Please refresh and try again -400',
        '',
        'error',
        false
      );
      throw 'Some error occured.(400)';
    } else {
      this.showMessage(
        'Some error occured.Please refresh and try again -500',
        '',
        'error',
        false
      );
      throw 'Some error occured.(500)';
    }
    console.log(err.message);
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

//-----------------------------------------------------------------------------------------------

// private sub = new Subject<Product>();
// public obs: Observable<Product> = this.sub.asObservable();
// getRepeater() {
//   setInterval(() => {
//     this.sub.next({} as Product);
//   }, 3000);
// }
