import { Component, OnInit, ComponentFactoryResolver, ViewChild, ViewContainerRef } from '@angular/core';
import { ApiCategoryService } from 'src/app/api-client/api-category.service';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { AlertService } from 'src/app/services/alert.service';
import { ContentItemCardsComponent } from 'src/app/components/component-content-item-cards/content-item-cards.component';
import { ContentSettingsComponent } from 'src/app/components/component-content-settings/content-settings.component';
import { ContentInvoicesComponent } from 'src/app/components/component-content-invoices/content-invoices.component';
import { ContentBasketComponent } from 'src/app/components/component-content-basket/content-basket.component';
import { CategoryDto } from 'src/app/shared-dto/category-dto.model';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

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
    private componentFactoryResolver: ComponentFactoryResolver) { }

  public ngOnInit(): void {
    this.showCategories();
  }

  public catchUserLoginState(loginState: boolean) {
    if (loginState) {
      this.loginCloseButton.nativeElement.click();
      this.isUserAdministrator = this.authenticationService.isUserAdministrator();
    }
    this.isUserLogged = loginState;
  }

  public showItemCards(categoryId: number) {
    if (categoryId == 0) {
      return;
    }
    const componentFactory = this.componentFactoryResolver.resolveComponentFactory(ContentItemCardsComponent);
    this.dynamicContent.clear();
    const componentRef = this.dynamicContent.createComponent(componentFactory);
    componentRef.instance.categoryId = categoryId;
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
}