import { Component } from '@angular/core';
import { ApiUserService } from 'src/app/api-client/api-user.service';
import { AlertService } from 'src/app/services/alert.service';
import { UserDto } from 'src/app/shared-dto/user-dto.model';
import { GlobalConstants } from 'src/app/common/global-constants';

@Component({
  selector: 'app-management-user',
  templateUrl: './management-user.component.html',
  styleUrls: ['./management-user.component.scss']
})
export class ManagementUserComponent {

  public isScreenStateForm: boolean = false;
  public users: UserDto[];
  public userForm: UserDto = new UserDto();

  public popoverTitle = GlobalConstants.questionDeleteRecordTitle;
  public popoverMessage = GlobalConstants.questionDeleteRecordContent;

  constructor(private apiUserService: ApiUserService,
    private alertService: AlertService) {
    this.getUsers();
  }

  public manageRecord(id: number) {
    this.isScreenStateForm = true;
    this.userForm = new UserDto();
    if (id > 0) {
      this.apiUserService.get(id).subscribe((data: UserDto) => {
        this.userForm = data;
        this.userForm.password = null;
      },
        error => this.alertService.error("Chyba při čtení detailu uživatele! " + error.message, this.alertService.getStandardOption(true)));
    }
  }

  public deleteRecord(id: number) {
    this.apiUserService.delete(id).subscribe(_success => this.refreshOverview(),
      error => this.alertService.error("Chyba při odstrańování záznamu uživatele! " + error.message, this.alertService.getStandardOption(true)));
  }

  public onSubmit() {
    if (this.userForm.id > 0) {
      if (!this.userForm.password) {
        delete this.userForm.password;
      }
      this.apiUserService.update(this.userForm).subscribe(_success => this.refreshOverview(),
        error => this.alertService.error("Chyba při změně hodnot uživatele! " + error.message, this.alertService.getStandardOption(true)));
    } else {
      if (!this.userForm.password) {
        this.alertService.error("Při zakládání nového záznamu uživatele musí být heslo vyplněno! ", this.alertService.getStandardOption(true));
      } else {
        this.apiUserService.insert(this.userForm).subscribe(_success => this.refreshOverview(),
          error => this.alertService.error("Chyba při zápisu nové uživatele! " + error.message, this.alertService.getStandardOption(true)));
      }
    }
    this.getUsers();
  }

  private getUsers() {
    this.apiUserService.getUsersFilter().subscribe((data: UserDto[]) => this.users = data,
      error => this.alertService.error("Chyba při čtení záznamů uživatelů! " + error.message, this.alertService.getStandardOption(true)));
  }

  private refreshOverview() {
    this.getUsers();
    this.isScreenStateForm = false;
  }
}
