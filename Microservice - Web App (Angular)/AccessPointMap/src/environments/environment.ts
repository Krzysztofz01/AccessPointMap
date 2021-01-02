// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  apiUrl: "localhost:3201",
  CACHE_ACCESSPOINTS: 'ACCESSPOINTS_ARRAY',
  CACHE_CHART_SECURITY: 'CHART_SECURITY',
  CACHE_CHART_FREQUENCY: 'CHART_FREQUENCY',
  CACHE_CHART_AREA: 'CHART_AREA',
  CACHE_CHART_BRANDS: 'CHART_BRANDS',
  CACHE_JWT: 'JWT'
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
