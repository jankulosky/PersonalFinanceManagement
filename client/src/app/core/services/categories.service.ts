import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SettingsService } from '../settings/settings.service';
import {
  getPaginatedResult,
  getPaginationHeaders,
} from '../helpers/paginationHelper';
import { FileParams } from '../models/fileParams';
import { Category } from '../models/category';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CategoriesService {
  constructor(
    private http: HttpClient,
    private settingsService: SettingsService
  ) {}

  getTransactions(fileParams: FileParams) {
    let params = getPaginationHeaders(
      fileParams.pageNumber,
      fileParams.pageSize
    );

    return getPaginatedResult<Category[]>(
      `${this.settingsService.baseEndpoint}/${this.settingsService.categoriesEndpoint}`,
      params,
      this.http
    ).pipe(
      map((response) => {
        return response;
      })
    );
  }
}
