/* tslint:disable */
/* eslint-disable */
//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v13.9.4.0 (NJsonSchema v10.3.1.0 (Newtonsoft.Json v12.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------
// ReSharper disable InconsistentNaming

import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');

export interface IAccountHoldersClient {
    /**
     * @param pageSize (optional) 
     * @return Successful operation
     */
    getWallet(pageSize: number | undefined): Observable<WalletDto>;
    /**
     * @param pageIndex (optional) 
     * @param pageSize (optional) 
     * @return Successful operation
     */
    getWalletTransactions(walletId: number, pageIndex: number | undefined, pageSize: number | undefined, accountHolderId: string): Observable<PaginatedListOfTransactionDto>;
    /**
     * @param command (optional) 
     * @return Successful operation
     */
    depositInWallet(accountHolderId: number, walletId: number, command: CreateTransactionCommand | null | undefined): Observable<number>;
    /**
     * @return Successful operation
     */
    getTransactionById(accountHolderId: number, walletId: number, transactionNumber: number): Observable<TransactionDto>;
}

@Injectable({
    providedIn: 'root'
})
export class AccountHoldersClient implements IAccountHoldersClient {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "";
    }

    /**
     * @param pageSize (optional) 
     * @return Successful operation
     */
    getWallet(pageSize: number | undefined): Observable<WalletDto> {
        let url_ = this.baseUrl + "/api/AccountHolders/wallet?";
        if (pageSize === null)
            throw new Error("The parameter 'pageSize' cannot be null.");
        else if (pageSize !== undefined)
            url_ += "pageSize=" + encodeURIComponent("" + pageSize) + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processGetWallet(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetWallet(<any>response_);
                } catch (e) {
                    return <Observable<WalletDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<WalletDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetWallet(response: HttpResponseBase): Observable<WalletDto> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = WalletDto.fromJS(resultData200);
            return _observableOf(result200);
            }));
        } else if (status === 404) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result404: any = null;
            let resultData404 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result404 = resultData404 !== undefined ? resultData404 : <any>null;
            return throwException("Not Found operation", status, _responseText, _headers, result404);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<WalletDto>(<any>null);
    }

    /**
     * @param pageIndex (optional) 
     * @param pageSize (optional) 
     * @return Successful operation
     */
    getWalletTransactions(walletId: number, pageIndex: number | undefined, pageSize: number | undefined, accountHolderId: string): Observable<PaginatedListOfTransactionDto> {
        let url_ = this.baseUrl + "/api/AccountHolders/{accountHolderId}/wallets/{walletId}/transactions?";
        if (walletId === undefined || walletId === null)
            throw new Error("The parameter 'walletId' must be defined.");
        url_ = url_.replace("{walletId}", encodeURIComponent("" + walletId));
        if (accountHolderId === undefined || accountHolderId === null)
            throw new Error("The parameter 'accountHolderId' must be defined.");
        url_ = url_.replace("{accountHolderId}", encodeURIComponent("" + accountHolderId));
        if (pageIndex === null)
            throw new Error("The parameter 'pageIndex' cannot be null.");
        else if (pageIndex !== undefined)
            url_ += "pageIndex=" + encodeURIComponent("" + pageIndex) + "&";
        if (pageSize === null)
            throw new Error("The parameter 'pageSize' cannot be null.");
        else if (pageSize !== undefined)
            url_ += "pageSize=" + encodeURIComponent("" + pageSize) + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processGetWalletTransactions(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetWalletTransactions(<any>response_);
                } catch (e) {
                    return <Observable<PaginatedListOfTransactionDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<PaginatedListOfTransactionDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetWalletTransactions(response: HttpResponseBase): Observable<PaginatedListOfTransactionDto> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = PaginatedListOfTransactionDto.fromJS(resultData200);
            return _observableOf(result200);
            }));
        } else if (status === 404) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result404: any = null;
            let resultData404 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result404 = resultData404 !== undefined ? resultData404 : <any>null;
            return throwException("Not Found operation", status, _responseText, _headers, result404);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<PaginatedListOfTransactionDto>(<any>null);
    }

    /**
     * @param command (optional) 
     * @return Successful operation
     */
    depositInWallet(accountHolderId: number, walletId: number, command: CreateTransactionCommand | null | undefined): Observable<number> {
        let url_ = this.baseUrl + "/api/AccountHolders/{accountHolderId}/wallets/{walletId}/transactions";
        if (accountHolderId === undefined || accountHolderId === null)
            throw new Error("The parameter 'accountHolderId' must be defined.");
        url_ = url_.replace("{accountHolderId}", encodeURIComponent("" + accountHolderId));
        if (walletId === undefined || walletId === null)
            throw new Error("The parameter 'walletId' must be defined.");
        url_ = url_.replace("{walletId}", encodeURIComponent("" + walletId));
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(command);

        let options_ : any = {
            body: content_,
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Content-Type": "application/json",
                "Accept": "application/json"
            })
        };

        return this.http.request("post", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processDepositInWallet(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processDepositInWallet(<any>response_);
                } catch (e) {
                    return <Observable<number>><any>_observableThrow(e);
                }
            } else
                return <Observable<number>><any>_observableThrow(response_);
        }));
    }

    protected processDepositInWallet(response: HttpResponseBase): Observable<number> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 201) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result201: any = null;
            let resultData201 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result201 = resultData201 !== undefined ? resultData201 : <any>null;
            return _observableOf(result201);
            }));
        } else if (status === 400) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result400: any = null;
            let resultData400 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result400 = resultData400 !== undefined ? resultData400 : <any>null;
            return throwException("Bad Request operation", status, _responseText, _headers, result400);
            }));
        } else if (status === 404) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result404: any = null;
            let resultData404 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result404 = resultData404 !== undefined ? resultData404 : <any>null;
            return throwException("Not Found operation", status, _responseText, _headers, result404);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<number>(<any>null);
    }

    /**
     * @return Successful operation
     */
    getTransactionById(accountHolderId: number, walletId: number, transactionNumber: number): Observable<TransactionDto> {
        let url_ = this.baseUrl + "/api/AccountHolders/{accountHolderId}/wallets/{walletId}/transactions/{transactionNumber}";
        if (accountHolderId === undefined || accountHolderId === null)
            throw new Error("The parameter 'accountHolderId' must be defined.");
        url_ = url_.replace("{accountHolderId}", encodeURIComponent("" + accountHolderId));
        if (walletId === undefined || walletId === null)
            throw new Error("The parameter 'walletId' must be defined.");
        url_ = url_.replace("{walletId}", encodeURIComponent("" + walletId));
        if (transactionNumber === undefined || transactionNumber === null)
            throw new Error("The parameter 'transactionNumber' must be defined.");
        url_ = url_.replace("{transactionNumber}", encodeURIComponent("" + transactionNumber));
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processGetTransactionById(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetTransactionById(<any>response_);
                } catch (e) {
                    return <Observable<TransactionDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<TransactionDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetTransactionById(response: HttpResponseBase): Observable<TransactionDto> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = TransactionDto.fromJS(resultData200);
            return _observableOf(result200);
            }));
        } else if (status === 404) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result404: any = null;
            let resultData404 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result404 = resultData404 !== undefined ? resultData404 : <any>null;
            return throwException("Not Found operation", status, _responseText, _headers, result404);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<TransactionDto>(<any>null);
    }
}

export class WalletDto implements IWalletDto {
    id?: number;
    accountHolderId?: number;
    balance?: number;
    transactions?: TransactionDto[] | undefined;

    constructor(data?: IWalletDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.id = _data["id"];
            this.accountHolderId = _data["accountHolderId"];
            this.balance = _data["balance"];
            if (Array.isArray(_data["transactions"])) {
                this.transactions = [] as any;
                for (let item of _data["transactions"])
                    this.transactions!.push(TransactionDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): WalletDto {
        data = typeof data === 'object' ? data : {};
        let result = new WalletDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["accountHolderId"] = this.accountHolderId;
        data["balance"] = this.balance;
        if (Array.isArray(this.transactions)) {
            data["transactions"] = [];
            for (let item of this.transactions)
                data["transactions"].push(item.toJSON());
        }
        return data; 
    }
}

export interface IWalletDto {
    id?: number;
    accountHolderId?: number;
    balance?: number;
    transactions?: TransactionDto[] | undefined;
}

export class TransactionDto implements ITransactionDto {
    transactionNumber?: number;
    amount?: number;
    transactionType?: TransactionType;
    walletId?: number;
    orderNumber?: string | undefined;

    constructor(data?: ITransactionDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.transactionNumber = _data["transactionNumber"];
            this.amount = _data["amount"];
            this.transactionType = _data["transactionType"];
            this.walletId = _data["walletId"];
            this.orderNumber = _data["orderNumber"];
        }
    }

    static fromJS(data: any): TransactionDto {
        data = typeof data === 'object' ? data : {};
        let result = new TransactionDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["transactionNumber"] = this.transactionNumber;
        data["amount"] = this.amount;
        data["transactionType"] = this.transactionType;
        data["walletId"] = this.walletId;
        data["orderNumber"] = this.orderNumber;
        return data; 
    }
}

export interface ITransactionDto {
    transactionNumber?: number;
    amount?: number;
    transactionType?: TransactionType;
    walletId?: number;
    orderNumber?: string | undefined;
}

export enum TransactionType {
    Fund = 0,
    Payment = 1,
}

export class PaginatedListOfTransactionDto implements IPaginatedListOfTransactionDto {
    items?: TransactionDto[] | undefined;
    pageIndex?: number;
    totalPages?: number;
    totalCount?: number;
    hasPreviousPage?: boolean;
    hasNextPage?: boolean;

    constructor(data?: IPaginatedListOfTransactionDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            if (Array.isArray(_data["items"])) {
                this.items = [] as any;
                for (let item of _data["items"])
                    this.items!.push(TransactionDto.fromJS(item));
            }
            this.pageIndex = _data["pageIndex"];
            this.totalPages = _data["totalPages"];
            this.totalCount = _data["totalCount"];
            this.hasPreviousPage = _data["hasPreviousPage"];
            this.hasNextPage = _data["hasNextPage"];
        }
    }

    static fromJS(data: any): PaginatedListOfTransactionDto {
        data = typeof data === 'object' ? data : {};
        let result = new PaginatedListOfTransactionDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        if (Array.isArray(this.items)) {
            data["items"] = [];
            for (let item of this.items)
                data["items"].push(item.toJSON());
        }
        data["pageIndex"] = this.pageIndex;
        data["totalPages"] = this.totalPages;
        data["totalCount"] = this.totalCount;
        data["hasPreviousPage"] = this.hasPreviousPage;
        data["hasNextPage"] = this.hasNextPage;
        return data; 
    }
}

export interface IPaginatedListOfTransactionDto {
    items?: TransactionDto[] | undefined;
    pageIndex?: number;
    totalPages?: number;
    totalCount?: number;
    hasPreviousPage?: boolean;
    hasNextPage?: boolean;
}

export class CreateTransactionCommand implements ICreateTransactionCommand {
    amount?: number;
    orderNumber?: string | undefined;
    walletId?: number;

    constructor(data?: ICreateTransactionCommand) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.amount = _data["amount"];
            this.orderNumber = _data["orderNumber"];
            this.walletId = _data["walletId"];
        }
    }

    static fromJS(data: any): CreateTransactionCommand {
        data = typeof data === 'object' ? data : {};
        let result = new CreateTransactionCommand();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["amount"] = this.amount;
        data["orderNumber"] = this.orderNumber;
        data["walletId"] = this.walletId;
        return data; 
    }
}

export interface ICreateTransactionCommand {
    amount?: number;
    orderNumber?: string | undefined;
    walletId?: number;
}

export interface FileResponse {
    data: Blob;
    status: number;
    fileName?: string;
    headers?: { [name: string]: any };
}

export class SwaggerException extends Error {
    message: string;
    status: number;
    response: string;
    headers: { [key: string]: any; };
    result: any;

    constructor(message: string, status: number, response: string, headers: { [key: string]: any; }, result: any) {
        super();

        this.message = message;
        this.status = status;
        this.response = response;
        this.headers = headers;
        this.result = result;
    }

    protected isSwaggerException = true;

    static isSwaggerException(obj: any): obj is SwaggerException {
        return obj.isSwaggerException === true;
    }
}

function throwException(message: string, status: number, response: string, headers: { [key: string]: any; }, result?: any): Observable<any> {
    if (result !== null && result !== undefined)
        return _observableThrow(result);
    else
        return _observableThrow(new SwaggerException(message, status, response, headers, null));
}

function blobToText(blob: any): Observable<string> {
    return new Observable<string>((observer: any) => {
        if (!blob) {
            observer.next("");
            observer.complete();
        } else {
            let reader = new FileReader();
            reader.onload = event => {
                observer.next((<any>event.target).result);
                observer.complete();
            };
            reader.readAsText(blob);
        }
    });
}