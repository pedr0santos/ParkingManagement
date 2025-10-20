import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Faturamento } from './faturamento';

describe('Faturamento', () => {
  let component: Faturamento;
  let fixture: ComponentFixture<Faturamento>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Faturamento]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Faturamento);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
