import { Component, Input } from '@angular/core';
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

  public invoiceItem: InvoiceItemDto = new InvoiceItemDto();
  public popoverTitle = 'Smazání záznamu';
  public popoverMessage = 'Opravdu záznam odstranit?';

  constructor(private apiInvoiceItemService: ApiInvoiceItemService,
    private apiInvoiceService: ApiInvoiceService,
    private alertService: AlertService) { }

  public onSubmitInvoiceItem() {
    if (this.invoice.id == 0) {
      this.alertService.error("Záhlaví faktury není k dispozici! ", this.alertService.getStandardOption(true));
      return;
    }
    this.invoiceItem.invoiceId = this.invoice.id;
    this.apiInvoiceItemService.insert(this.invoiceItem).subscribe(_success => {
      this.invoiceItem = new InvoiceItemDto();
      this.reloadInvoice();
    },
      (error: { message: string; }) => this.alertService.error("Chyba při zápisu nového řádku! " + error.message, this.alertService.getStandardOption(true)));
  }

  public deleteIvoiceItem(id: number) {
    this.apiInvoiceItemService.delete(id).subscribe(_success => this.reloadInvoice(),
      error => this.alertService.error("Chyba při odstrańování řádku! " + error.message, this.alertService.getStandardOption(true)));
  }

  private reloadInvoice() {
    this.apiInvoiceService.getInvoiceDetail(this.invoice.id).subscribe((data: InvoiceDto) => {
      this.invoice = data;
      this.invoice.paymentMethodDescription = this.invoicePaymentMethods.find(method => method.key == this.invoice.paymentMethod)?.description;
    },
      (error: { message: string; }) => this.alertService.error("Chyba při čtení detailu faktury! " + error.message, this.alertService.getStandardOption(true)));
  }
}
