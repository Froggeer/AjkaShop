import { Injectable } from '@angular/core';
import { GlobalConstants } from 'src/app/common/global-constants';
import { ItemCardDto } from 'src/app/shared-dto/item-card-dto.model';
import { ItemCardBasketDto } from 'src/app/shared-dto/item-card-basket-dto.model';

@Injectable({
  providedIn: 'root'
})
export class BasketService {

  constructor() {
    this.clearStorage();
  }

  public get() {
    return JSON.parse(localStorage.getItem(GlobalConstants.storageBasket));
  }

  public add(itemCard: ItemCardDto, requestedQuantity: number): string {
    if (itemCard.quantity == 0) {
      return "Zboží již není dostupné!";
    }
    var basketContent: Array<ItemCardBasketDto> = JSON.parse(localStorage.getItem(GlobalConstants.storageBasket));
    var indexOfExistingItem = basketContent.findIndex((element: ItemCardBasketDto) => element.itemCard.id == itemCard.id
      && element.sizeName == itemCard.basketSizePrice?.sizeName
      && element.colorName == itemCard.basketColorName);
    if (indexOfExistingItem >= 0) {
      if ((basketContent[indexOfExistingItem].orderedQuantity + requestedQuantity) > basketContent[indexOfExistingItem].itemCard.quantity) {
        return "V košíku je již maximální dostupné množství zboží";
      }
      basketContent[indexOfExistingItem].orderedQuantity += requestedQuantity;
    } else {
      basketContent.push(new ItemCardBasketDto(itemCard, itemCard.basketSizePrice, requestedQuantity, itemCard.basketColorName, 
        itemCard.basketSizePrice?.sizeName, itemCard.basketItemImagePath));
    }
    localStorage.setItem(GlobalConstants.storageBasket, JSON.stringify(basketContent));
    return null;
  }

  public remove(id: number) {
    var basketContent: Array<ItemCardBasketDto> = JSON.parse(localStorage.getItem(GlobalConstants.storageBasket));
    var indexOfExistingItem = basketContent.findIndex((element: ItemCardBasketDto) => element.itemCard.id == id);
    if (indexOfExistingItem >= 0) {
      basketContent.splice(indexOfExistingItem, 1);
      localStorage.setItem(GlobalConstants.storageBasket, JSON.stringify(basketContent));
    }
  }

  public clearStorage() {
    localStorage.setItem(GlobalConstants.storageBasket, JSON.stringify(new Array<ItemCardBasketDto>()));
  }
}
