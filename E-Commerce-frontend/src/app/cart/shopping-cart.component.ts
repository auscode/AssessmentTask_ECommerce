import { Component, OnInit } from '@angular/core';
import { ProductService } from '../services/product.service';
import { CartService } from '../services/cart.service';
import { DiscountService } from '../services/discount.service';
import { SalesService } from '../services/sales.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

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
  discounts: any[] = [];
  discountCode: string = '';
  discountApplied: boolean = false;
  discountAmount: number = 0;
  totalValue: number = 0;
  totalValueAfterDiscount: number = 0;

  constructor(
    private productService: ProductService,
    private cartService: CartService,
    private discountService: DiscountService,
    private salesService: SalesService
  ) { }

  ngOnInit(): void {
    this.loadProducts();
    this.loadCart();
    this.loadDiscounts();
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
        this.applyDiscount();
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
      const discountPercentage = discount.discountPercentage / 100;
      this.discountAmount = this.totalValue * discountPercentage;
    } else {
      this.discountApplied = false;
      this.discountAmount = 0;
    }
    this.calculateTotal();
  }

  selectDiscount(discountCode: string): void {
    this.discountCode = discountCode;
    this.applyDiscount();
  }

  calculateTotal(): void {
    this.totalValue = this.cartItems.reduce((sum, item) => sum + (item.product.price * item.quantity), 0);

    if (this.discountApplied) {
      this.totalValueAfterDiscount = this.totalValue - this.discountAmount;
    } else {
      this.totalValueAfterDiscount = this.totalValue;
    }
  }

  buyAll(): void {
    const salesData = this.cartItems.map(item => ({
      saleID: 0, // Replace with appropriate logic for saleID
      productID: item.product.productID,
      quantity: item.quantity,
      price: item.product.price
    }));

    this.salesService.createSalesReport(salesData).subscribe({
      next: (response) => {
        console.log("Sales report created successfully:", response);
        this.cartItems = []; // Clear the cart after successful sale
        this.calculateTotal(); // Update totals
      },
      error: (error) => {
        console.error("Error creating sales report:", error);
      }
    });
  }
}
