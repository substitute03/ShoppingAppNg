import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CreateProductRequest, Product } from '../models/product';
import { UserRoleService } from './user-role.service';

@Injectable({ providedIn: 'root' })
export class ProductsApiService {
  private readonly baseUrl = 'http://localhost:5254/api/products';

  constructor(
    private readonly http: HttpClient,
    private readonly userRoleService: UserRoleService
  ) {}

  getProducts(): Observable<Product[]> {
    return this.http.get<Product[]>(this.baseUrl);
  }

  createProduct(request: CreateProductRequest): Observable<Product> {
    return this.http.post<Product>(this.baseUrl, request, { headers: this.getRoleHeaders() });
  }

  deleteProduct(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`, { headers: this.getRoleHeaders() });
  }

  private getRoleHeaders(): HttpHeaders {
    return new HttpHeaders({
      'X-User-Role': this.userRoleService.getRoleHeaderValue()
    });
  }
}
