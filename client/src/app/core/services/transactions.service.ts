import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TransactionParams } from '../models/transactionParams';
import {
  getPaginatedResult,
  getPaginationHeaders,
} from '../helpers/paginationHelper';
import { Transaction } from '../models/transaction';
import { SettingsService } from '../settings/settings.service';
import { Observable, map } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class TransactionsService {
  constructor(
    private http: HttpClient,
    private settingsService: SettingsService
  ) {}

  getTransactions(transactionParams: TransactionParams) {
    let params = getPaginationHeaders(
      transactionParams.pageNumber,
      transactionParams.pageSize
    );

    params = params.append('TransactionKind', transactionParams.kind);
    if (transactionParams.startDate) {
      params = params.append(
        'startDate',
        transactionParams.startDate.toISOString().split('T')[0]
      );
    }

    if (transactionParams.endDate) {
      params = params.append(
        'endDate',
        transactionParams.endDate.toISOString().split('T')[0]
      );
    }
    params = params.append('sortBy', transactionParams.sortBy);
    params = params.append('sortOrder', transactionParams.sortOrder);

    return getPaginatedResult<Transaction[]>(
      `${this.settingsService.baseEndpoint}/${this.settingsService.transactionsEndpoint}`,
      params,
      this.http
    ).pipe(
      map((response) => {
        return response;
      })
    );
  }

  categorizeTransaction(
    transactionId: number,
    catCode: string
  ): Observable<Transaction> {
    const url = `${this.settingsService.baseEndpoint}/${this.settingsService.transactionsEndpoint}/${transactionId}/${this.settingsService.categorizeEndpoint}`;
    const body = { catCode: catCode };
    return this.http.post<Transaction>(url, body);
  }
}
