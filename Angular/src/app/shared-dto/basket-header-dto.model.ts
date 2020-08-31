import { ItemCardBasketDto } from './item-card-basket-dto.model';

export class BasketHeaderDto {
    public items: ItemCardBasketDto[];
    public email: string;
    public isRequestedCopyOfOrder: boolean;
}
