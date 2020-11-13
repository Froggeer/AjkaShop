import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { } from 'jasmine';

import { LoginFormComponent } from 'src/app/components/component-login-form/login-form.component';
import { FormsModule } from '@angular/forms';
import { AlertModule } from 'src/app/components/component-alert/alert.module';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { AuthenticationService } from 'src/app/services/authentication.service';

describe('LoginFormComponent', () => {
  let component: LoginFormComponent;
  let fixture: ComponentFixture<LoginFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [LoginFormComponent],
      imports: [RouterTestingModule, HttpClientTestingModule, FormsModule, AlertModule],
      providers: [AuthenticationService]
    })
      .compileComponents().then(() => {
        fixture = TestBed.createComponent(LoginFormComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
      });
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
