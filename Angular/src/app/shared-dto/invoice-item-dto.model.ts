export class InvoiceItemDto {
    public id: number;
    public invoiceId: number;
    public description: string;
    public orderNumber: number;
    public pricePerPiece: number;
    public quantity: number;
}
