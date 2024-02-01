import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LokalMapaLokacijaComponent } from './lokal-mapa-lokacija.component';

describe('LokalMapaLokacijaComponent', () => {
  let component: LokalMapaLokacijaComponent;
  let fixture: ComponentFixture<LokalMapaLokacijaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LokalMapaLokacijaComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LokalMapaLokacijaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
