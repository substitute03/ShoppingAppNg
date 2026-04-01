import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CreateProductRequest, Product } from '../models/product';

@Injectable({ providedIn: 'root' })
export class ProductsApiService {
  private readonly baseUrl = 'http://localhost:5254/api/products';
  private readonly adminHeaders = new HttpHeaders({ 'X-User-Role': 'admin' });

  constructor(private readonly http: HttpClient) {}

  getProducts(): Observable<Product[]> {
    return this.http.get<Product[]>(this.baseUrl);
  }

  createProduct(request: CreateProductRequest): Observable<Product> {
    return this.http.post<Product>(this.baseUrl, request, { headers: this.adminHeaders });
  }

  deleteProduct(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`, { headers: this.adminHeaders });
  }
}
