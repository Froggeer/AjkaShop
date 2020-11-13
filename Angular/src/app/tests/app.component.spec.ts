import { TestBed, async, ComponentFixture } from '@angular/core/testing';
import { LoginFormComponent } from 'src/app/components/component-login-form/login-form.component';
import { CategoryComponent } from '../components/component-category/category.component';
import { AppComponent } from 'src/app/app.component';
import { RouterTestingModule } from '@angular/router/testing';
import { FontAwesomeTestingModule } from '@fortawesome/angular-fontawesome/testing';
import { LoadingBarHttpClientModule } from '@ngx-loading-bar/http-client';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { AlertModule } from 'src/app/components/component-alert/alert.module';
import { FormsModule } from '@angular/forms';
import { ApiCategoryService } from 'src/app/api-client/api-category.service';
import { ContentSettingsComponent } from '../components/component-content-settings/content-settings.component';
import { ContentInvoicesComponent } from '../components/component-content-invoices/content-invoices.component';
import { ContentBasketComponent } from '../components/component-content-basket/content-basket.component';
import { ContentInfoComponent } from '../components/component-content-info/content-info.component';
import { of } from 'rxjs';
import { CategoryDto } from '../shared-dto/category-dto.model';

describe('AppComponent', () => {
  let component: AppComponent;
  let fixture: ComponentFixture<AppComponent>;
  let apiCategoryService: ApiCategoryService;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [AppComponent, LoginFormComponent, CategoryComponent, ContentSettingsComponent,
        ContentInvoicesComponent, ContentBasketComponent, ContentInfoComponent],
      imports: [RouterTestingModule, HttpClientTestingModule, FontAwesomeTestingModule,
        LoadingBarHttpClientModule, AlertModule, FormsModule],
      providers: [ApiCategoryService]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AppComponent);
    component = fixture.componentInstance;
    apiCategoryService = TestBed.get(ApiCategoryService);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('list of categories is shown', () => {
    var testData: CategoryDto[] = [
      { 'id': 1, 'description': 'ABCD', 'isValid': true },
      { 'id': 2, 'description': 'EFGH', 'isValid': true }];
    spyOn(apiCategoryService, 'getCategoriesValid').and.returnValue(of(testData));
    component.showCategories();
    fixture.detectChanges();
    expect(fixture.nativeElement.querySelector('app-category')).not.toBeNull();
  });
});
