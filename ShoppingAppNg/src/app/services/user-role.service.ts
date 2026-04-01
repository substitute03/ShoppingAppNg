import { Injectable, signal } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class UserRoleService {
  private static readonly storageKey = 'shopping-app-is-admin';

  readonly isAdmin = signal<boolean>(this.getInitialRole());

  setIsAdmin(value: boolean): void {
    this.isAdmin.set(value);
    localStorage.setItem(UserRoleService.storageKey, String(value));
  }

  getRoleHeaderValue(): 'admin' | 'user' {
    return this.isAdmin() ? 'admin' : 'user';
  }

  private getInitialRole(): boolean {
    const storedValue = localStorage.getItem(UserRoleService.storageKey);
    return storedValue === 'true';
  }
}
