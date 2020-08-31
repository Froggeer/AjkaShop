import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CrudExtensionService } from '../api-client/crud-extension.service';

@Injectable({
  providedIn: 'root'
})
export class ApiInvoiceItemService extends CrudExtensionService {

  constructor(protected http: HttpClient) {
    super(http, '/InvoiceItem');
  }
}
