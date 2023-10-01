import { Product } from 'src/app/models/Product';
export class Cart {
  CartId: number = 0;
  Products: Product[] = [];
  TotalPrice: number = 0;
  PurchaseDate: Date = new Date();
}
