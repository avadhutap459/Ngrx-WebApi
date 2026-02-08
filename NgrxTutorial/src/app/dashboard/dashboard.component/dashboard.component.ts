import { Component, inject } from '@angular/core';
import { UserService } from '../../sign-in/services/user.service';
import { Store } from '@ngrx/store';
import { selectToken } from '../../sign-in/store/auth.reducer';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-dashboard.component',
  standalone: false,
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css',
})
export class DashboardComponent {

    private store = inject(Store);

  // ✅ Token Observable
  token$: Observable<string | null> = this.store.select(selectToken);

  constructor(private authService: UserService) {}

  ngOnInit() {
    debugger
  }

  logout() {
    this.authService.logout();
  }
}
