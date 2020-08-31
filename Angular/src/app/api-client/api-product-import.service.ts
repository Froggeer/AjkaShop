import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiProductImportService {

  constructor(private http: HttpClient) { }

  public importAdlerProducts(xmlFile: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', xmlFile);
    return this.http.post<any>(environment.apiAjkaUrl + '/ProductImport/import-adler-product-list', formData);
  }
}
