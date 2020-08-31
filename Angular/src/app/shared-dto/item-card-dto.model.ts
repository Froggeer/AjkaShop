import { ItemCardState } from 'src/app/api-client/api-item-card.service';
import { ItemCardSizePriceDto } from 'src/app/shared-dto/item-card-size-price-dto.model';

export class ItemCardDto {
    public id: number;
    public categoryId: number;
    public headline: string;
    public description: string;
    public thumbnailImagePath: string;
    public quantity: number;
    public price: number;
    public sizeList: string;
    public commodityIdentifier: string;
    public isAdlerProduct: boolean;
    public state: ItemCardState;

    public thumbnailImageFakePath: string;
    public imageFakePath: string;
    public basketColorName: string;
    public basketItemImagePath: string;
    public basketSizePrice: ItemCardSizePriceDto;

    constructor(){
        this.id = 0;
        this.categoryId = 0;
        this.headline = '';
        this.description = '';
        this.thumbnailImagePath = '';
        this.quantity = 0;
        this.price = 0;
        this.sizeList = '';
        this.commodityIdentifier = '';
        this.isAdlerProduct = false;
        this.state = ItemCardState.Undefined;

        this.basketColorName = '';
        this.basketSizePrice = null;
    }
}
