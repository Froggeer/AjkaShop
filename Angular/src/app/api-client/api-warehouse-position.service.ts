import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CrudExtensionService } from 'src/app/api-client/crud-extension.service';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { WarehousePositionDto } from '../shared-dto/warehouse-position-dto.model';

@Injectable({
  providedIn: 'root'
})
export class ApiWarehousePositionService extends CrudExtensionService {

  constructor(protected http: HttpClient) {
    super(http, '/WarehousePosition');
  }

  public getWarehousePositionFilter(objectsPerPage: number, pageNumber: number): Observable<WarehousePositionDto[]> {
    let postData = {
      'objectsPerPage': objectsPerPage,
      'pageNumber': pageNumber,
      'orderColumn': 'Code',
      'isDescendingOrder': true
    };
    return this.http.post<WarehousePositionDto[]>(environment.apiAjkaUrl + '/WarehousePosition/filter', JSON.stringify(postData), this.httpOptions);
  }
}
