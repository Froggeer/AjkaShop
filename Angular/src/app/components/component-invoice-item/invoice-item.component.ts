import { Component, Input, SimpleChanges } from '@angular/core';
import { ApiInvoiceItemService } from 'src/app/api-client/api-invoice-item.service';
import { ApiInvoiceService } from 'src/app/api-client/api-invoice.service';
import { AlertService } from 'src/app/services/alert.service';
import { InvoiceDto } from 'src/app/shared-dto/invoice-dto.model';
import { InvoiceItemDto } from 'src/app/shared-dto/invoice-item-dto.model';
import { FormDropdownDto } from 'src/app/shared-dto/form-dropdown-dto.model';

@Component({
  selector: 'app-invoice-item',
  templateUrl: './invoice-item.component.html',
  styleUrls: ['./invoice-item.component.scss']
})
export class InvoiceItemComponent {

  @Input() invoice: InvoiceDto;
  @Input() invoicePaymentMethods: FormDropdownDto[];

  public priceSum: number;
  public itemsCount: number;
  public invoiceItem: InvoiceItemDto = new InvoiceItemDto();
  public popoverTitle = 'Smazání záznamu';
  public popoverMessage = 'Opravdu záznam odstranit?';

  constructor(private apiInvoiceItemService: ApiInvoiceItemService,
    private apiInvoiceService: ApiInvoiceService,
    private alertService: AlertService) { }

  public ngOnChanges(_changes: SimpleChanges) {
    this.setItemsPriceSum();
  }

  public onSubmitInvoiceItem() {
    if (this.invoice.id == 0) {
      this.alertService.error("Záhlaví faktury není k dispozici! ", this.alertService.getStandardOption(true));
      return;
    }
    if (this.invoiceItem.id > 0) {
      this.apiInvoiceItemService.update(this.invoiceItem).subscribe(_success => {
        this.reloadInvoice();
      },
        error => this.alertService.error("Chyba při změně hodnot faktury! " + error.message, this.alertService.getStandardOption(true)));
    } else {
      this.invoiceItem.invoiceId = this.invoice.id;
      this.apiInvoiceItemService.insert(this.invoiceItem).subscribe((_newId: number) => {
        this.reloadInvoice();
      },
        error => this.alertService.error("Chyba při zápisu nové položky faktury! " + error.message, this.alertService.getStandardOption(true)));
    }
  }

  public editInvoiceItem(invoiceRecord: InvoiceItemDto) {
    this.invoiceItem = invoiceRecord;
  }

  public deleteInvoiceItem(id: number) {
    this.apiInvoiceItemService.delete(id).subscribe(_success => this.reloadInvoice(),
      error => this.alertService.error("Chyba při odstrańování řádku! " + error.message, this.alertService.getStandardOption(true)));
  }

  public setItemsPriceSum() {
    if (this.invoice.invoiceItems.length > 0) {
      this.priceSum = this.invoice.invoiceItems.map(x =>
        (x.pricePerPiece * (x.quantity > 0 ? x.quantity : 1))).reduce(function (a, b) { return a + b; });
      this.itemsCount = this.invoice.invoiceItems.map(x => x.quantity).reduce(function (a, b) { return a + b; });
    }
  }

  private reloadInvoice() {
    this.invoiceItem = new InvoiceItemDto();
    this.apiInvoiceService.getInvoiceDetail(this.invoice.id).subscribe((data: InvoiceDto) => {
      this.invoice = data;
      this.setItemsPriceSum();
      this.invoice.paymentMethodDescription = this.invoicePaymentMethods.find(method => method.key == this.invoice.paymentMethod)?.description;
    },
      (error: { message: string; }) => this.alertService.error("Chyba při čtení detailu faktury! " + error.message, this.alertService.getStandardOption(true)));
  }
}
