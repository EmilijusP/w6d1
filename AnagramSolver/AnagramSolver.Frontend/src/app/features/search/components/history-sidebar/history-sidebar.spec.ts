import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HistorySidebar } from './history-sidebar';

describe('HistorySidebar', () => {
  let component: HistorySidebar;
  let fixture: ComponentFixture<HistorySidebar>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HistorySidebar]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HistorySidebar);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
