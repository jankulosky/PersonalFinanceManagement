import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class SettingsService {
  private baseUrl = 'http://localhost:5001/api';

  get baseEndpoint(): string {
    return this.baseUrl;
  }

  get transactionsEndpoint(): string {
    return 'Transactions';
  }

  get categoriesEndpoint(): string {
    return 'Categories';
  }
}
