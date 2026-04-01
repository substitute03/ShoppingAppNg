export interface Product {
  id: string;
  name: string;
  price: number;
}

export interface CreateProductRequest {
  name: string;
  price: number;
  idempotencyToken: string;
}
