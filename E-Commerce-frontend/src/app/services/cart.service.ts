import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private apiUrl = 'http://localhost:5182/cart'; // Replace with your API URL

  constructor(private http: HttpClient) { }

  getCartItems(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  addToCart(product: any): Observable<any> {
    return this.http.post<any>(this.apiUrl, product);
  }
}
