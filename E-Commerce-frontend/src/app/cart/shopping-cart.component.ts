import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ProductService } from '../services/product.service'; // Adjust path as necessary
import { CartService } from '../services/cart.service'; // Adjust path as necessary
import { DiscountService } from '../services/discount.service'; // Adjust path as necessary
import { CommonModule } from '@angular/common';  // Add this line

@Component({
  selector: 'app-shopping-cart',
  templateUrl: './shopping-cart.component.html',
  styleUrls: ['./shopping-cart.component.css'],
  standalone: true,
  imports: [FormsModule, CommonModule]
})
export class ShoppingCartComponent implements OnInit {
  products: any[] = [];
  cartItems: any[] = [];
  discounts: any[] = []; // Array to store available discounts
  discountCode: string = '';
  discountApplied: boolean = false;
  discountAmount: number = 0;
  totalValue: number = 0;
  totalValueAfterDiscount: number = 0;

  constructor(
    private productService: ProductService,
    private cartService: CartService,
    private discountService: DiscountService // Inject the DiscountService
  ) { }

  ngOnInit(): void {
    this.loadProducts();
    this.loadCart();
    this.loadDiscounts(); // Fetch available discounts
  }

  loadProducts(): void {
    this.productService.getProducts().subscribe({
      next: (products) => {
        this.products = products;
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
    });
  }

  loadDiscounts(): void {
    this.discountService.getDiscounts().subscribe({
      next: (discounts) => {
        this.discounts = discounts;
      },
      error: (error) => {
        console.error("Error loading discounts:", error);
      }
    });
  }

  addToCart(product: any): void {
    this.cartService.addToCart(product).subscribe(() => {
      this.loadCart();
    });
  }

  increaseQuantity(item: any): void {
    item.quantity += 1;
    this.cartService.updateCartItem(item.cartItemID, item).subscribe(() => {
      this.loadCart();
    });
  }

  decreaseQuantity(item: any): void {
    if (item.quantity > 1) {
      item.quantity -= 1;
      this.cartService.updateCartItem(item.cartItemID, item).subscribe(() => {
        this.loadCart();
      });
    }
  }

  deleteCartItem(id: number): void {
    this.cartService.deleteCartItem(id).subscribe(() => {
      this.loadCart();
    });
  }

  applyDiscount(): void {
    const discount = this.discounts.find(d => d.discountCode === this.discountCode);
    if (discount) {
      this.discountApplied = true;
      // Ensure discount percentage is used as a decimal
      this.discountAmount = this.totalValue * (discount.discountPercentage / 100);
      this.totalValueAfterDiscount = this.totalValue - this.discountAmount;
    } else {
      this.discountApplied = false;
      this.discountAmount = 0;
      this.totalValueAfterDiscount = this.totalValue;
    }
  }


  selectDiscount(discountCode: string): void {
    this.discountCode = discountCode;
    this.applyDiscount(); // Recalculate total value with the selected discount
  }

  calculateTotal(): void {
  // Sum up total value of cart items
  this.totalValue = this.cartItems.reduce((sum, item) => sum + (item.product.price * item.quantity), 0);

  // Calculate the total value after discount
  this.totalValueAfterDiscount = this.totalValue - this.discountAmount;
}
}
