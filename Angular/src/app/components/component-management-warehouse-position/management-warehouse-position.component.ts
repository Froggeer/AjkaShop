import { Component } from '@angular/core';
import { WarehousePositionDto } from 'src/app/shared-dto/warehouse-position-dto.model';
import { GlobalConstants } from 'src/app/common/global-constants';
import { ApiWarehousePositionService } from 'src/app/api-client/api-warehouse-position.service';
import { AlertService } from 'src/app/services/alert.service';

@Component({
  selector: 'app-management-warehouse-position',
  templateUrl: './management-warehouse-position.component.html',
  styleUrls: ['./management-warehouse-position.component.scss']
})
export class ManagementWarehousePositionComponent {

  public isScreenStateForm: boolean = false;
  public warehousePositions: WarehousePositionDto[];
  public warehousePositionForm: WarehousePositionDto = new WarehousePositionDto();

  public popoverTitle = GlobalConstants.questionDeleteRecordTitle;
  public popoverMessage = GlobalConstants.questionDeleteRecordContent;

  constructor(private apiWarehousePositionService: ApiWarehousePositionService,
    private alertService: AlertService) {
    this.getWarehousePositions();
  }

  public manageRecord(id: number) {
    this.isScreenStateForm = true;
    this.warehousePositionForm = new WarehousePositionDto();
    if (id > 0) {
      this.apiWarehousePositionService.get(id).subscribe((data: WarehousePositionDto) => this.warehousePositionForm = data,
        error => this.alertService.error("Chyba při čtení detailu pozice! " + error.message, this.alertService.getStandardOption(true)));
    }
  }

  public deleteRecord(id: number) {
    this.apiWarehousePositionService.delete(id).subscribe(_success => this.refreshOverview(),
      error => this.alertService.error("Chyba při odstrańování záznamu! " + error.message, this.alertService.getStandardOption(true)));
  }

  public onSubmit() {
    if (this.warehousePositionForm.id > 0) {
      this.apiWarehousePositionService.update(this.warehousePositionForm).subscribe(_success => this.refreshOverview(),
        error => this.alertService.error("Chyba při změně hodnot záznamu! " + error.message, this.alertService.getStandardOption(true)));
    } else {
      this.apiWarehousePositionService.insert(this.warehousePositionForm).subscribe(_success => this.refreshOverview(),
        error => this.alertService.error("Chyba při zápisu nového záznamu! " + error.message, this.alertService.getStandardOption(true)));
    }
    this.getWarehousePositions();
  }

  private getWarehousePositions() {
    this.apiWarehousePositionService.getWarehousePositionFilter(0, 0).subscribe((data: WarehousePositionDto[]) => this.warehousePositions = data,
      error => this.alertService.error("Chyba při čtení přehledu! " + error.message, this.alertService.getStandardOption(true)));
  }

  private refreshOverview() {
    this.getWarehousePositions();
    this.isScreenStateForm = false;
  }
}
