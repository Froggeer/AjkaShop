import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { } from 'jasmine';

import { LoginFormComponent } from './login-form.component';
import { FormsModule } from '@angular/forms';
import { AlertModule } from '../component-alert/alert.module';
import { RouterTestingModule } from '@angular/router/testing';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { HttpClient, HttpHandler } from '@angular/common/http';

describe('LoginFormComponent', () => {
  let component: LoginFormComponent;
  let fixture: ComponentFixture<LoginFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [LoginFormComponent],
      imports: [RouterTestingModule, FormsModule, AlertModule],
      providers: [AuthenticationService, HttpClient, HttpHandler]
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
