import { NgxLoggerLevel } from 'ngx-logger';

// The file contents for the current environment will overwrite these during build.
// The build system defaults to the dev environment which uses `environment.ts`, but if you do
// `ng build --env=prod` then `environment.prod.ts` will be used instead.
// The list of which env maps to which file can be found in `.angular-cli.json`.

export const environment = {
  production: false,
  logLevel: NgxLoggerLevel.TRACE,
  serverLogLevel: NgxLoggerLevel.OFF,

  userManagementUrl: 'http://localhost:5001',
  gigsManagementUrl: 'http://localhost:5057',
  billingManagementUrl: 'http://localhost:5051',
  orderingManagementUrl: 'http://localhost:5041',
  investingManagement: 'http://localhost:5031',

  chatUrl: 'http://localhost:6001/chat',
  chatApiUrl: 'http://localhost:6001/api/chat',

  clientRoot: 'http://localhost:4200',
  idpAuthority: 'https://localhost:7001',
  clientId: 'angular-client'
};