import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CsvUpload } from './csv-upload';

describe('CsvUpload', () => {
  let component: CsvUpload;
  let fixture: ComponentFixture<CsvUpload>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CsvUpload]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CsvUpload);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
