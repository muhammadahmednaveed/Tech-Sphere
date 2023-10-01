import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BuyerUIRoutingModule } from './buyer-ui-routing.module';
import { ProductComponent } from './product-list/product/product.component';
import { CartComponent } from './cart/cart.component';
import { BannerComponent } from './banner/banner.component';
import { ProductListComponent } from './product-list/product-list.component';
import { ProductPageComponent } from './product-page/product-page.component';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { ToastrModule } from 'ngx-toastr';
import { SearchFilterPipe } from '../pipes/search-filter.pipe';
import { BuyerUIComponent } from './buyer-ui.component';
import { HeaderComponent } from './header/header.component';

@NgModule({
  declarations: [
    ProductComponent,
    CartComponent,
    BannerComponent,
    ProductListComponent,
    ProductPageComponent,
    SearchFilterPipe,
    BuyerUIComponent,
    HeaderComponent,
  ],
  imports: [
    CommonModule,
    BuyerUIRoutingModule,
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    ToastrModule.forRoot(),
  ],
})
export class BuyerUIModule {}
