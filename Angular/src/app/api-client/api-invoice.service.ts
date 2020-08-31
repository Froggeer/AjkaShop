import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CrudExtensionService } from '../api-client/crud-extension.service';
import { InvoiceDto } from 'src/app/shared-dto/invoice-dto.model';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiInvoiceService extends CrudExtensionService {

  constructor(protected http: HttpClient) {
    super(http, '/Invoice');
  }

  public getInvoiceDetail(id: number): Observable<InvoiceDto> {
    return this.http.get<InvoiceDto>(environment.apiAjkaUrl + '/Invoice/' + id + '/detail');
  }

  public getInvoicesFilter(objectsPerPage: number, pageNumber: number): Observable<InvoiceDto[]> {
    let postData = {
      'objectsPerPage': objectsPerPage,
      'pageNumber': pageNumber,
      'orderColumn': 'releaseDate'
    };
    return this.http.post<InvoiceDto[]>(environment.apiAjkaUrl + '/Invoice/filter', JSON.stringify(postData), this.httpOptions);
  }

  public getInvoicePdfUrl(id: number): string {
    return environment.apiAjkaUrl + '/Invoice/' + id + '/pdf-export';
  }
}

export enum InvoicePaymentMethod {
  Undefined = 0,

  /// <summary>
  /// Payment is made by bank transfer.
  /// </summary>
  BankTransfer = 1,

  /// <summary>
  /// Payment is made by cash in case of personal delivery.
  /// </summary>
  Cash = 2
}
