// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  SERVER_URL: 'http://localhost:5000',
  CLIENT_VERSION: '3.0-alpha',
  SERVER_VERSION: '4.0',
  PIN_ICON_GOOD: 'http://cdn.mapmarker.io/api/v1/pin?size=50&hoffset=1&background=%2368BC00',
  PIN_ICON_AVERAGE: 'http://cdn.mapmarker.io/api/v1/pin?size=50&hoffset=1&background=%23FB9E00',
  PIN_ICON_BAD: 'http://cdn.mapmarker.io/api/v1/pin?size=50&hoffset=1&background=%23F44E3B',
  PIN_ICON_ALTERNATIVE: 'http://cdn.mapmarker.io/api/v1/pin?size=50&hoffset=1&background=%230062B1'
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/plugins/zone-error';  // Included with Angular CLI.
