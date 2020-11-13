import { async, ComponentFixture, TestBed, fakeAsync, tick, discardPeriodicTasks } from '@angular/core/testing';
import { delay } from 'rxjs/operators';

import { ItemCardComponent } from 'src/app/components/component-item-card/item-card.component';
import { RouterTestingModule } from '@angular/router/testing';
import { FormsModule } from '@angular/forms';
import { AlertModule } from 'src/app/components/component-alert/alert.module';
import { LoadingBarHttpClientModule } from '@ngx-loading-bar/http-client';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { QuillModule } from 'ngx-quill';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { ApiItemCardService } from 'src/app/api-client/api-item-card.service';
import { ItemCardDto } from 'src/app/shared-dto/item-card-dto.model';
import { By } from '@angular/platform-browser';
import { of } from 'rxjs/internal/observable/of';

describe('ItemCardComponent', () => {
  let component: ItemCardComponent;
  let fixture: ComponentFixture<ItemCardComponent>;
  let apiItemCardService: ApiItemCardService;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ItemCardComponent],
      imports: [RouterTestingModule, HttpClientTestingModule, FormsModule, AlertModule, LoadingBarHttpClientModule, QuillModule.forRoot()],
      providers: [AuthenticationService, ApiItemCardService]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ItemCardComponent);
    component = fixture.componentInstance;
    apiItemCardService = TestBed.get(ApiItemCardService);
    var testData = new ItemCardDto();
    testData.headline = '123456';
    testData.commodityIdentifier = 'ABCD';
    spyOn(apiItemCardService, 'get').and.returnValue(of(testData).pipe(delay(1)));
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('item card is initialized in async mode', fakeAsync(() => {
    component.initialize(1);
    fixture.detectChanges();
    expect(component.itemCard.headline).toBe('');

    tick(1);
    fixture.detectChanges();
    expect(component.itemCard.headline).toBe('123456');
    expect(component.itemCardProdictSizeOverviewUrl).toBe('https://file.adler.info/pdf/size_chart/ABCD---product_size.pdf');
    discardPeriodicTasks();
  }));

  it('administrators form has right values', () => {
    component.isUserAdministrator = true;
    component.itemCard = new ItemCardDto();
    component.itemCard.quantity = 10;
    fixture.detectChanges();

    let element = fixture.debugElement.query(By.css('#itemCardQuantity')).nativeElement;
    expect(element.value).toBe('');
    fixture.whenStable().then(() => {
      expect(element.value).toBe(10);
    });
  });
});
