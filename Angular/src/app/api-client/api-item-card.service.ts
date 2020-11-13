import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CrudExtensionService } from '../api-client/crud-extension.service';
import { ItemCardDto } from 'src/app/shared-dto/item-card-dto.model';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiItemCardService extends CrudExtensionService {

  constructor(protected http: HttpClient) {
    super(http, '/ItemCard');
  }

  public getItemCardsFilter(objectsPerPage: number, pageNumber: number): Observable<ItemCardDto[]> {
    let postData = {
      'objectsPerPage': objectsPerPage,
      'pageNumber': pageNumber,
      'orderColumn': 'Headline',
      "isDescendingOrder": false
    };
    return this.http.post<ItemCardDto[]>(environment.apiAjkaUrl + '/ItemCard/filter', JSON.stringify(postData), this.httpOptions);
  }

  public getItemCardsOverview(categoryId: number, isUserAdministrator: boolean): Observable<ItemCardDto[]> {
    if (isUserAdministrator) {
      return this.http.get<ItemCardDto[]>(environment.apiAjkaUrl + '/ItemCard/category-id/' + categoryId + '/administrator');
    }
    return this.http.get<ItemCardDto[]>(environment.apiAjkaUrl + '/ItemCard/category-id/' + categoryId + '/for-sale');
  }

  public getItemCardsByKeyWord(keyWord: string): Observable<ItemCardDto[]> {
    return this.http.get<ItemCardDto[]>(environment.apiAjkaUrl + '/ItemCard/key-word/' + keyWord + '/search');
  }

  public uploadThumbnailImage(id: number, image: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', image);
    return this.http.post<any>(environment.apiAjkaUrl + '/ItemCard/' + id + '/upload-thumbnail-image', formData);
  }

  public getImage(imageUrl: string): Observable<Blob> {
    return this.http.get(imageUrl, { responseType: "blob" });
  }
}

export enum ItemCardState {
  Undefined = 0,

  /// <summary>
  /// The goods is available for sale to customers.
  /// </summary>
  ForSale = 1,

  /// <summary>
  /// The goods is sold and will be never offered for sale again.
  /// </summary>
  Sold = 2,

  /// <summary>
  /// Item is hidden from eshop offer, but can be anytime returned to sale.
  /// </summary>
  Inactive = 3
}
