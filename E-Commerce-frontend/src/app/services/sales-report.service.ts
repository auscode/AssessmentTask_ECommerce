import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SalesReportService {
  private apiUrl = 'https://your-api-endpoint/api/sales-report'; // Replace with your actual API endpoint

  constructor(private http: HttpClient) { }

  getDailySalesReport(): Observable<any> {
    return this.http.get<any>(this.apiUrl);
  }
}
