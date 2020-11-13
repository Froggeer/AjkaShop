export class IndividualVariableDto {
    public id: number;
    public keyName: string;
    public valueString: string;

    constructor() {
        this.id = 0;
        this.keyName = '';
        this.valueString = '';
    }
}
