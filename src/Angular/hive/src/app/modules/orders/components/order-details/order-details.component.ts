import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Observable, of } from 'rxjs';
import { map, switchMap, takeUntil, tap } from 'rxjs/operators';
import { FileResponse, GigDto, GigsClient } from 'src/app/clients/gigs-client';
import { OrderDto, OrdersClient, OrderState } from 'src/app/clients/ordering-client';
import { AuthService } from 'src/app/modules/layout/services/auth.service';
import { saveAs } from 'file-saver';

@Component({
  selector: 'app-order-details',
  templateUrl: './order-details.component.html',
  styleUrls: ['./order-details.component.scss']
})
export class OrderDetailsComponent implements OnInit {
  public gig$!: Observable<GigDto>;
  public download!: Observable<FileResponse>;


  constructor(
    private authService: AuthService,
    private gigsClient: GigsClient,
    private ordersClient: OrdersClient,
    public dialogRef: MatDialogRef<OrderDetailsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: OrderDto
  ) { }

  ngOnInit(): void {
    this.gig$ = this.gigsClient.getGigByPackageId(this.data.packageId!)
      .pipe(
        map(gig => {
          this.download = this.gigsClient.getAvatar(gig.id!)
          return gig;
        })
    )
    
    this.authService.user?.profile;
  }

  accept(version: string) {
    this.ordersClient.acceptResolution(version, this.data.orderNumber!)
      .pipe(tap({ next: (x) => this.onNoClick() }))
      .subscribe();
  }

  downloadResolution(version: string) {
    this.ordersClient.downloadResolutionFile(version, this.data.orderNumber!)
      .pipe(
        tap({ next: (response) => saveAs(response.data!, version)}))
      .subscribe();
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  stateAsString(state: OrderState) {
    return OrderState[state];
  }

  orderedPackage(gig: GigDto) {
    return gig.packages?.filter(p => p.id == this.data.packageId)[0];
  }
  
  public isBuyer() {
    return true;
  }

}
