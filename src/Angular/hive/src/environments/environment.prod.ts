import { NgxLoggerLevel } from "ngx-logger";

export const environment = {
  production: true,
  logLevel: NgxLoggerLevel.TRACE,
  serverLogLevel: NgxLoggerLevel.OFF,

  userManagementUrl: 'http://localhost:5001',
  gigsManagementUrl: 'http://localhost:5057',
  billingManagementUrl: 'http://localhost:5051',
  orderingManagementUrl: 'http://localhost:5041',
  investingManagement: 'http://localhost:5031',

  chatUrl: 'http://localhost:6001/chat',
  chatApiUrl: 'http://localhost:6001/api/chat',

  clientRoot: 'http://localhost',
  idpAuthority: 'https://localhost:7001',
  clientId: 'angular-prod'
};
