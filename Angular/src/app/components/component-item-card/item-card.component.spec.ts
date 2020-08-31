import { async, ComponentFixture, TestBed, fakeAsync, tick, flush } from '@angular/core/testing';
import { of } from 'rxjs';
import { delay } from 'rxjs/operators';

import { ItemCardComponent } from './item-card.component';
import { RouterTestingModule } from '@angular/router/testing';
import { FormsModule } from '@angular/forms';
import { AlertModule } from '../component-alert/alert.module';
import { LoadingBarHttpClientModule } from '@ngx-loading-bar/http-client';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { ApiItemCardService } from 'src/app/api-client/api-item-card.service';
import { HttpClient, HttpHandler } from '@angular/common/http';
import { ItemCardDto } from 'src/app/shared-dto/item-card-dto.model';

describe('ItemCardComponent', () => {
  let component: ItemCardComponent;
  let fixture: ComponentFixture<ItemCardComponent>;
  let apiItemCardService: ApiItemCardService;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ItemCardComponent ],
      imports: [RouterTestingModule, FormsModule, AlertModule, LoadingBarHttpClientModule],
      providers: [AuthenticationService, ApiItemCardService, HttpClient, HttpHandler]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ItemCardComponent);
    component = fixture.componentInstance;
    apiItemCardService = TestBed.get(ApiItemCardService);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('item card is initialized in async mode', fakeAsync(() => {
    var testData = new ItemCardDto();
    testData.headline = '123456';
    spyOn(apiItemCardService, 'get').and.returnValue(of(testData).pipe(delay(1)));
    component.initialize(1);

    fixture.detectChanges();
    expect(component.itemCard.headline).toBe('');

    tick(1);
    fixture.detectChanges();
    expect(component.itemCard.headline).toBe(testData.headline);
    flush();
  }));
});
