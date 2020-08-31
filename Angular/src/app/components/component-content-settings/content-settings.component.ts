import { Component, Output, EventEmitter } from '@angular/core';
import { ApiCategoryService } from 'src/app/api-client/api-category.service';
import { ApiUserService } from 'src/app/api-client/api-user.service';
import { ApiProductImportService } from 'src/app/api-client/api-product-import.service';
import { AlertService } from 'src/app/services/alert.service';
import { CategoryDto } from 'src/app/shared-dto/category-dto.model';
import { UserDto } from 'src/app/shared-dto/user-dto.model';

export enum ScreenIdentifier {
  none, users, categories, userForm, categoryForm, importAdler
}

@Component({
  selector: 'app-content-settings',
  templateUrl: './content-settings.component.html',
  styleUrls: ['./content-settings.component.scss']
})
export class ContentSettingsComponent {

  @Output() isCategoryChangedEvent = new EventEmitter();

  public screenIdentifierActiveState: ScreenIdentifier = ScreenIdentifier.none;
  public categories: CategoryDto[];
  public users: UserDto[];
  public categoryForm: CategoryDto = new CategoryDto();
  public userForm: UserDto = new UserDto();

  public popoverTitle = 'Smazání záznamu';
  public popoverMessage = 'Opravdu záznam odstranit?';

  public importAdlerXmlFile: File;

  constructor(private apiCategoryService: ApiCategoryService,
    private apiUserService: ApiUserService,
    private apiProductImportService: ApiProductImportService,
    private alertService: AlertService) { }

  get screenIdentifiers() { return ScreenIdentifier; }

  public setScreenIdentifierState(newState: ScreenIdentifier, id: number) {
    this.screenIdentifierActiveState = newState;
    switch (this.screenIdentifierActiveState) {
      case ScreenIdentifier.users:
        this.apiUserService.getUsersFilter().subscribe((data: UserDto[]) => this.users = data,
          error => this.alertService.error("Chyba při čtení záznamů uživatelů! " + error.message, this.alertService.getStandardOption(true)));
        break;
      case ScreenIdentifier.categories:
        this.apiCategoryService.getCategoriesFilter(0, 0).subscribe((data: CategoryDto[]) => this.categories = data,
          error => this.alertService.error("Chyba při čtení kategorií! " + error.message, this.alertService.getStandardOption(true)));
        break;
      case ScreenIdentifier.categoryForm:
        this.categoryForm = new CategoryDto();
        if (id > 0) {
          this.apiCategoryService.get(id).subscribe((data: CategoryDto) => this.categoryForm = data,
            error => this.alertService.error("Chyba při čtení detailu kategorie! " + error.message, this.alertService.getStandardOption(true)));
        }
        break;
      case ScreenIdentifier.userForm:
        this.userForm = new UserDto();
        if (id > 0) {
          this.apiUserService.get(id).subscribe((data: UserDto) => {
            this.userForm = data;
            this.userForm.password = null;
          },
            error => this.alertService.error("Chyba při čtení detailu uživatele! " + error.message, this.alertService.getStandardOption(true)));
        }
        break;
    }
  }

  public onSubmitCategory() {
    if (this.categoryForm.id > 0) {
      this.apiCategoryService.update(this.categoryForm).subscribe(_success => this.processCategoryAfterOperation(),
        error => this.alertService.error("Chyba při změně hodnot kategorie! " + error.message, this.alertService.getStandardOption(true)));
    } else {
      this.apiCategoryService.insert(this.categoryForm).subscribe(_success => this.processCategoryAfterOperation(),
        error => this.alertService.error("Chyba při zápisu nové kategorie! " + error.message, this.alertService.getStandardOption(true)));
    }
  }

  public deleteCategory(id: number) {
    this.apiCategoryService.delete(id).subscribe(_success => this.processCategoryAfterOperation(),
      error => this.alertService.error("Chyba při odstrańování záznamu kategorie! " + error.message, this.alertService.getStandardOption(true)));
  }

  public deleteUser(id: number) {
    this.apiUserService.delete(id).subscribe(_success => this.setScreenIdentifierState(this.screenIdentifiers.users, 0),
      error => this.alertService.error("Chyba při odstrańování záznamu uživatele! " + error.message, this.alertService.getStandardOption(true)));
  }

  public onSubmitUser() {
    if (this.userForm.id > 0) {
      if (!this.userForm.password) {
        delete this.userForm.password;
      }
      this.apiUserService.update(this.userForm).subscribe(_success => this.setScreenIdentifierState(this.screenIdentifiers.users, 0),
        error => this.alertService.error("Chyba při změně hodnot uživatele! " + error.message, this.alertService.getStandardOption(true)));
    } else {
      if (!this.userForm.password) {
        this.alertService.error("Při zakládání nového záznamu uživatele musí být heslo vyplněno! ", this.alertService.getStandardOption(true));
      } else {
        this.apiUserService.insert(this.userForm).subscribe(_success => this.setScreenIdentifierState(this.screenIdentifiers.users, 0),
          error => this.alertService.error("Chyba při zápisu nové uživatele! " + error.message, this.alertService.getStandardOption(true)));
      }
    }
  }

  public onImportAdler() {
    if (this.importAdlerXmlFile == undefined) {
      this.alertService.error("Není přiložen soubor s daty!", this.alertService.getStandardOption(true));
      return;
    }
    this.alertService.info("Import položek je zahájen ...", this.alertService.getStandardOption(true));
    this.apiProductImportService.importAdlerProducts(this.importAdlerXmlFile).subscribe(_success =>
      this.alertService.success("Data byly zpracovány na serveru.", this.alertService.getStandardOption(true)));
    this.importAdlerXmlFile = null;
  }

  public handleImportAdlerXmlFileInput(files: FileList) {
    if (files != null) {
      this.importAdlerXmlFile = files.item(0);
    }
  }

  private processCategoryAfterOperation() {
    this.isCategoryChangedEvent.emit();
    this.setScreenIdentifierState(this.screenIdentifiers.categories, 0);
  }
}
