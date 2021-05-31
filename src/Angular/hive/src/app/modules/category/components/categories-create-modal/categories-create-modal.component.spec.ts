import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoriesCreateModalComponent } from './categories-create-modal.component';

describe('CategoriesCreateModalComponent', () => {
  let component: CategoriesCreateModalComponent;
  let fixture: ComponentFixture<CategoriesCreateModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CategoriesCreateModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CategoriesCreateModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
