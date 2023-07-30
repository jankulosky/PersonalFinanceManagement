import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SettingsService } from '../settings/settings.service';
import {
  getPaginatedResult,
  getPaginationHeaders,
} from '../helpers/paginationHelper';
import { Category } from '../models/category';
import { map } from 'rxjs';
import { CategoryParams } from '../models/categoryParams';

@Injectable({
  providedIn: 'root',
})
export class CategoriesService {
  constructor(
    private http: HttpClient,
    private settingsService: SettingsService
  ) {}

  getTransactions(categoryParams: CategoryParams) {
    let params = getPaginationHeaders(
      categoryParams.pageNumber,
      categoryParams.pageSize
    );

    params = params.append('ParentCode', categoryParams.parentcode);

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
