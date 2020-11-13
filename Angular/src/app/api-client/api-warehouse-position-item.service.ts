import { Injectable } from '@angular/core';
import { CrudExtensionService } from './crud-extension.service';
import { WarehousePositionItemOverviewDto } from '../shared-dto/warehouse-position-item-overview-dto.model';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiWarehousePositionItemService extends CrudExtensionService {

  constructor(protected http: HttpClient) {
    super(http, '/WarehousePositionItem');
  }

  public getWarehousePositionItems(): Observable<WarehousePositionItemOverviewDto[]> {
    return this.http.get<WarehousePositionItemOverviewDto[]>(environment.apiAjkaUrl + '/WarehousePositionItem/overview');
  }

  public getWarehousePositionItemCards(): Observable<WarehousePositionItemOverviewDto[]> {
    return this.http.get<WarehousePositionItemOverviewDto[]>(environment.apiAjkaUrl + '/WarehousePositionItem/item-cards-overview');
  }
}
