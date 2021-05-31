import { TestBed } from '@angular/core/testing';

import { GigsService } from './gigs.service';

describe('GigsService', () => {
  let service: GigsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GigsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
