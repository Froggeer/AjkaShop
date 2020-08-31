import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { AuthDetailDto } from 'src/app/shared-dto/auth-detail-dto.model';

@Injectable({
  providedIn: 'root'
})
export class ApiAuthService {

  constructor(private http: HttpClient) { }

  public async authentication(username: string, password: string): Promise<AuthDetailDto> {
    const httpOptions = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    }
    let postData = {
      'username': username,
      'password': password
    };
    return await this.http.post<AuthDetailDto>(environment.apiAjkaUrl + '/Auth/Login', JSON.stringify(postData), httpOptions).toPromise();
  }
}
