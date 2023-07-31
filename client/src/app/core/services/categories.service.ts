import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SettingsService } from '../settings/settings.service';
import { Category } from '../models/category';
import { Observable } from 'rxjs';
import { CategoryParams } from '../models/categoryParams';

@Injectable({
  providedIn: 'root',
})
export class CategoriesService {
  constructor(
    private http: HttpClient,
    private settingsService: SettingsService
  ) {}

  getCategories(categoryParams: CategoryParams): Observable<Category[]> {
    let params = new HttpParams();

    params = params.append('ParentCode', categoryParams.parentCode);

    return this.http.get<Category[]>(
      `${this.settingsService.baseEndpoint}/${this.settingsService.categoriesEndpoint}`,
      { params: params }
    );
  }
}
