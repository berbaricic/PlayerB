import { TestBed } from '@angular/core/testing';

import { SessiondataService } from './sessiondata.service';

describe('SessiondataService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: SessiondataService = TestBed.get(SessiondataService);
    expect(service).toBeTruthy();
  });
});
