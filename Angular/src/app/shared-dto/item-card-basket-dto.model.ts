import { ItemCardDto } from './item-card-dto.model';
import { ItemCardSizePriceDto } from './item-card-size-price-dto.model';

export class ItemCardBasketDto {
    public itemCardId: number;
    public itemCard: ItemCardDto;
    public itemCardSizePriceId: number;;
    public itemCardSizePrice: ItemCardSizePriceDto;
    public orderedQuantity: number;
    public colorName: string;
    public sizeName: string;
    public imagePath: string;

    constructor(itemCard: ItemCardDto, itemCardSizePrice: ItemCardSizePriceDto, 
        requiredQuantity: number, colorName: string, sizeName: string, itemImagePath: string){
        this.itemCard = itemCard;
        this.itemCardSizePrice = itemCardSizePrice;
        this.orderedQuantity = requiredQuantity;
        this.colorName = colorName;
        this.sizeName = sizeName;
        this.imagePath = itemImagePath;
    }
}
