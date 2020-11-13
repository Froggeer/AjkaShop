import { Component, OnInit } from '@angular/core';
import { ApiWarehousePositionService } from 'src/app/api-client/api-warehouse-position.service';
import { ApiWarehousePositionItemService } from 'src/app/api-client/api-warehouse-position-item.service';
import { ApiItemCardService } from 'src/app/api-client/api-item-card.service';
import { ApiItemCardSizePriceService } from 'src/app/api-client/api-item-card-size-price.service';
import { ApiItemCardImageService } from 'src/app/api-client/api-item-card-image.service';
import { AlertService } from 'src/app/services/alert.service';
import { GlobalConstants } from 'src/app/common/global-constants';
import { WarehousePositionItemOverviewDto } from 'src/app/shared-dto/warehouse-position-item-overview-dto.model';
import { WarehousePositionItemDto } from 'src/app/shared-dto/warehouse-position-item-dto.model';
import { WarehousePositionDto } from 'src/app/shared-dto/warehouse-position-dto.model';
import { ItemCardDto } from 'src/app/shared-dto/item-card-dto.model';
import { ItemCardSizePriceDto } from 'src/app/shared-dto/item-card-size-price-dto.model';
import { ItemCardImageDto } from 'src/app/shared-dto/item-card-image-dto.model';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-content-warehouse-positions',
  templateUrl: './content-warehouse-positions.component.html',
  styleUrls: ['./content-warehouse-positions.component.scss']
})
export class ContentWarehousePositionsComponent implements OnInit {

  public isOverview: boolean;
  public isGoodsOverview: boolean;
  public itemCardImageUrl: string;
  public itemCardImageDefaultUrl: string = environment.defaultImage;
  public warehousePositionItemsOriginal: WarehousePositionItemOverviewDto[];
  public warehousePositionItems: WarehousePositionItemOverviewDto[];
  public positionItemForm: WarehousePositionItemDto;
  public warehousePositions: WarehousePositionDto[];
  public itemCards: ItemCardDto[];
  public itemCardSizePrices: ItemCardSizePriceDto[];
  public itemCardImages: ItemCardImageDto[];
  public itemCardImageSelected: ItemCardImageDto;

  public popoverTitle = GlobalConstants.questionDeleteRecordTitle;
  public popoverMessage = GlobalConstants.questionDeleteRecordContent;

  constructor(private alertService: AlertService,
    private apiItemCardService: ApiItemCardService,
    private apiItemCardSizePriceService: ApiItemCardSizePriceService,
    private apiWarehousePositionService: ApiWarehousePositionService,
    private apiItemCardImageService: ApiItemCardImageService,
    private apiWarehousePositionItemService: ApiWarehousePositionItemService) { }

  public ngOnInit(): void {
    this.setupOverview();
    this.apiItemCardService.getItemCardsFilter(0, 0).subscribe((data: ItemCardDto[]) => this.itemCards = data,
      error => this.alertService.error("Chyba při čtení položek karet! " + error.message, this.alertService.getStandardOption(true)));
    this.apiWarehousePositionService.getWarehousePositionFilter(0, 0).subscribe((data: WarehousePositionDto[]) => this.warehousePositions = data,
      error => this.alertService.error("Chyba při čtení přehledu pozic! " + error.message, this.alertService.getStandardOption(true)));
  }

  public deletePosition(id: number) {
    this.apiWarehousePositionItemService.delete(id).subscribe(_success => this.setupOverview());
  }

  public editRecord(position: WarehousePositionItemOverviewDto) {
    this.positionItemForm = position;
    this.setFormData();
    this.isOverview = false;
  }

  public newRecord() {
    this.positionItemForm = new WarehousePositionItemDto();
    this.setFormData();
    this.isOverview = false;
  }

  public onSubmitWarehousePositionItem() {
    if (this.positionItemForm.id > 0) {
      this.apiWarehousePositionItemService.update(this.positionItemForm).subscribe(_success => this.setupOverview(),
        error => this.alertService.error("Chyba při změně hodnot záznamu! " + error.message, this.alertService.getStandardOption(true)));
    } else {
      this.apiWarehousePositionItemService.insert(this.positionItemForm).subscribe(_success => this.setupOverview(),
        error => this.alertService.error("Chyba při zápisu nového záznamu! " + error.message, this.alertService.getStandardOption(true)));
    }
  }

  public checkImagePath(imagePath: string): boolean {
    if (imagePath == null) {
      return false;
    }
    this.itemCardImageUrl = GlobalConstants.urlPrefixPattern.test(imagePath) ? imagePath : environment.apiAjkaUrl + '/' + imagePath;
    return true;
  }

  public handleSelectImageHandler(itemCardImageId: number) {
    this.itemCardImageSelected = this.itemCardImages.find(e => e.id === itemCardImageId);
  }

  public handleSelectItemCardHandler(itemCardId: number) {
    if (itemCardId <= 0) {
      return;
    }

    this.apiItemCardSizePriceService.get(itemCardId).subscribe((data: ItemCardSizePriceDto[]) => {
      this.itemCardSizePrices = data;
      if (this.itemCardSizePrices.length > 0 && this.positionItemForm?.itemCardSizePriceId == null) {
        this.positionItemForm.itemCardSizePriceId = this.itemCardSizePrices[0].id;
      } else if (this.itemCardSizePrices.length == 0) {
        this.positionItemForm.itemCardSizePriceId = null;
      }
    },
      error => this.alertService.error("Chyba při čtení variant velikostí a cen! " + error.message, this.alertService.getStandardOption(true)));

    this.apiItemCardImageService.getItemCardImages(itemCardId).subscribe((data: ItemCardImageDto[]) => {
      this.itemCardImages = data;
      if (this.itemCardImages.length > 0) {
        this.itemCardImageSelected = this.positionItemForm?.itemCardImageId == null
          ? this.itemCardImages[0] : this.itemCardImages.find(x => x.id == this.positionItemForm.itemCardImageId);
        if (this.itemCardImageSelected != null) {
          if (GlobalConstants.urlPrefixPattern.test(this.itemCardImageSelected.imagePath)) {
            if (this.itemCardImageSelected.colorName.length > 0 && this.positionItemForm.itemCardImageId == null) {
              this.positionItemForm.itemCardImageId = this.itemCardImageSelected.id;
            }
          } else {
            this.itemCardImageSelected.imagePath = environment.apiAjkaUrl + '/' + this.itemCardImageSelected.imagePath;
          }
        }
      } else if (this.itemCardImages.length == 0) {
        this.positionItemForm.itemCardImageId = null;
        this.itemCardImageSelected = new ItemCardImageDto();
        this.itemCardImageSelected.imagePath = this.itemCardImageDefaultUrl;
      }
    },
      error => this.alertService.error("Chyba při čtení fotografií! " + error.message, this.alertService.getStandardOption(true)));
  }

  public search(input: string) {
    if (input.length < 1) {
      this.warehousePositionItems = this.warehousePositionItemsOriginal;
      return;
    }
    this.warehousePositionItems = this.warehousePositionItemsOriginal.filter(x => x.itemCard.headline.toLowerCase().indexOf(input.toLowerCase()) != -1
      || x.warehousePosition.code.toLowerCase().indexOf(input.toLowerCase()) != -1);
  }

  public setOverviewToGoods(input: boolean) {
    this.isGoodsOverview = input;
    if (input) {
      this.apiWarehousePositionItemService.getWarehousePositionItemCards().subscribe((data: WarehousePositionItemOverviewDto[]) => this.warehousePositionItems = data,
        error => this.alertService.error("Chyba při čtení záznamů zboží na pozicích! " + error.message, this.alertService.getStandardOption(true)));
    } else {
      this.warehousePositionItems = this.warehousePositionItemsOriginal
    }
  }

  private setupOverview() {
    this.isOverview = true;
    this.apiWarehousePositionItemService.getWarehousePositionItems().subscribe((data: WarehousePositionItemOverviewDto[]) => {
      this.warehousePositionItems = data;
      this.warehousePositionItemsOriginal = this.warehousePositionItems;
    },
      error => this.alertService.error("Chyba při čtení záznamů zboží na pozicích! " + error.message, this.alertService.getStandardOption(true)));
  }

  private setFormData() {
    if (this.itemCards.length > 0 && this.positionItemForm?.itemCardId == null) {
      this.positionItemForm.itemCardId = this.itemCards[0].id;
    }
    this.handleSelectItemCardHandler(this.positionItemForm?.itemCardId);
    if (this.warehousePositions.length > 0 && this.positionItemForm?.warehousePositionId == null) {
      this.positionItemForm.warehousePositionId = this.warehousePositions[0].id;
    }
  }
}
