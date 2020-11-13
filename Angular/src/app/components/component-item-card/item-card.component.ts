import { Component, Input, Output, EventEmitter } from '@angular/core';
import { ApiItemCardService, ItemCardState } from 'src/app/api-client/api-item-card.service';
import { ApiItemCardImageService } from 'src/app/api-client/api-item-card-image.service';
import { ApiItemCardSizePriceService } from 'src/app/api-client/api-item-card-size-price.service';
import { AlertService } from 'src/app/services/alert.service';
import { BasketService } from 'src/app/services/basket.service';
import { environment } from 'src/environments/environment';
import { ItemCardDto } from 'src/app/shared-dto/item-card-dto.model';
import { ItemCardImageDto } from 'src/app/shared-dto/item-card-image-dto.model';
import { FormDropdownDto } from 'src/app/shared-dto/form-dropdown-dto.model';
import { ItemCardSizePriceDto } from 'src/app/shared-dto/item-card-size-price-dto.model';
import { GlobalConstants } from 'src/app/common/global-constants';

@Component({
  selector: 'app-item-card',
  templateUrl: './item-card.component.html',
  styleUrls: ['./item-card.component.scss']
})
export class ItemCardComponent {

  @Input() isUserAdministrator: boolean = false;
  @Input() categoryId: number;
  @Output() isItemCardChangedEvent = new EventEmitter<boolean>();

  public itemCard: ItemCardDto;
  public itemCardImages: ItemCardImageDto[];
  public itemCardImageSelected: ItemCardImageDto;
  public itemCardImageUrl: string;
  public itemCardImageDefaultUrl: string = environment.defaultImage;
  public itemCardProdictSizeOverviewUrl: string;
  public itemCardSizePriceFullList: ItemCardSizePriceDto[];
  public itemCardSizePrices: ItemCardSizePriceDto[];
  public chosenColor: string = '';
  public chosenSizePrice: ItemCardSizePriceDto;
  public requestedQuantity: number = 1;
  public isRecordEditable: boolean;
  public itemCardStates: FormDropdownDto[] = [
    { key: 0, description: "Neurčeno" },
    { key: 1, description: "V prodeji" },
    { key: 2, description: "Prodáno" },
    { key: 3, description: "Dočasně nedostupné" }
  ];

  private itemThumbnailImageFile: File;
  private itemCardImageFile: FileList;
  private alertOptions = {
    id: 'alert-item-card',
    autoClose: true,
    keepAfterRouteChange: false
  };

  constructor(private apiItemCardService: ApiItemCardService,
    private apiItemCardImageService: ApiItemCardImageService,
    private apiItemCardSizePriceService: ApiItemCardSizePriceService,
    private alertService: AlertService,
    private basketService: BasketService) { }

  public initialize(itemCardId: number) {
    this.isRecordEditable = false;
    this.itemCardImageSelected = new ItemCardImageDto();
    this.itemCardImages = null;
    this.itemCard = new ItemCardDto();
    if (itemCardId > 0) {
      this.apiItemCardService.get(itemCardId).subscribe((data: ItemCardDto) => {
        this.itemCard = data;
        if (this.itemCard.commodityIdentifier) {
          this.itemCardProdictSizeOverviewUrl = 'https://file.adler.info/pdf/size_chart/' + this.itemCard.commodityIdentifier + '---product_size.pdf';
        }
        this.isRecordEditable = true;
        if (this.itemCard.isAdlerProduct) {
          this.apiItemCardSizePriceService.get(itemCardId).subscribe((data: ItemCardSizePriceDto[]) => {
            data.sort(function (a, b) {
              var sizeLeft = a.sizeName.toLowerCase(), sizeRight = b.sizeName.toLowerCase();
              if (sizeLeft < sizeRight) {
                return -1;
              }
              if (sizeLeft > sizeRight) {
                return 1;
              }
              return 0;
            })
            this.itemCardSizePriceFullList = data;
            this.setAvailableSizes();
          },
            error => this.alertService.error("Chyba při čtení variant velikostí a cen! " + error.message, this.alertOptions));
        }
      },
        error => this.alertService.error("Chyba při čtení detailu zboží! " + error.message, this.alertOptions));
      this.apiItemCardImageService.getItemCardImages(itemCardId).subscribe((data: ItemCardImageDto[]) => {
        this.itemCardImages = data;
        if (this.itemCardImages.length > 0) {
          this.itemCardImageSelected = this.itemCardImages[0];
          if (GlobalConstants.urlPrefixPattern.test(this.itemCardImageSelected.imagePath)) {
            if (this.itemCardImageSelected.colorName.length > 0) {
              this.chosenColor = this.itemCardImageSelected.colorName;
            }
          } else {
            this.itemCardImageSelected.imagePath = environment.apiAjkaUrl + '/' + this.itemCardImageSelected.imagePath;
          }
        }
      },
        error => this.alertService.error("Chyba při čtení fotografií! " + error.message, this.alertOptions));
    } else {
      this.isRecordEditable = true;
    }
  }

  public onSubmitItemCard() {
    if (this.itemThumbnailImageFile != null) {
      this.uploadThumbnailImage();
    } else if (this.itemCardImageFile != null) {
      this.apiItemCardImageService.uploadImage(this.itemCard.id, this.itemCardImageFile).subscribe(_success => this.isItemCardChangedEvent.emit(true));
      this.itemCardImageFile = null;
      this.itemCard.imageFakePath = null;
      this.alertService.success("Fotografie byly uloženy do systému.", this.alertOptions);
    } else if (this.itemCard.id > 0) {
      this.apiItemCardService.update(this.itemCard).subscribe(_success => {
        this.alertService.success("Změna záznamu byla úspěšná.", this.alertOptions);
        this.isItemCardChangedEvent.emit(true);
      },
        error => this.alertService.error("Chyba při změně hodnot záznamu! " + error.message, this.alertOptions));
    } else {
      this.itemCard.categoryId = this.categoryId;
      this.itemCard.state = ItemCardState.ForSale;
      this.apiItemCardService.insert(this.itemCard).subscribe(itemCardNewId => {
        this.itemCard.id = itemCardNewId;
        this.alertService.success("Nový záznam je založen.", this.alertOptions);
      },
        error => this.alertService.error("Chyba při zápisu nové karty zboží!" + error.message, this.alertOptions));
    }
  }

  public onSubmitItemCardBasket() {
    if (this.itemCard.isAdlerProduct) {
      this.itemCard.basketColorName = this.chosenColor;
      this.itemCard.basketSizePrice = this.chosenSizePrice;
    }
    if (this.requestedQuantity <= 0) {
      this.alertService.error("Počet kusů do košíku musí být vyšší než 0!", this.alertOptions);
      return;
    }
    if (this.requestedQuantity > this.itemCard.quantity) {
      this.alertService.error("Nelze objednat více než " + this.itemCard.quantity + " kusů zboží!", this.alertOptions);
      return;
    }
    this.itemCard.basketItemImagePath = this.itemCardImageSelected.imagePath;
    var errorMessage = this.basketService.add(this.itemCard, this.requestedQuantity);
    if (errorMessage != null) {
      this.alertService.error(errorMessage, this.alertOptions);
    } else {
      this.alertService.info("Přidáno do košíku.", this.alertOptions);
    }
  }

  public handleSelectChangeHandler(event: any) {
    var chosenSizeName = event.target.value;
    this.chosenSizePrice = this.itemCardSizePrices.find(x => x.sizeName == chosenSizeName);
    this.itemCard.price = this.chosenSizePrice.price;
  }

  public handleThumbnailImageFileInput(files: FileList) {
    if (files != null) {
      this.itemThumbnailImageFile = files.item(0);
    }
  }

  public createThumbnailFromExistingImage() {
    if (this.itemCardImages == undefined || this.itemCardImages.length == 0) {
      this.alertService.error("Ke kartě musí být přiřazena alespoň jedna fotografie!", this.alertOptions);
      return;
    }
    var randomPointer = this.itemCardImages.length > 1 ? (Math.floor(Math.random() * this.itemCardImages.length) + 1) : 0;
    this.apiItemCardService.getImage(this.itemCardImages[randomPointer].imagePath).subscribe((data: File) => {
      this.itemThumbnailImageFile = new File([data], "sample.jpg");
      this.uploadThumbnailImage();
    });
  }

  public handleImageFileInput(files: FileList) {
    if (files != null) {
      this.itemCardImageFile = files;
    }
  }

  public checkImagePath(imagePath: string): boolean {
    if (imagePath == null) {
      return false;
    }
    if (GlobalConstants.urlPrefixPattern.test(imagePath)) {
      this.itemCardImageUrl = imagePath;
      return true;
    }
    this.itemCardImageUrl = environment.apiAjkaUrl + '/' + imagePath;
    return true;
  }

  public setMainImagePath(imageSelected: ItemCardImageDto) {
    this.itemCardImageSelected = imageSelected;
    this.setAvailableSizes();
    if (GlobalConstants.urlPrefixPattern.test(imageSelected.imagePath)) {
      this.chosenColor = imageSelected.colorName;
      return;
    }
    this.itemCardImageSelected.imagePath = environment.apiAjkaUrl + '/' + imageSelected.imagePath;
  }

  public deleteImage(itemCardImageId: number) {
    this.apiItemCardImageService.delete(itemCardImageId).subscribe(_success => this.initialize(this.itemCard.id));
  }

  private uploadThumbnailImage() {
    this.apiItemCardService.uploadThumbnailImage(this.itemCard.id, this.itemThumbnailImageFile).subscribe(_success => this.isItemCardChangedEvent.emit(true));
    this.itemThumbnailImageFile = null;
    this.itemCard.thumbnailImageFakePath = null;
    this.alertService.success("Obrázek náhledu byl změněn.", this.alertOptions);
  }

  private setAvailableSizes() {
    if (this.itemCardImageSelected.availableSizesList == null) {
      this.chosenSizePrice = null;
      return;
    }
    var availabilityPattern = this.itemCardImageSelected.availableSizesList.split('#');
    var availableSizes = new Array<ItemCardSizePriceDto>();
    this.itemCardSizePriceFullList.forEach(function (item) {
      if (availabilityPattern.find(element => element == item.sizeName)) {
        availableSizes.push(item);
      }
    });
    this.itemCardSizePrices = availableSizes;
    if (this.itemCardSizePrices.length > 0) {
      this.chosenSizePrice = this.itemCardSizePrices[0];
      this.itemCard.price = this.itemCardSizePrices[0].price;
    }
  }
}
