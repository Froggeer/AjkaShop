import { Component, OnInit } from '@angular/core';
import { BasketService } from 'src/app/services/basket.service';
import { ApiOrderService } from 'src/app/api-client/api-order.service';
import { AlertService } from 'src/app/services/alert.service';
import { environment } from 'src/environments/environment';
import { ItemCardBasketDto } from 'src/app/shared-dto/item-card-basket-dto.model';
import { BasketHeaderDto } from 'src/app/shared-dto/basket-header-dto.model';

@Component({
  selector: 'app-content-basket',
  templateUrl: './content-basket.component.html',
  styleUrls: ['./content-basket.component.scss']
})
export class ContentBasketComponent implements OnInit {

  public basketItemCards: ItemCardBasketDto[];
  public itemCardImageUrl: string;
  public itemCardImageDefaultUrl: string = environment.defaultImage;
  public priceSummary: number = 0;
  public customerEmail: string;
  public isRequiredSendOrderCopy: boolean;

  constructor(private basketService: BasketService,
    private alertService: AlertService,
    private apiOrderService: ApiOrderService) { }

  ngOnInit(): void {
    this.getItemCards();
  }

  public removeItem(itemCardId: number) {
    this.basketService.remove(itemCardId);
    this.getItemCards();
  }

  public onSubmitCreateOrder() {
    var basketHeader = new BasketHeaderDto();
    basketHeader.email = this.customerEmail;
    basketHeader.isRequestedCopyOfOrder = this.isRequiredSendOrderCopy;
    var basketItems = JSON.parse(JSON.stringify(this.basketItemCards));
    basketItems.forEach((item: ItemCardBasketDto) => {
      item.itemCardId = item.itemCard?.id;
      item.itemCard = null;
      item.itemCardSizePriceId = item.itemCardSizePrice?.id;
      item.itemCardSizePrice = null;
    });
    basketHeader.items = basketItems;
    this.alertService.warn("Objednávka se připravuje, nezavírejte prosím tuto stránku ...", this.alertService.getStandardOption(true));
    this.apiOrderService.createOrder(basketHeader).subscribe((successMessage: string) => {
      if (successMessage) {
        this.alertService.error("Při zakládání objednávky došlo k chybě: " + successMessage, this.alertService.getStandardOption(true));
        return;
      }
      this.alertService.success("Objednávka byla vytvořena a odeslána Ajce na email.", this.alertService.getStandardOption(true));
      this.basketService.clearStorage();
      this.basketItemCards = new Array<ItemCardBasketDto>();
    },
      _error => this.alertService.error("Objednávku se nepodařilo založit, kontaktujte správce na emailové adrese ajcinobchudek@seznam.cz!", this.alertService.getStandardOption(true)));
  }

  private getItemCards() {
    this.basketItemCards = this.basketService.get();
    this.priceSummary = 0;
    this.basketItemCards.forEach(item => this.priceSummary += (item.itemCard.price * item.orderedQuantity));
  }
}
