import { trigger, state, style, transition, animate } from '@angular/animations';
import { HttpClient, HttpEventType } from '@angular/common/http';
import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { Observable, Subject } from 'rxjs';
import { takeUntil, tap } from 'rxjs/operators';
import { OrderDto, OrdersClient, OrderState, PaginatedListOfOrderDto, ReviewOrderCommand, SetInProgressOrderCommand, StateDto } from 'src/app/clients/ordering-client';
import { NotificationService } from 'src/app/modules/core/services/notification.service';
import { AuthService } from 'src/app/modules/layout/services/auth.service';
import { OrderDetailsComponent } from '../order-details/order-details.component';

@Component({
  selector: 'app-orders-list',
  templateUrl: './orders-list.component.html',
  styleUrls: ['./orders-list.component.scss'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({height: '0px', minHeight: '0'})),
      state('expanded', style({height: '*'})),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ],
})
export class OrdersListComponent implements OnInit, AfterViewInit {
  private unsubscribe = new Subject();
  private orders!: PaginatedListOfOrderDto;

  // Pagination
  // MatPaginator Inputs
  length = 100;
  pageSize = 10;
  pageNumber = 0;
  pageSizeOptions: number[] = [5, 10, 25, 100];

  // MatPaginator Output
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  pageChange(pageEvent: PageEvent) {
    this.pageSize = pageEvent.pageSize;
    this.pageNumber = pageEvent.pageIndex;

    this.getOrders();
  }

  // Table
  displayedColumns: string[] = [
    'orderNumber', 'orderedAt',  'orderStates',
    'unitPrice', 'quantity', 'totalPrice', 'actions'];
  dataSource: MatTableDataSource<OrderDto> = new MatTableDataSource<OrderDto>([]);

  // Data
  orders$!: Observable<PaginatedListOfOrderDto>
  isSeller: boolean = false;

  constructor(
    private ordersClient: OrdersClient,
    private authService: AuthService,
    private notificationService: NotificationService,
    public dialog: MatDialog,
    private http: HttpClient) { }

  ngOnInit(): void {
    this.determineIfSeller();
    this.orders$ =
      this.ordersClient.getMyOrders(this.pageNumber + 1, this.pageSize, this.isSeller)
        .pipe(tap({
          next: (orders) => {
            this.dataSource = new MatTableDataSource<OrderDto>(orders.items);
            this.length = orders.totalCount!;
            this.orders = orders;
          }
        }));
  }

  private determineIfSeller() {
    const user = this.authService.user;
    const roles = user?.profile.role;
    this.isSeller = (roles as string[]).includes('Seller');
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  openDialog(order: OrderDto): void {
    const dialogRef = this.dialog.open(OrderDetailsComponent, {
      width: '50%',
      data: { order: order, isSeller: this.isSeller }
    });

    dialogRef.afterClosed().subscribe(result => {
      this.getOrders();
    });
  }

  updateStatusAsSeller(orderNumber: string, accept: boolean = true) {
    if (this.orders) {
      const state = accept ? OrderState.Accepted : OrderState.Declined;
      const message = accept ? 'Accepted by seller' : 'Declined by seller'

      this.updateOrderStatusLocally(orderNumber, state);
      this.dataSource.data = this.orders.items!

      const command = ReviewOrderCommand.fromJS({
        orderNumber: orderNumber, 
        orderState: state,
        reason: message
      });
      this.ordersClient.reviewOrder(orderNumber, command).subscribe()
    }
  }

  cancelOrder(orderNumber: string) {
    if (this.orders) {
      const state = OrderState.Canceled;
      const message = 'Canceled by buyer'

      this.updateOrderStatusLocally(orderNumber, state);
      this.dataSource.data = this.orders.items!

      const command = ReviewOrderCommand.fromJS({
        orderNumber: orderNumber, 
        orderState: state,
        reason: message
      });
      this.ordersClient.reviewOrder(orderNumber, command).subscribe()
    }
  }

  setInProgress(orderNumber: string) {
    if (this.orders) {
      const state = OrderState.InProgress;
      this.updateOrderStatusLocally(orderNumber, state);
      this.dataSource.data = this.orders.items!

      const command = SetInProgressOrderCommand.fromJS({ orderNumber: orderNumber });
      this.ordersClient.setInProgress(orderNumber, command).subscribe()
    }
  }

  public uploadFile = (orderNumber: string, files: any) => {
    if (files.length === 0) {
      return;
    }
    let fileToUpload = <File>files[0];
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);
 
    this.http.post(`https://localhost:5041/api/orders/${orderNumber}/resolutions`, formData)
      .pipe(
        takeUntil(this.unsubscribe),
        tap({ next: (x) => this.notificationService.openSnackBar('Successfully uploaded resolution to the order') })
      )
      .subscribe(x => this.getOrders());
  }

  private updateOrderStatusLocally(orderNumber : string, state: OrderState) {
    const orders = this.orders.items!;
    const index = orders!.findIndex(o => o.orderNumber == orderNumber);
    let order = this.orders.items![index!]
    order.orderStates = []
    order?.orderStates?.push(new StateDto(
      { orderState: state, reason: '', created: new Date() }));
    
    if (state == OrderState.Completed) {
      order.isClosed = true;
    }
    
    this.orders.items?.splice(index!, 1, order);
  }

  private getOrders() {
    this.ordersClient.getMyOrders(this.pageNumber + 1, this.pageSize, this.isSeller)
        .pipe(tap({
          next: (orders) => {
            this.orders = orders;
            this.dataSource.data = orders.items!;
            this.length = orders.totalCount!;
          }
      })).subscribe();
  }

  // States Management
  canProcess(states: StateDto[]) {
    return this.canChangeStateTo(states, OrderState.Accepted)
  }

  canSetInProgress(states: StateDto[]) {
    const data = states.map(x => x.orderState!)
    const inProgress = data.includes(OrderState.InProgress);
    const accepted = data.includes(OrderState.Accepted);

    return !inProgress && accepted;
  }

  canUpload(states: StateDto[]) {
    const data = states.map(x => x.orderState!)
    const inProgress = data.includes(OrderState.InProgress);
    const completed = data.includes(OrderState.Completed);
    return inProgress && !completed
  }

  private canChangeStateTo(states: StateDto[], s: OrderState) {
    const validState = s;
    const state = this.latestState(states)!;
    const data = states.map(s => s.orderState!);
    const inProcessableState = state < validState;
    const dataIsValid = data.includes(OrderState.OrderDataValid) && data.includes(OrderState.UserBalanceValid);

    return dataIsValid && inProcessableState;
  }

  latestState(states: StateDto[]) {
    const values = states.map(x => x.orderState);
    const invalidStateIndex = values.indexOf(OrderState.Invalid);
    if (invalidStateIndex > -1) {
      return values[invalidStateIndex];
    }

    const latest = states.sort((a, b) => b.created!.getTime() - a.created!.getTime())[0]
    return latest.orderState;
  }
}
