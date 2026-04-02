import { CommonModule, CurrencyPipe, DatePipe } from '@angular/common';
import { Component, OnInit, signal } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Product } from '../../models/product';
import { CreateOrderRequest, Order } from '../../models/order';
import { OrdersApiService } from '../../services/orders-api.service';
import { ProductsApiService } from '../../services/products-api.service';

@Component({
  selector: 'app-orders',
  imports: [CommonModule, ReactiveFormsModule, DatePipe, CurrencyPipe],
  templateUrl: './orders.component.html'
})
export class OrdersComponent implements OnInit {
  readonly products = signal<Product[]>([]);
  readonly isLoadingProducts = signal(false);
  readonly isCreating = signal(false);
  readonly isRetrieving = signal(false);
  readonly showIdempotencyResetToast = signal(false);
  readonly showCopiedOrderToast = signal(false);
  readonly createErrorMessage = signal('');
  readonly retrieveErrorMessage = signal('');
  readonly createdOrder = signal<Order | null>(null);
  readonly retrievedOrder = signal<Order | null>(null);
  readonly currentOrderIdempotencyToken = signal(crypto.randomUUID());
  readonly ordersActiveTab = signal<'create' | 'retrieve'>('create');

  readonly createOrderForm;
  readonly retrieveOrderForm;

  constructor(
    private readonly fb: FormBuilder,
    private readonly ordersApi: OrdersApiService,
    private readonly productsApi: ProductsApiService
  ) {
    this.createOrderForm = this.fb.group({
      forcePaymentFailure: this.fb.nonNullable.control(false),
      items: this.fb.array([this.createOrderFormBuilderItems()])
    });

    this.retrieveOrderForm = this.fb.group({
      orderId: this.fb.nonNullable.control('', [Validators.required])
    });
  }

  ngOnInit(): void {
    this.loadProducts();
  }

  get orderItemsFormArray(): FormArray {
    return this.createOrderForm.controls.items as FormArray;
  }

  isOrderLineFieldInvalid(lineIndex: number, fieldName: 'productId' | 'quantity'): boolean {
    const group = this.orderItemsFormArray.at(lineIndex) as FormGroup;
    const control = group.get(fieldName);
    return !!control && control.invalid && control.touched;
  }

  isRetrieveOrderIdInvalid(): boolean {
    const control = this.retrieveOrderForm.get('orderId');
    return !!control && control.invalid && control.touched;
  }

  addItem(): void {
    this.orderItemsFormArray.push(this.createOrderFormBuilderItems());
  }

  removeItem(index: number): void {
    if (this.orderItemsFormArray.length === 1) {
      return;
    }

    this.orderItemsFormArray.removeAt(index);
  }

  createOrder(): void {
    this.createErrorMessage.set('');
    this.createdOrder.set(null);

    if (this.createOrderForm.invalid) {
      this.createOrderForm.markAllAsTouched();
      return;
    }

    const items = this.orderItemsFormArray.controls
      .map((control) => ({
        productId: String(control.get('productId')?.value ?? ''),
        quantity: Number(control.get('quantity')?.value ?? 0)
      }))
      .filter((item) => item.productId && item.quantity > 0);

    if (items.length === 0) {
      this.createErrorMessage.set('Add at least one valid order item.');
      return;
    }

    const request: CreateOrderRequest = {
      items,
      forcePaymentFailure: this.createOrderForm.controls.forcePaymentFailure.value ?? false,
      idempotencyToken: this.currentOrderIdempotencyToken()
    };

    this.isCreating.set(true);
    this.ordersApi.createOrder(request).subscribe({
      next: (order) => {
        this.createdOrder.set(order);
        this.isCreating.set(false);
      },
      error: (error) => {
        this.createErrorMessage.set(error?.error.detail ?? 'Could not create order.');
        this.isCreating.set(false);
      }
    });
  }

  startNewOrder(): void {
    this.currentOrderIdempotencyToken.set(crypto.randomUUID());
    this.showIdempotencyResetToast.set(true);
    setTimeout(() => this.showIdempotencyResetToast.set(false), 2500);
  }

  copyCreatedOrderId(): void {
    const id = this.createdOrder()?.id;
    if (!id) {
      return;
    }
    void navigator.clipboard.writeText(id).then(() => {
      this.showCopiedOrderToast.set(true);
      setTimeout(() => this.showCopiedOrderToast.set(false), 2500);
    });
  }

  retrieveOrder(): void {
    this.retrieveErrorMessage.set('');
    this.retrievedOrder.set(null);

    if (this.retrieveOrderForm.invalid) {
      this.retrieveOrderForm.markAllAsTouched();
      return;
    }

    const orderId = this.retrieveOrderForm.controls.orderId.value.trim();
    if (!orderId) {
      return;
    }

    this.isRetrieving.set(true);
    this.ordersApi.getOrderById(orderId).subscribe({
      next: (order) => {
        this.retrievedOrder.set(order);
        this.isRetrieving.set(false);
      },
      error: (error) => {
        if (error?.status === 404) {
          this.retrieveErrorMessage.set('Order not found.');
        } else {
          this.retrieveErrorMessage.set(error?.error ?? 'Could not retrieve order.');
        }
        this.isRetrieving.set(false);
      }
    });
  }

  private loadProducts(): void {
    this.isLoadingProducts.set(true);
    this.productsApi.getProducts().subscribe({
      next: (products) => {
        this.products.set(products);
        this.isLoadingProducts.set(false);
      },
      error: () => {
        this.isLoadingProducts.set(false);
        this.createErrorMessage.set('Could not load products for order creation.');
      }
    });
  }

  private createOrderFormBuilderItems() {
    return this.fb.group({
      productId: this.fb.nonNullable.control('', [Validators.required]),
      quantity: this.fb.nonNullable.control(1, [Validators.required, Validators.min(1)])
    });
  }

  getProductName(productId: string): string {
    return this.products().find((p) =>
        p.id === productId)?.name ?? `Unknown product`;
  }
}
