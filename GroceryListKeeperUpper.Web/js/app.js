'use strict';

var app = angular.module('app', ['ngRoute', 'appControllers', 'appServices', 'xeditable', 'ui.bootstrap']);

var appServices = angular.module('appServices', []);
var appControllers = angular.module('appControllers', []);

var options = {};
    options.api = {};
    options.api.base_url = 'http://localhost:12009/api/v1';

app.config(['$locationProvider', '$routeProvider', function($locationProvider, $routeProvider){
	$routeProvider.
		when('/', {
			templateUrl: 'partials/index.html',
			controller: 'indexController'
		}).
        when('/lists/:listid', {
            templateUrl: 'partials/listItems.html',
            controller: 'listItemController'
        }).

		when('/login', {
			templateUrl: 'partials/login.html',
			controller: 'loginController'
		}).
		otherwise({
			redirectTo: '/'
		});

}]);

app.run(function ($rootScope, $location, editableOptions) {
    $rootScope.location = $location;
    editableOptions.theme = 'bs3';
});