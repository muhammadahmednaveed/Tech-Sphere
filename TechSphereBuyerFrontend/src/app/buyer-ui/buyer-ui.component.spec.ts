import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BuyerUIComponent } from './buyer-ui.component';

describe('BuyerUIComponent', () => {
  let component: BuyerUIComponent;
  let fixture: ComponentFixture<BuyerUIComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BuyerUIComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BuyerUIComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
