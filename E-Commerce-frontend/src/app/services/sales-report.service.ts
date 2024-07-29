import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SalesReportService {
  private baseUrl = 'http://localhost:5182/salesreport';

  constructor(private http: HttpClient) { }

  getDailySalesReport(): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/daily`);
  }
}
