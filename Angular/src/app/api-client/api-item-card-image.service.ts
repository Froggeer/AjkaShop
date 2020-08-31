import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { ItemCardImageDto } from 'src/app/shared-dto/item-card-image-dto.model';

@Injectable({
  providedIn: 'root'
})
export class ApiItemCardImageService {

  private httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  }

  constructor(private http: HttpClient) { }

  public getItemCardImages(itemCardId: number): Observable<ItemCardImageDto[]> {
    return this.http.get<ItemCardImageDto[]>(environment.apiAjkaUrl + '/ItemCardImage/item-card-id/' + itemCardId);
  }

  public uploadImage(itemCardId: number, images: FileList): Observable<any> {
    const formData = new FormData();
    Array.from(images).forEach(x => formData.append('file', x));
    return this.http.post<any>(environment.apiAjkaUrl + '/ItemCardImage/' + itemCardId + '/upload-image', formData);
  }

  public delete(id: number): Observable<any> {
    if (id == 0) {
      return null;
    }
    return this.http.delete<any>(environment.apiAjkaUrl + '/ItemCardImage/' + id + '/delete-image', this.httpOptions);
  }
}
