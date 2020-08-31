import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CrudExtensionService } from '../api-client/crud-extension.service';
import { environment } from 'src/environments/environment';
import { CategoryDto } from 'src/app/shared-dto/category-dto.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiCategoryService extends CrudExtensionService {

  constructor(protected http: HttpClient) {
    super(http, '/Category');
  }

  public getCategoriesValid(): Observable<CategoryDto[]> {
    return this.http.get<CategoryDto[]>(environment.apiAjkaUrl + '/Category/valid');
  }

  public getCategoriesFilter(objectsPerPage: number, pageNumber: number): Observable<CategoryDto[]> {
    let postData = {
      'objectsPerPage': objectsPerPage,
      'pageNumber': pageNumber
    };
    return this.http.post<CategoryDto[]>(environment.apiAjkaUrl + '/Category/filter', JSON.stringify(postData), this.httpOptions);
  }
}
