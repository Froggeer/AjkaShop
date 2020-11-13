import { Component, ViewChild, Input, OnInit } from '@angular/core';
import { ApiItemCardService, ItemCardState } from 'src/app/api-client/api-item-card.service';
import { AlertService } from 'src/app/services/alert.service';
import { environment } from 'src/environments/environment';
import { ItemCardComponent } from 'src/app/components/component-item-card/item-card.component';
import { ItemCardDto } from 'src/app/shared-dto/item-card-dto.model';

@Component({
  selector: 'app-content-item-cards',
  templateUrl: './content-item-cards.component.html',
  styleUrls: ['./content-item-cards.component.scss']
})
export class ContentItemCardsComponent implements OnInit {

  @ViewChild("itemCardComponent") itemCardComponent: ItemCardComponent;
  @ViewChild('itemCardCloseButton') itemCardCloseButton: { nativeElement: { click: () => void; }; };

  @Input() categoryId: number;
  @Input() keyWord: string;
  @Input() isUserAdministrator: boolean = false;

/*   @HostListener('window:resize', ['$event'])
  onResize(_event) {
    this.innerWidth = window.innerWidth;
  }
  public innerWidth: number; */

  public activeCategoryTitle: string;
  public itemCards: ItemCardDto[];
  public itemCardImageUrl: string;
  public itemCardImageDefaultUrl: string = environment.defaultImage;

  constructor(private apiItemCardService: ApiItemCardService,
    private alertService: AlertService) { }

  public ngOnInit(): void {
    this.setupScreen();
/*     this.innerWidth = window.innerWidth; */
  }

  public detail(itemCardId: number) {
    this.itemCardComponent.isUserAdministrator = this.isUserAdministrator;
    this.itemCardComponent.categoryId = this.categoryId;
    this.itemCardComponent.initialize(itemCardId);
  }

  public catchItemCardChange(isItemCardChanged: boolean) {
    if (isItemCardChanged) {
      this.itemCardCloseButton.nativeElement.click();
      this.setupScreen();
    }
  }

  public checkImagePath(imagePath: string): boolean {
    if (imagePath == null) {
      return false;
    }
    this.itemCardImageUrl = environment.apiAjkaUrl + '/' + imagePath;
    return true;
  }

  public getBackgroundColorByState(state: ItemCardState): string {
    switch (state) {
      case ItemCardState.Undefined:
        return 'orangered';
      case ItemCardState.Inactive:
        return 'yellow';
      case ItemCardState.Sold:
        return 'palegreen';
    }
    return 'white';
  }

  public newCategoryCheck(typeList: string): boolean {
    if (typeList != this.activeCategoryTitle) {
      this.activeCategoryTitle = typeList;
      return true;
    }
    return false;
  }

/*   public getWidth(): number {
    return this.innerWidth - (this.innerWidth / 5);
  } */

  private setupScreen() {
    if (this.categoryId > 0) {
      this.apiItemCardService.getItemCardsOverview(this.categoryId, this.isUserAdministrator).subscribe((data: ItemCardDto[]) => this.itemCards = data,
        error => this.alertService.error("Chyba při čtení položek kategorie! " + error.message, this.alertService.getStandardOption(true)));
    } else {
      this.apiItemCardService.getItemCardsByKeyWord(this.keyWord).subscribe((data: ItemCardDto[]) => this.itemCards = data,
        error => this.alertService.error("Chyba při čtení vyhledaných položek! " + error.message, this.alertService.getStandardOption(true)));
    }
  }
}
