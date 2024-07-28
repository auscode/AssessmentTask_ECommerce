import { Routes } from '@angular/router';
import { ProductListComponent } from './product-list/product-list.component';
import { HomeComponent } from './home/home.component';
import { ShoppingCartComponent } from './cart/shopping-cart.component';
import { SalesReportComponent } from './sales/sales-report.component';

export const routes: Routes = [
    { path: '', component: HomeComponent },
    { path: "product-list", component: ProductListComponent},
    { path: "cart", component: ShoppingCartComponent},
    { path: "sales", component: SalesReportComponent }
];
