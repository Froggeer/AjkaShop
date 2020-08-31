import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from 'src/environments/environment';
import { AuthenticationService } from 'src/app/services/authentication.service'

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

    constructor(private authenticationService: AuthenticationService) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const currentLogin = this.authenticationService.currentLoginValue;
        const isLoggedIn = currentLogin && currentLogin.accessToken;
        const isApiUrl = request.url.startsWith(environment.apiAjkaUrl);
        if (isLoggedIn && isApiUrl) {
            request = request.clone({
                setHeaders: {
                    Authorization: `Bearer ${currentLogin.accessToken}`
                }
            });
        }

        return next.handle(request);
    }
}
