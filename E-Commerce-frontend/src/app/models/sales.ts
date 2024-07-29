
export interface SalesItem {
  productID: String;
  saleID: String;
  quantity: number;
  price: number;
}

export interface SalesReport {
  salesItems: SalesItem[];
}
