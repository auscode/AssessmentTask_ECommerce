import { Component, OnInit } from '@angular/core';
import { Product } from '../models/product';
import { ProductService } from '../services/product.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent implements OnInit {
  products: Product[] = []; 
  selectedProduct: Product | null = null;
  isEditMode: boolean = false;
    newProduct: Product = { 
    productID: 0, 
    productName: '', 
    price: 0,
    cartItems: [],
    salesItems: []
  };

  constructor(private productService: ProductService) { }

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.productService.getProducts().subscribe(products => this.products = products);
  }

  deleteProduct(id: number): void {
    this.productService.deleteProduct(id).subscribe(() => this.loadProducts());
  }

 addProduct(): void {
    if (this.newProduct.productName && this.newProduct.price > 0) {
      const productToAdd: Product = {
        ...this.newProduct,
        cartItems: this.newProduct.cartItems || [],
        salesItems: this.newProduct.salesItems || []
      };
      this.productService.addProduct(productToAdd).subscribe(() => {
        this.loadProducts();
        this.newProduct = { 
          productID: 0, 
          productName: '', 
          price: 0,
          cartItems: [],   
          salesItems: []
        };
      });
    }
  }

  editProduct(product: Product): void {
    this.selectedProduct = { ...product };
    this.isEditMode = true;
  }

  updateProduct(): void {
    if (this.selectedProduct) {
      // Ensure cartItems and salesItems are included as empty arrays
      const updatedProduct: Product = {
        ...this.selectedProduct,
        cartItems: this.selectedProduct.cartItems || [],
        salesItems: this.selectedProduct.salesItems || []
      };
      this.productService.updateProduct(updatedProduct.productID, updatedProduct).subscribe({
        next: () => {
          this.loadProducts();
          this.selectedProduct = null;
          this.isEditMode = false;
        },
        error: (error) => {
          console.error('Error updating product:', error); // Log errors
        }
      });
    }
  }

  cancelEdit(): void {
    this.selectedProduct = null;
    this.isEditMode = false;
  }
}
