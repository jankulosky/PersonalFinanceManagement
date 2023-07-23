import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FileParams } from '../models/fileParams';
import {
  getPaginatedResult,
  getPaginationHeaders,
} from '../helpers/paginationHelper';
import { Transaction } from '../models/transaction';
import { SettingsService } from '../settings/settings.service';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class TransactionsService {
  constructor(
    private http: HttpClient,
    private settingsService: SettingsService
  ) {}

  getTransactions(fileParams: FileParams) {
    let params = getPaginationHeaders(
      fileParams.pageNumber,
      fileParams.pageSize
    );

    params = params.append('kind', fileParams.kind);
    params = params.append('startDate', fileParams.startDate);
    params = params.append('endDate', fileParams.endDate);
    params = params.append('sortBy', fileParams.sortBy);
    params = params.append('sortOrder', fileParams.sortOrder);

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
}
