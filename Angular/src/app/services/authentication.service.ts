import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { ApiAuthService } from 'src/app/api-client/api-auth.service';
import { AuthDetailDto } from 'src/app/shared-dto/auth-detail-dto.model';
import { GlobalConstants } from 'src/app/common/global-constants';
import * as jwt_decode from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  private currentUserSubject: BehaviorSubject<AuthDetailDto>;
  private authDetail: AuthDetailDto;

  constructor(public apiAuthService: ApiAuthService) {
    this.currentUserSubject = new BehaviorSubject<AuthDetailDto>(JSON.parse(localStorage.getItem(GlobalConstants.storageCurrentUser)));
  }

  public get currentLoginValue(): AuthDetailDto {
    return this.currentUserSubject.value;
  }

  public isUserAdministrator(): boolean {
    if (this.authDetail?.role?.role == GlobalConstants.userRoleAdministrator) {
      return true;
    }
    return false;
  }

  public async authentication(username: string, password: string): Promise<AuthDetailDto> {
    await this.apiAuthService.authentication(username, password).then(data => {
      this.authDetail = data;
      if (this.authDetail.errorMessage?.length > 1) {
        this.logout();
        return this.authDetail;
      }
      if (this.authDetail != undefined) {
        this.authDetail.role = jwt_decode(this.authDetail.accessToken);
        localStorage.setItem(GlobalConstants.storageCurrentUser, JSON.stringify(this.authDetail));
        this.currentUserSubject = new BehaviorSubject<AuthDetailDto>(this.authDetail);
      } else {
        this.logout();
      }
    });
    return this.authDetail;
  }

  public logout() {
    localStorage.removeItem(GlobalConstants.storageCurrentUser);
    this.currentUserSubject.next(null);
  }
}
