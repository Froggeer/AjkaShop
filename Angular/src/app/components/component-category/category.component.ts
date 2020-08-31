import { Component, Input, AfterViewInit } from '@angular/core';
import * as $ from 'jquery';

@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.scss']
})
export class CategoryComponent implements AfterViewInit {

  @Input() id: string;
  @Input() description: string;

  constructor() { }

  public ngAfterViewInit() {
    $(".top-menu-button-active-state").click(function () {
      $(".top-menu-button-active-state").removeClass("active");
      $(this).addClass("active");
    });
  }
}
