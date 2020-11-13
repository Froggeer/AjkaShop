import { Component, OnInit } from '@angular/core';
import { ApiOrderService } from 'src/app/api-client/api-order.service';
import { GlobalConstants } from 'src/app/common/global-constants';
import { AlertService } from 'src/app/services/alert.service';
import { FormDropdownDto } from 'src/app/shared-dto/form-dropdown-dto.model';
import { OrderDto } from 'src/app/shared-dto/order-dto.model';

@Component({
  selector: 'app-content-orders',
  templateUrl: './content-orders.component.html',
  styleUrls: ['./content-orders.component.scss']
})
export class ContentOrdersComponent implements OnInit {

  public isOverview: boolean;
  public orders: OrderDto[];
  public orderDetail: OrderDto;
  public selectedState: number = 1;
  public orderStates: FormDropdownDto[] = [
    { key: 0, description: "Neurčeno" },
    { key: 1, description: "Nezpracované objednávky" },
    { key: 2, description: "Čekající na zaplacení" },
    { key: 3, description: "Uzavřené" },
    { key: 4, description: "Storno" }
  ];

  public popoverTitle = GlobalConstants.questionDeleteRecordTitle;
  public popoverMessage = GlobalConstants.questionDeleteRecordContent;

  constructor(private alertService: AlertService,
    private apiOrderService: ApiOrderService) { }

  ngOnInit(): void {
    this.setupOverview();
  }

  public handleOrderSelected(selectedValue: number) {
    this.selectedState = selectedValue;
    this.setupOverview();
  }

  public editRecord(orderRecord: OrderDto) {
    this.orderDetail = orderRecord;
    this.isOverview = false;
  }

  public deleteRecord(recordId: number) {
    // TODO
    console.log(recordId);
  }

  public returnBackToOverview() {
    this.isOverview = true;
  }

  private setupOverview() {
    this.isOverview = true;
    this.apiOrderService.getDetails(this.selectedState).subscribe((data: OrderDto[]) => {
      this.orders = data;
    },
      error => this.alertService.error("Chyba při čtení záznamů objednávek! " + error.message, this.alertService.getStandardOption(true)));
  }
}
