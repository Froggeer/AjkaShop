import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { BasketHeaderDto } from 'src/app/shared-dto/basket-header-dto.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiOrderService {

  private httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  }

  constructor(private http: HttpClient) { }

  public createOrder(basketHeader: BasketHeaderDto): Observable<string> {
    return this.http.post<string>(environment.apiAjkaUrl + '/Order/create', JSON.stringify(basketHeader), this.httpOptions);
  }
}

export enum OrderState {
  Undefined = 0,

  /// <summary>
  /// Order is just created by customer and waits for administrator to process.
  /// </summary>
  Created = 1,

  /// <summary>
  /// The customer has received payment instructions and is awaiting payment.
  /// </summary>
  WaitingForPayment = 2,

  /// <summary>
  /// Order is successfully completed, bought goods has removed from offer.
  /// </summary>
  Closed = 3,

  /// <summary>
  /// Order is cancelled and nothing change.
  /// </summary>
  Cancellation = 4
}
