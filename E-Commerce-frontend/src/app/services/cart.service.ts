import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private apiUrl = 'http://localhost:5182/Cart'; // Replace with your API URL

  constructor(private http: HttpClient) { }

  // Get all cart items
  getCartItems(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  // Add a product to the cart
  addToCart(product: any): Observable<any> {
    return this.http.post<any>(this.apiUrl, {
      cartItemID: 0, // Assuming cartItemID is auto-generated
      productID: product.productID,
      quantity: 1, // Default quantity, adjust as needed
      product: product
    });
  }

  // Get a specific cart item by ID
  getCartItem(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }

  // Update a cart item
  updateCartItem(id: number, item: any): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/${id}`, item);
  }

  // Delete a cart item
  deleteCartItem(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${id}`);
  }

  // Calculate cart summary
  calculateCart(): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/calculate`, {});
  }
}
