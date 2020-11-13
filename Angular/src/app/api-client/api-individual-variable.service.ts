import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CrudExtensionService } from '../api-client/crud-extension.service';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { IndividualVariableDto } from '../shared-dto/individual-variable-dto.model';

@Injectable({
  providedIn: 'root'
})
export class ApiIndividualVariableService extends CrudExtensionService {

  constructor(protected http: HttpClient) {
    super(http, '/IndividualVariable');
  }

  public getIndividualVariable(keyName: string): Observable<IndividualVariableDto> {
    return this.http.get<IndividualVariableDto>(environment.apiAjkaUrl + '/IndividualVariable/key-name/' + keyName);
  }
}
