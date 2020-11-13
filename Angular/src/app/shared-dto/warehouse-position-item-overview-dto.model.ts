import { WarehousePositionDto } from './warehouse-position-dto.model';
import { ItemCardDto } from './item-card-dto.model';
import { ItemCardSizePriceDto } from './item-card-size-price-dto.model';
import { ItemCardImageDto } from './item-card-image-dto.model';

export class WarehousePositionItemOverviewDto {
    id: number;
    warehousePositionId: number;
    warehousePosition: WarehousePositionDto;
    itemCardId: number;
    itemCard: ItemCardDto;
    itemCardSizePriceId: number;
    itemCardSizePrice: ItemCardSizePriceDto;
    itemCardImageId: number;
    itemCardImage: ItemCardImageDto;
    quantity: number;
}
