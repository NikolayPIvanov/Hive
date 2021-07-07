import { NgxLoggerLevel } from 'ngx-logger';

// The file contents for the current environment will overwrite these during build.
// The build system defaults to the dev environment which uses `environment.ts`, but if you do
// `ng build --env=prod` then `environment.prod.ts` will be used instead.
// The list of which env maps to which file can be found in `.angular-cli.json`.

export const environment = {
  production: false,
  logLevel: NgxLoggerLevel.TRACE,
  serverLogLevel: NgxLoggerLevel.OFF,

  userManagementUrl: 'https://localhost:5001',
  gigsManagementUrl: 'https://localhost:5057',
  billingManagementUrl: 'https://localhost:5051',
  orderingManagementUrl: 'https://localhost:5041',
  investingManagement: 'https://localhost:5031',

  chatUrl: 'https://localhost:6001/chat',
  chatApiUrl: 'https://localhost:6001/api/chat',

  clientRoot: 'http://localhost:4200',
  idpAuthority: 'https://localhost:7001',
  clientId: 'angular-client'
};