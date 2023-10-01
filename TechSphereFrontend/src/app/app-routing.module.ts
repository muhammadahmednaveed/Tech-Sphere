import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BuyerUIComponent } from './buyer-ui/buyer-ui.component';
import { LoginUIComponent } from './login-ui/login-ui.component';
import { AuthGuardGuard } from './guards/auth-guard.guard';

const routes: Routes = [
  {
    path: 'TopShop',
    component: BuyerUIComponent,
  },
  { path: 'Account', component: LoginUIComponent },
  // { path: 'products/product/:id', component: ProductPageComponent },
  // { path: 'cart', component: CartComponent },
  { path: '', redirectTo: 'Account/login', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],

  exports: [RouterModule],
})
export class AppRoutingModule {}
