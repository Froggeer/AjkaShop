import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { filter } from 'rxjs/operators';

import { AlertModel, AlertType } from 'src/app/components/component-alert/alert.model';

@Injectable({ providedIn: 'root' })
export class AlertService {
    private subject = new Subject<AlertModel>();
    private defaultId = 'default-alert';

    public getStandardOption(isTemporary: boolean) {
        return {
            autoClose: isTemporary,
            keepAfterRouteChange: false
        };
    };

    onAlert(id = this.defaultId): Observable<AlertModel> {
        return this.subject.asObservable().pipe(filter(x => x && x.id === id));
    }

    public success(message: string, options?: any) {
        this.alert(new AlertModel({ ...options, type: AlertType.Success, message }));
    }

    public error(message: string, options?: any) {
        this.alert(new AlertModel({ ...options, type: AlertType.Error, message }));
    }

    public info(message: string, options?: any) {
        this.alert(new AlertModel({ ...options, type: AlertType.Info, message }));
    }

    public warn(message: string, options?: any) {
        this.alert(new AlertModel({ ...options, type: AlertType.Warning, message }));
    }

    public alert(alert: AlertModel) {
        alert.id = alert.id || this.defaultId;
        this.subject.next(alert);
    }

    public clear(id = this.defaultId) {
        this.subject.next(new AlertModel({ id }));
    }
}
