import { Component, Input } from '@angular/core';
import { ApiIndividualVariableService } from 'src/app/api-client/api-individual-variable.service';
import { AlertService } from 'src/app/services/alert.service';
import { IndividualVariableDto } from 'src/app/shared-dto/individual-variable-dto.model';

@Component({
  selector: 'app-content-info',
  templateUrl: './content-info.component.html',
  styleUrls: ['./content-info.component.scss']
})
export class ContentInfoComponent {

  @Input() isUserAdministrator: boolean = false;

  public individualVariable: IndividualVariableDto = new IndividualVariableDto();

  constructor(private alertService: AlertService,
    private apiIndividualVariableService: ApiIndividualVariableService) { }

  public getContent(keyName: string) {
    this.apiIndividualVariableService.getIndividualVariable(keyName).subscribe((data: IndividualVariableDto) => {
      this.individualVariable = data;
      if (this.individualVariable == null) {
        this.individualVariable = new IndividualVariableDto();
        this.individualVariable.keyName = keyName;
      }
    },
      error => this.alertService.error("Chyba při čtení záznamu informací! " + error.message, this.alertService.getStandardOption(true)));
  }

  public onSubmit() {
    if (this.individualVariable.id > 0) {
      this.apiIndividualVariableService.update(this.individualVariable).subscribe(_success => this.alertService.success("Změna záznamu byla úspěšná.",
        this.alertService.getStandardOption(true)),
        error => this.alertService.error("Chyba při změně hodnot záznamu! " + error.message, this.alertService.getStandardOption(true)));
    } else {
      this.apiIndividualVariableService.insert(this.individualVariable).subscribe(_success => this.alertService.success("Nový záznam je založen.",
        this.alertService.getStandardOption(true)),
        error => this.alertService.error("Chyba při zápisu nového záznamu!" + error.message, this.alertService.getStandardOption(true)));
    }
  }
}
