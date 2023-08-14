import { Pipe, PipeTransform } from '@angular/core';
import { Product } from 'src/app/models/Product';

@Pipe({
  name: 'searchFilter',
})
export class SearchFilterPipe implements PipeTransform {
  transform(products: Product[], input: string): Product[] {
    return products
      ? products.filter(
          (item) => item.Title.search(new RegExp(input, 'i')) > -1
        )
      : [];
  }
}
