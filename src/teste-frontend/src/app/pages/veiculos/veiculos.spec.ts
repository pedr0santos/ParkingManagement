import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Veiculos } from './veiculos';

describe('Veiculos', () => {
  let component: Veiculos;
  let fixture: ComponentFixture<Veiculos>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Veiculos]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Veiculos);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
