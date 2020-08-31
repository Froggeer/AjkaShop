import { TestBed, async, ComponentFixture } from '@angular/core/testing';
import { LoginFormComponent } from './components/component-login-form/login-form.component';
import { AppComponent } from './app.component';
import { RouterTestingModule } from '@angular/router/testing';
import { FontAwesomeTestingModule } from '@fortawesome/angular-fontawesome/testing';
import { LoadingBarHttpClientModule } from '@ngx-loading-bar/http-client';
import { AlertModule } from './components/component-alert/alert.module';
import { FormsModule }   from '@angular/forms';
import { ApiCategoryService } from './api-client/api-category.service';
import { HttpClient, HttpHandler } from '@angular/common/http';

describe('AppComponent', () => {
  let component: AppComponent;
  let fixture: ComponentFixture<AppComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [AppComponent, LoginFormComponent],
      imports: [RouterTestingModule, FontAwesomeTestingModule, LoadingBarHttpClientModule, AlertModule, FormsModule],
      providers: [ApiCategoryService, HttpClient, HttpHandler] 
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AppComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

 /* it('should create the app', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });

  it(`should have as title 'ajka-shop-front-end'`, () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    expect(app.title).toEqual('ajka-shop-front-end');
  });

  it('should render title', () => {
    const fixture = TestBed.createComponent(AppComponent);
    fixture.detectChanges();
    const compiled = fixture.nativeElement;
    expect(compiled.querySelector('.content span').textContent).toContain('ajka-shop-front-end app is running!');
  });*/
});
