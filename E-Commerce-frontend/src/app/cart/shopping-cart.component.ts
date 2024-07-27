import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';  // Import FormsModule here
import { ProductService } from '../services/product.service'; // Adjust path as necessary
import { CartService } from '../services/cart.service'; // Adjust path as necessary
import { CommonModule } from '@angular/common';  // Add this line


@Component({
  selector: 'app-shopping-cart',
  templateUrl: './shopping-cart.component.html',
  styleUrls: ['./shopping-cart.component.css'],
  standalone: true,
   imports: [FormsModule, CommonModule]  
   // Add FormsModule to the imports array
})
export class ShoppingCartComponent implements OnInit {
  products: any[] = [];
  cartItems: any[] = [];
  discountCode: string = '';
  discountApplied: boolean = false;
  discountAmount: number = 0;
  totalValue: number = 0;
  totalValueAfterDiscount: number = 0;

  constructor(private productService: ProductService, private cartService: CartService) { }

  ngOnInit(): void {
    this.loadProducts();
    this.loadCart();
  }

  loadProducts(): void {
    this.productService.getProducts().subscribe({
      next: (products) => {
        this.products = products;
        // console.log(products)
      },
      error: (error) => {
        console.error("Error loading products:", error);
      }
    });
  }

  loadCart(): void {
    this.cartService.getCartItems().subscribe(cartItems => {
      this.cartItems = cartItems;
      this.calculateTotal();
      console.log(cartItems)
    });
  }

  addToCart(product: any): void {
    this.cartService.addToCart(product).subscribe(() => {
      this.loadCart();
    });
  }

  applyDiscount(): void {
    if (this.discountCode === 'DISCOUNT10') {
      this.discountApplied = true;
      this.discountAmount = 10; // Example fixed discount amount
    } else {
      this.discountApplied = false;
      this.discountAmount = 0;
    }
    this.calculateTotal();
  }

  calculateTotal(): void {
    this.totalValue = this.cartItems.reduce((sum, item) => sum + item.total, 0);
    this.totalValueAfterDiscount = this.totalValue - this.discountAmount;
  }
}
