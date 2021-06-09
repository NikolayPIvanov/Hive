import { trigger, state, style, transition, animate } from '@angular/animations';
import { HttpClient, HttpEventType } from '@angular/common/http';
import { ThrowStmt } from '@angular/compiler';
import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { Observable, Subject } from 'rxjs';
import { takeUntil, tap } from 'rxjs/operators';
import { OrderDto, OrdersClient, OrderState, PaginatedListOfOrderDto, ReviewOrderCommand, SetInProgressOrderCommand, StateDto } from 'src/app/clients/ordering-client';
import { NotificationService } from 'src/app/modules/core/services/notification.service';
import { AuthService } from 'src/app/modules/layout/services/auth.service';

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
  pageSize = 20;
  pageIndex = 1;

  displayedColumns: string[] = [
    'orderNumber', 'orderedAt', 'isClosed', 'orderStates',
    'unitPrice', 'quantity', 'totalPrice', 'actions'];
  dataSource: MatTableDataSource<OrderDto> = new MatTableDataSource<OrderDto>([]);;
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  orders$!: Observable<PaginatedListOfOrderDto>
  private orders!: PaginatedListOfOrderDto;
  isSeller: boolean = false;

  constructor(
    private ordersClient: OrdersClient,
    private authService: AuthService,
    private notificationService: NotificationService,
    private http: HttpClient) { }

  ngOnInit(): void {
    this.orders$ =
      this.ordersClient.getMyOrders(this.pageIndex, this.pageSize)
        .pipe(tap({
          next: (orders) => {
            this.dataSource = new MatTableDataSource<OrderDto>(orders.items);
            this.orders = orders;
          }
        }));
    
    const user = this.authService.user;
    const roles = user?.profile.role;
    
    this.isSeller = (roles as string[]).includes('Seller');
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  processSeller(orderNumber: string, accept: boolean = true) {
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
      .subscribe();
  }

  latestState(states: StateDto[]) {
    const sorted = states.sort((a, b) => b.created!.getTime() - a.created!.getTime())[0]
    return sorted.orderState;
  }

  canProcess(states: StateDto[]) {
    const validState = OrderState.UserBalanceValid
    return this.canChangeStateTo(states, validState);
  }

  canSetInProgress(states: StateDto[]) {
    const validState = OrderState.InProgress
    return this.canChangeStateTo(states, validState) && !this.canProcess(states);
  }

  canUpload(states: StateDto[]) {
    const validState = OrderState.Completed
    return this.canChangeStateTo(states, validState) && !this.canSetInProgress(states);
  }

  private canChangeStateTo(states: StateDto[], s: OrderState) {
    const validState = s;
    const state = this.latestState(states)!;
    const data = states.map(s => s.orderState!);
    const inProcessableState = state < validState;

    return data.includes(OrderState.OrderDataValid) && data.includes(OrderState.UserBalanceValid) && inProcessableState;
  }

  private updateOrderStatusLocally(orderNumber : string, state: OrderState) {
    const orders = this.orders.items!;
    const index = orders!.findIndex(o => o.orderNumber == orderNumber);
    let order = this.orders.items![index!]
    order.orderStates = []
    order?.orderStates?.push(new StateDto(
      { orderState: state, reason: '', created: new Date() }));
    
    this.orders.items?.splice(index!, 1);
    this.orders.items?.push(order);
  }
}
