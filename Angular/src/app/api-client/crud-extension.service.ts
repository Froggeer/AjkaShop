import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';

export class CrudExtensionService {

  private apiNamespace: string;

  protected httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  }

  constructor(protected http: HttpClient,
    protected childApiNamespace: string) {
    this.apiNamespace = childApiNamespace;
  }

  public get(id: number): Observable<any> {
    return this.http.get<any>(environment.apiAjkaUrl + this.apiNamespace + '/' + id);
  }

  public update(entity: any): Observable<number> {
    if (entity.id == 0) {
      return null;
    }
    return this.http.put<number>(environment.apiAjkaUrl + this.apiNamespace, JSON.stringify(entity), this.httpOptions);
  }

  public insert(entity: any): Observable<number> {
    if (entity.id > 0) {
      return null;
    }
    return this.http.post<number>(environment.apiAjkaUrl + this.apiNamespace, JSON.stringify(entity), this.httpOptions);
  }

  public delete(id: number): Observable<any> {
    if (id == 0) {
      return null;
    }
    return this.http.delete<any>(environment.apiAjkaUrl + this.apiNamespace + '/' + id, this.httpOptions);
  }
}
