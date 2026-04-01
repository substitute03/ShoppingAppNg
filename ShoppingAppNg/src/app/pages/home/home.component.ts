import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { UserRoleService } from '../../services/user-role.service';

@Component({
  selector: 'app-home',
  imports: [RouterLink, FormsModule],
  templateUrl: './home.component.html'
})
export class HomeComponent {
  constructor(public readonly userRoleService: UserRoleService) {}

  onRoleChanged(isAdmin: boolean): void {
    this.userRoleService.setIsAdmin(isAdmin);
  }
}
