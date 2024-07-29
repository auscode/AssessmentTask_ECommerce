import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { SalesItem } from '../models/sales';

@Injectable({
  providedIn: 'root'
})
export class SalesService {
  private apiUrl = 'http://localhost:5182/Sales';

  constructor(private http: HttpClient) { }

  createSalesReport(salesData: any[]): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/report`, salesData);
  }

  getTotalRevenue(): Observable<number> {
    return this.http.get<number>(`${this.apiUrl}/total-revenue`);
  }

  getMostSoldProducts(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/most-sold-products`);
  }
   getAllSalesItems(): Observable<SalesItem[]> {
    return this.http.get<SalesItem[]>(`${this.apiUrl}/all-sales-items`);
  }
}
