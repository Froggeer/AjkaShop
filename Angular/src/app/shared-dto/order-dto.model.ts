import { OrderState } from 'src/app/api-client/api-order.service';
import { OrderItemDto } from './order-item-dto.model';

export class OrderDto {
    id: number;
    createDate: Date;
    discount: number;
    customerEmail: string;
    note: string;
    state: OrderState;
    orderItems: OrderItemDto[];
}
