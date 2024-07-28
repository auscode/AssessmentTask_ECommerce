import { Component, OnInit } from '@angular/core';
import { SalesReportService } from '../services/sales-report.service';

@Component({
  selector: 'app-sales-report',
  templateUrl: './sales-report.component.html',
  styleUrls: ['./sales-report.component.css']
})
export class SalesReportComponent implements OnInit {
  totalRevenue: number = 0;
  mostSoldProducts: any[] = [];
  productsSold: any[] = [];
  productQuantities: any[] = [];
  revenueFromProducts: any[] = [];

  constructor(private salesReportService: SalesReportService) { }

  ngOnInit(): void {
    this.getSalesReport();
  }

  getSalesReport(): void {
    this.salesReportService.getDailySalesReport().subscribe(report => {
      this.totalRevenue = report.totalRevenue;
      this.mostSoldProducts = report.mostSoldProducts;
      this.productsSold = report.productsSold;
      this.productQuantities = report.productQuantities;
      this.revenueFromProducts = report.revenueFromProducts;
    });
  }
}
