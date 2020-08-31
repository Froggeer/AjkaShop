import { Output, EventEmitter, Component, OnInit, ViewChild } from '@angular/core';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { AlertService } from 'src/app/services/alert.service';
import { LoginModel } from './login-model';

@Component({
  selector: 'app-login-form',
  templateUrl: './login-form.component.html',
  styleUrls: ['./login-form.component.scss']
})
export class LoginFormComponent {

  public model = new LoginModel('', '');
  public submitted = false;
  private alertOptions = {
    id: 'alert-login',
    autoClose: true,
    keepAfterRouteChange: false
  };

  @Output() userIsLoggedInEvent = new EventEmitter<boolean>();

  constructor(private authService: AuthenticationService,
    private alertService: AlertService) { }

  public onSubmit() {
    this.submitted = true;
  }

  public async login() {
    var authResult = await this.authService.authentication(this.model.email, this.model.password);
    if (authResult != undefined) {
      if (authResult.errorMessage?.length > 1) {
        this.alertService.error(authResult.errorMessage, this.alertOptions);
      } else {
        this.userIsLoggedInEvent.emit(true);
        return;
      }
    }
    this.userIsLoggedInEvent.emit(false);
  }
}
