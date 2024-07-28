// src/app/product.ts
export interface Product {
  productID: number;
  productName: string;
  price: number;
  cartItems?: any[];   // Include these properties as optional arrays
  salesItems?: any[];
}

  