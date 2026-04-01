import { CommonModule, CurrencyPipe } from '@angular/common';
import { Component, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { CreateProductRequest, Product } from '../../models/product';
import { ProductsApiService } from '../../services/products-api.service';

type ProductRow = Product & { isPending?: boolean };

@Component({
  selector: 'app-products',
  imports: [CommonModule, FormsModule, RouterLink, CurrencyPipe],
  templateUrl: './products.component.html'
})
export class ProductsComponent implements OnInit {
  products = signal<ProductRow[]>([]);
  isLoading = signal(false);
  isSubmitting = signal(false);
  errorMessage = signal('');

  newProductName = '';
  newProductPrice = 0;

  constructor(private readonly productsApi: ProductsApiService) {}

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.productsApi.getProducts().subscribe({
        next: (products) => {
          this.products.set([...products]);
          this.isLoading.set(false);
          console.log('products:', this.products(), 'isLoading:', this.isLoading());
        },
        error: () => {
          this.errorMessage.set('Could not load products from the API.');
          this.isLoading.set(false);
        }
      });
  }

  addProduct(): void {
    const name = this.newProductName.trim();

    if (!name || this.newProductPrice <= 0) {
      this.errorMessage.set('Enter a product name and a price greater than zero.');
      return;
    }

    this.errorMessage.set('');
    this.isSubmitting.set(true);

    const tempId = crypto.randomUUID();
    const idempotencyToken = crypto.randomUUID();

    const optimisticProduct: ProductRow = {
      id: tempId,
      name,
      price: this.newProductPrice,
      isPending: true
    };

    this.products.set([optimisticProduct, ...this.products()]);
    this.newProductName = '';
    this.newProductPrice = 0;

    const request: CreateProductRequest = {
      name,
      price: optimisticProduct.price,
      idempotencyToken
    };

    this.productsApi.createProduct(request).subscribe({
      next: (createdProduct) => {
        this.products.set(this.products().map((product) =>
          product.id === tempId ? createdProduct : product
        ));
        this.isSubmitting.set(false);
      },
      error: () => {
        this.products.set(this.products().filter((product) => product.id !== tempId));
        this.errorMessage.set('Create failed. Product was removed from the list.');
        this.isSubmitting.set(false);
      }
    });
  }

  deleteProduct(product: ProductRow): void {
    const existingProducts = [...this.products()];
    this.products.set(this.products().filter((p) => p.id !== product.id));

    this.productsApi.deleteProduct(product.id).subscribe({
      error: () => {
        this.products.set(existingProducts);
        this.errorMessage.set('Delete failed. Product was restored to the list.');
      }
    });
  }
}
