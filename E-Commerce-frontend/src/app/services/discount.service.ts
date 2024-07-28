import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DiscountService {
  private apiUrl = 'http://localhost:5182/Discount'; // Adjust URL if necessary

  constructor(private http: HttpClient) { }

  getDiscounts(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }
}
