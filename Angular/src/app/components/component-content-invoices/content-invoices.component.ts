import { Component, OnInit } from '@angular/core';
import { ApiInvoiceService } from 'src/app/api-client/api-invoice.service';
import { AlertService } from 'src/app/services/alert.service';
import { InvoiceDto } from 'src/app/shared-dto/invoice-dto.model';
import { FormDropdownDto } from 'src/app/shared-dto/form-dropdown-dto.model';
import { GlobalConstants } from 'src/app/common/global-constants';
import { retry } from 'rxjs/operators';

export enum ScreenIdentifier {
  none, invoices, invoiceForm, invoiceDetail
}

@Component({
  selector: 'app-content-invoices',
  templateUrl: './content-invoices.component.html',
  styleUrls: ['./content-invoices.component.scss']
})
export class ContentInvoicesComponent implements OnInit {

  public screenIdentifierActiveState: ScreenIdentifier = ScreenIdentifier.invoices;
  public invoicePdfUrl: string;
  public invoices: InvoiceDto[];
  public invoiceForm: InvoiceDto = new InvoiceDto();
  public invoicePaymentMethods: FormDropdownDto[] = [
    { key: 0, description: "Neurčeno" },
    { key: 1, description: "Bankovním převodem" },
    { key: 2, description: "Hotově" }
  ];

  public popoverTitle = GlobalConstants.questionDeleteRecordTitle;
  public popoverMessage = GlobalConstants.questionDeleteRecordContent;

  constructor(private alertService: AlertService,
    private apiInvoiceService: ApiInvoiceService) { }

  public ngOnInit(): void {
    this.getInvoices();
  }

  get screenIdentifiers() { return ScreenIdentifier; }

  public setScreenIdentifierState(newState: ScreenIdentifier, id: number = 0) {
    this.screenIdentifierActiveState = newState;
    switch (this.screenIdentifierActiveState) {
      case ScreenIdentifier.invoices:
        this.getInvoices();
        break;
      case ScreenIdentifier.invoiceForm:
      case ScreenIdentifier.invoiceDetail:
        this.invoiceForm = new InvoiceDto();
        if (id > 0) {
          this.apiInvoiceService.getInvoiceDetail(id).pipe(retry(1)).subscribe((data: InvoiceDto) => {
            this.invoiceForm = data;
            this.invoiceForm.paymentMethodDescription = this.invoicePaymentMethods.find(method => method.key == this.invoiceForm.paymentMethod)?.description;
          },
            error => this.alertService.error("Chyba při čtení detailu faktury! " + error.message, this.alertService.getStandardOption(true)));
          this.invoicePdfUrl = this.apiInvoiceService.getInvoicePdfUrl(id);
        }
        break;
    }
  }

  public onSubmitInvoice() {
    if (this.invoiceForm.id > 0) {
      this.apiInvoiceService.update(this.invoiceForm).subscribe(_success => this.setScreenIdentifierState(this.screenIdentifiers.invoices),
        error => this.alertService.error("Chyba při změně hodnot faktury! " + error.message, this.alertService.getStandardOption(true)));
    } else {
      this.apiInvoiceService.insert(this.invoiceForm).subscribe((newId: number) => {
        this.setScreenIdentifierState(this.screenIdentifiers.invoiceDetail, newId);
      },
        error => this.alertService.error("Chyba při zápisu nové faktury! " + error.message, this.alertService.getStandardOption(true)));
    }
  }

  public deleteInvoice(id: number) {
    this.apiInvoiceService.delete(id).subscribe(_success => this.setScreenIdentifierState(this.screenIdentifiers.invoices),
      error => this.alertService.error("Chyba při odstrańování záznamu faktury! " + error.message, this.alertService.getStandardOption(true)));
  }

  public getInvoices() {
    this.apiInvoiceService.getInvoicesFilter(0, 0).subscribe((data: InvoiceDto[]) => this.invoices = data,
      error => this.alertService.error("Chyba při čtení faktur! " + error.message, this.alertService.getStandardOption(true)));
  }
}
