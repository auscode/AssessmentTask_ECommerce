import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SalesReport } from '../models/sales'; 
import { SalesService } from '../services/sales.service'; 
import { SalesItem } from '../models/sales'; 

@Component({
  selector: 'app-sales-report',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './sales-report.component.html',
  styleUrls: ['./sales-report.component.css']
})
export class SalesReportComponent implements OnInit {
  totalRevenue: number = 0;
  mostSoldProducts: any[] = [];
  productsSold: any[] = [];
  quantitiesSold: number[] = [];
  salesItems: SalesItem[] = [];



  constructor(private salesService: SalesService) {}

  ngOnInit(): void {

     this.loadSalesItems();
  }

  loadSalesItems(): void {
      this.salesService.getAllSalesItems().subscribe({
        next: (items) => {
          this.salesItems = items;
        },
        error: (error) => {
          console.error('Error fetching sales items:', error);
        }
      });
    }
}
