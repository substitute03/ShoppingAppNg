import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { UserRoleService } from './services/user-role.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, RouterLink, RouterLinkActive, FormsModule],
  templateUrl: './app.html'
})
export class App {
  constructor(public readonly userRoleService: UserRoleService) {}

  onRoleChanged(isAdmin: boolean): void {
    this.userRoleService.setIsAdmin(isAdmin);
  }
}
