// Karma configuration
// Generated on Mon Dec 01 2014 17:39:39 GMT+0200 (South Africa Standard Time)

module.exports = function(config) {
  config.set({

    // base path that will be used to resolve all patterns (eg. files, exclude)
    basePath: '',


    // frameworks to use
    // available frameworks: https://npmjs.org/browse/keyword/karma-adapter
    frameworks: ['jasmine'],


    // list of files / patterns to load in the browser
    files: [
	  // 3rd party dependencies
      'LendingLibrary.Web/js/**/*.js',
	  // test specific
      'LendingLibrary.Web.Tests/js/**/*.js',
      'LendingLibrary.Web/Scripts/jquery-2.2.3.js',
	  'LendingLibrary.Web/Scripts/jquery-ui-1.11.4.js',
      'LendingLibrary.Web/Scripts/sweetalert.min.js',
      'LendingLibrary.Web/Scripts/jquery.redirect.js'
    ],


    // list of files to exclude
    exclude: [    
	'LendingLibrary.Web/Scripts/js/main.js'
    ],


    // preprocess matching files before serving them to the browser
    // available preprocessors: https://npmjs.org/browse/keyword/karma-preprocessor
    preprocessors: {
        '**/js/**/*.js': 'coverage'
    },


    // test results reporter to use
    // possible values: 'dots', 'progress'
    // available reporters: https://npmjs.org/browse/keyword/karma-reporter
    reporters: ['progress', 'coverage'],
	
    junitReporter: {
		outputDir: '../buildreports/',
        outputFile: 'test-results.xml'
    },
	
    coverageReporter: {
        reporters: [
            {type: 'json'},
            {type: 'lcov'},
            {type: 'cobertura'}
        ],
        dir: 'buildreports/'
    },


    // web server port
    port: 9876,


    // enable / disable colors in the output (reporters and logs)
    colors: true,


    // level of logging
    // possible values: config.LOG_DISABLE || config.LOG_ERROR || config.LOG_WARN || config.LOG_INFO || config.LOG_DEBUG
    logLevel: config.LOG_INFO,


    // enable / disable watching file and executing tests whenever any file changes
    autoWatch: true,


    // start these browsers
    // available browser launchers: https://npmjs.org/browse/keyword/karma-launcher
    browsers: ['Chrome'],


    // Continuous Integration mode
    // if true, Karma captures browsers, runs the tests and exits
    singleRun: false
  });
};
