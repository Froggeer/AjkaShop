import { Component, Output, EventEmitter } from '@angular/core';
import { ApiCategoryService } from 'src/app/api-client/api-category.service';
import { ApiProductImportService } from 'src/app/api-client/api-product-import.service';
import { AlertService } from 'src/app/services/alert.service';
import { CategoryDto } from 'src/app/shared-dto/category-dto.model';
import { GlobalConstants } from 'src/app/common/global-constants';

export enum ScreenIdentifier {
  none, users, categories, categoryForm, warehousePositions, importAdler
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
  public categoryForm: CategoryDto = new CategoryDto();

  public popoverTitle = GlobalConstants.questionDeleteRecordTitle;
  public popoverMessage = GlobalConstants.questionDeleteRecordContent;

  public importAdlerXmlFile: File;

  constructor(private apiCategoryService: ApiCategoryService,
    private apiProductImportService: ApiProductImportService,
    private alertService: AlertService) { }

  get screenIdentifiers() { return ScreenIdentifier; }

  public setScreenIdentifierState(newState: ScreenIdentifier, id: number) {
    this.screenIdentifierActiveState = newState;
    switch (this.screenIdentifierActiveState) {
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
