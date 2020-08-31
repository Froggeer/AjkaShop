import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { ItemCardSizePriceDto } from 'src/app/shared-dto/item-card-size-price-dto.model';

@Injectable({
  providedIn: 'root'
})
export class ApiItemCardSizePriceService {

  constructor(private http: HttpClient) { }

  public get(itemCardId: number): Observable<ItemCardSizePriceDto[]> {
    return this.http.get<ItemCardSizePriceDto[]>(environment.apiAjkaUrl + '/ItemCardSizePrice/item-card-id/' + itemCardId);
  }
}
