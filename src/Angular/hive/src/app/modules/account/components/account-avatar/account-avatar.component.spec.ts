import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccountAvatarComponent } from './account-avatar.component';

describe('AccountAvatarComponent', () => {
  let component: AccountAvatarComponent;
  let fixture: ComponentFixture<AccountAvatarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AccountAvatarComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AccountAvatarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
