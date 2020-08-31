import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CrudExtensionService } from 'src/app/api-client/crud-extension.service';
import { UserDto } from 'src/app/shared-dto/user-dto.model';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiUserService extends CrudExtensionService {

  constructor(protected http: HttpClient) {
    super(http, '/User');
  }

  public getUsersValid(): Observable<UserDto[]> {
    return this.http.get<UserDto[]>(environment.apiAjkaUrl + '/User/valid');
  }

  public getUsersFilter(): Observable<UserDto[]> {
    let postData = {
      'objectsPerPage': 0,
      'pageNumber': 0
    };
    return this.http.post<UserDto[]>(environment.apiAjkaUrl + '/User/filter', JSON.stringify(postData), this.httpOptions);
  }
}
