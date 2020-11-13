import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http'
import { AppRoutingModule } from './app-routing.module';
import { FontAwesomeModule, FaIconLibrary } from '@fortawesome/angular-fontawesome';
import { LazyLoadImageModule } from 'ng-lazyload-image';
import { QuillModule } from 'ngx-quill';
import { AlertModule } from './components/component-alert/alert.module';
import { ConfirmationPopoverModule } from 'angular-confirmation-popover';
import { LoadingBarHttpClientModule } from '@ngx-loading-bar/http-client';

import { AppComponent } from './app.component';
import { LoginFormComponent } from './components/component-login-form/login-form.component';
import { ItemCardComponent } from './components/component-item-card/item-card.component';
import { CategoryComponent } from './components/component-category/category.component';
import { ContentItemCardsComponent } from './components/component-content-item-cards/content-item-cards.component';
import { ContentSettingsComponent } from './components/component-content-settings/content-settings.component';
import { ContentBasketComponent } from './components/component-content-basket/content-basket.component';
import { ContentInvoicesComponent } from './components/component-content-invoices/content-invoices.component';
import { InvoiceItemComponent } from './components/component-invoice-item/invoice-item.component';
import { ContentInfoComponent } from './components/component-content-info/content-info.component';
import { ManagementWarehousePositionComponent } from './components/component-management-warehouse-position/management-warehouse-position.component';
import { ManagementUserComponent } from './components/component-management-user/management-user.component';
import { ContentWarehousePositionsComponent } from './components/component-content-warehouse-positions/content-warehouse-positions.component';
import { ContentOrdersComponent } from './components/component-content-orders/content-orders.component';

import { faSearch, faUser, faCog, faTrashAlt, faEdit, faShoppingBasket, faFilePdf, faColumns, faShoppingCart } from '@fortawesome/free-solid-svg-icons';
import { HttpErrorInterceptor } from './interceptors/http-interceptor';
import { JwtInterceptor } from './interceptors/jwt-interceptor';

@NgModule({
  declarations: [
    AppComponent,
    LoginFormComponent,
    ItemCardComponent,
    CategoryComponent,
    ContentItemCardsComponent,
    ContentSettingsComponent,
    ContentBasketComponent,
    ContentInvoicesComponent,
    InvoiceItemComponent,
    ContentInfoComponent,
    ManagementWarehousePositionComponent,
    ManagementUserComponent,
    ContentWarehousePositionsComponent,
    ContentOrdersComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FontAwesomeModule,
    LazyLoadImageModule,
    AlertModule,
    FormsModule,
    LoadingBarHttpClientModule,
    QuillModule.forRoot(),
    ConfirmationPopoverModule.forRoot({
      confirmButtonType: 'danger',
      cancelText: 'Ne',
      confirmText: 'Ano'
    })
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: HttpErrorInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
  constructor(library: FaIconLibrary) {
    library.addIcons(faSearch, faUser, faCog, faTrashAlt, faEdit, faShoppingBasket, faFilePdf, faColumns, faShoppingCart);
  }
}
