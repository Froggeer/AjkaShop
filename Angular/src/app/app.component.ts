import { Component, OnInit, ComponentFactoryResolver, ViewChild, ViewContainerRef, AfterViewInit, ChangeDetectorRef } from '@angular/core';
import { ApiCategoryService } from 'src/app/api-client/api-category.service';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { AlertService } from 'src/app/services/alert.service';
import { ContentItemCardsComponent } from 'src/app/components/component-content-item-cards/content-item-cards.component';
import { ContentSettingsComponent } from 'src/app/components/component-content-settings/content-settings.component';
import { ContentInvoicesComponent } from 'src/app/components/component-content-invoices/content-invoices.component';
import { ContentBasketComponent } from 'src/app/components/component-content-basket/content-basket.component';
import { ContentInfoComponent } from './components/component-content-info/content-info.component';
import { ContentWarehousePositionsComponent } from './components/component-content-warehouse-positions/content-warehouse-positions.component';
import { ContentOrdersComponent } from './components/component-content-orders/content-orders.component';
import { CategoryDto } from 'src/app/shared-dto/category-dto.model';
import { GlobalConstants } from './common/global-constants';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, AfterViewInit {

  @ViewChild('loginCloseButton') loginCloseButton: { nativeElement: { click: () => void; }; };
  @ViewChild('dynamicContent', { read: ViewContainerRef }) dynamicContent: ViewContainerRef;

  public title = 'ajka-shop-front-end';
  public categories: CategoryDto[];
  public isUserLogged: boolean;
  // TODO login test (false)
  public isUserAdministrator: boolean = false;

  constructor(public apiCategoryService: ApiCategoryService,
    private authenticationService: AuthenticationService,
    private alertService: AlertService,
    private componentFactoryResolver: ComponentFactoryResolver,
    private changeDetectorRef: ChangeDetectorRef) { }

  public ngOnInit(): void {
    this.showCategories();
  }

  public ngAfterViewInit(): void {
    this.showIntroductoryInformation();
    this.changeDetectorRef.detectChanges();
  }

  public catchUserLoginState(loginState: boolean) {
    if (loginState) {
      this.loginCloseButton.nativeElement.click();
      this.isUserAdministrator = this.authenticationService.isUserAdministrator();
    }
    this.isUserLogged = loginState;
  }

  public showItemCards(categoryId: number, keyWord: string) {
    const componentFactory = this.componentFactoryResolver.resolveComponentFactory(ContentItemCardsComponent);
    this.dynamicContent.clear();
    const componentRef = this.dynamicContent.createComponent(componentFactory);
    componentRef.instance.categoryId = categoryId;
    componentRef.instance.keyWord = keyWord;
    componentRef.instance.isUserAdministrator = this.isUserAdministrator;
  }

  public showSettings() {
    const componentFactory = this.componentFactoryResolver.resolveComponentFactory(ContentSettingsComponent);
    this.dynamicContent.clear();
    const componentRef = this.dynamicContent.createComponent(componentFactory);
    componentRef.instance.isCategoryChangedEvent.subscribe((_sub: any) => this.showCategories());
  }

  public showInvoices() {
    const componentFactory = this.componentFactoryResolver.resolveComponentFactory(ContentInvoicesComponent);
    this.dynamicContent.clear();
    this.dynamicContent.createComponent(componentFactory);
  }

  public showBasket() {
    const componentFactory = this.componentFactoryResolver.resolveComponentFactory(ContentBasketComponent);
    this.dynamicContent.clear();
    this.dynamicContent.createComponent(componentFactory);
  }

  public logout() {
    this.authenticationService.logout();
    this.isUserLogged = false;
    this.isUserAdministrator = false;
    this.alertService.info("Úspěšně odhlášeno.", this.alertService.getStandardOption(true));
  }

  public showCategories() {
    this.apiCategoryService.getCategoriesValid().subscribe((data: CategoryDto[]) => this.categories = data,
      error => this.alertService.error("Chyba při čtení kategorií! " + error.message, this.alertService.getStandardOption(true)));
  }

  public showIntroductoryInformation() {
    const componentFactory = this.componentFactoryResolver.resolveComponentFactory(ContentInfoComponent);
    this.dynamicContent.clear();
    const componentRef = this.dynamicContent.createComponent(componentFactory);
    componentRef.instance.isUserAdministrator = this.isUserAdministrator;
    componentRef.instance.getContent(GlobalConstants.individualVariableIntroduction);
  }

  public showWarehousePositions() {
    const componentFactory = this.componentFactoryResolver.resolveComponentFactory(ContentWarehousePositionsComponent);
    this.dynamicContent.clear();
    this.dynamicContent.createComponent(componentFactory);
  }

  public showOrders() {
    const componentFactory = this.componentFactoryResolver.resolveComponentFactory(ContentOrdersComponent);
    this.dynamicContent.clear();
    this.dynamicContent.createComponent(componentFactory);
  }

  public search(input: string) {
    if (input.length < 2) {
      this.alertService.warn("Pro hledání musí být zadány alespoň dva znaky", this.alertService.getStandardOption(true));
      return;
    }
    this.showItemCards(0, input);
  }
}