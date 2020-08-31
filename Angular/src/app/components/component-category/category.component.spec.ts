import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryComponent } from './category.component';

describe('CategoryComponent', () => {
  let component: CategoryComponent;
  let fixture: ComponentFixture<CategoryComponent>;
  let someDescription: string = 'Tricka';

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [CategoryComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CategoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('category description is received from parent', () => {
    component.description = someDescription;
    fixture.detectChanges();
    const buttonElement: HTMLElement = fixture.nativeElement.querySelector('button');
    expect(buttonElement.innerText).toEqual(someDescription);
  });

  it('category has active tag after click', () => {
    const buttonElement: HTMLElement = fixture.nativeElement.querySelector('button');
    buttonElement.click();
    fixture.detectChanges();
    expect(buttonElement.classList).toContain('active');
  });
});
