import { InvoicePaymentMethod } from 'src/app/api-client/api-invoice.service';
import { InvoiceItemDto } from './invoice-item-dto.model';

export class InvoiceDto {
    public id: number;
    public invoiceNumber: number;
    public recipientName: string;
    public recipientStreet: string;
    public recipientCity: string;
    public recipientZipCode: string;
    public variableSymbol: string;
    public paymentMethod: InvoicePaymentMethod;
    public releaseDate: Date;
    public dueDate: Date;
    public taxablePerformanceDate: Date;
    public invoiceItems: InvoiceItemDto[];

    public paymentMethodDescription: string;
    
    constructor(){
        this.id = 0;
        this.invoiceNumber = 0;
        this.recipientName = '';
        this.recipientStreet = '';
        this.recipientCity = '';
        this.recipientZipCode = '';
        this.variableSymbol = '';
        this.paymentMethod = InvoicePaymentMethod.BankTransfer;
        this.releaseDate = new Date();
        this.dueDate = new Date();
        this.taxablePerformanceDate = new Date();
        this.invoiceItems = [];
    }
}
