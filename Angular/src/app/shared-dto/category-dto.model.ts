export class CategoryDto {
    public id: number;
    public orderNumber: number;
    public description: string;
    public isValid: boolean;

    constructor() {
        this.id = 0;
        this.orderNumber = 0;
        this.description = '';
        this.isValid = false;
    }
}
