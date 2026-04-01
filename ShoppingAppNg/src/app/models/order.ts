export interface CreateOrderItemRequest {
  productId: string;
  quantity: number;
}

export interface CreateOrderRequest {
  items: CreateOrderItemRequest[];
  forcePaymentFailure: boolean;
  idempotencyToken: string;
}

export interface OrderItem {
  productId: string;
  quantity: number;
  unitPrice: number;
}

export interface Order {
  id: string;
  createdAtUtc: string;
  items: OrderItem[];
  paymentSucceeded: boolean;
  idempotencyToken: string;
}
