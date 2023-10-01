import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuardGuard } from '../guards/auth-guard.guard';
import { BannerComponent } from './banner/banner.component';
import { BuyerUIComponent } from './buyer-ui.component';
import { CartComponent } from './cart/cart.component';
import { ProductListComponent } from './product-list/product-list.component';
import { ProductPageComponent } from './product-page/product-page.component';
import { ProfilePageComponent } from './profile-page/profile-page.component';

const routes: Routes = [
  {
    path: 'TopShop',
    component: BuyerUIComponent,
    children: [
      {
        path: 'home',
        component: BannerComponent,
        canActivate: [AuthGuardGuard],
      },
      {
        path: 'products/:category',
        component: ProductListComponent,
        canActivate: [AuthGuardGuard],
      },
      {
        path: 'products/product/:id',
        component: ProductPageComponent,
        canActivate: [AuthGuardGuard],
      },
      { path: 'cart', component: CartComponent, canActivate: [AuthGuardGuard] },
      {
        path: '',
        redirectTo: 'home',
        pathMatch: 'full',
      },
      {
        path: 'profile/:id',
        component: ProfilePageComponent,
        canActivate: [AuthGuardGuard],
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class BuyerUIRoutingModule {}
