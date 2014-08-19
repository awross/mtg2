// Main configuration file. Sets up AngularJS module and routes and any other config objects

var appRoot = angular.module('main', [
    'ngRoute',
    'ngGrid',
    'ngResource',
    'app.services',
    'angularStart.directives',
    'main.filters',
    'ngStorage'
]);     //Define the main module

appRoot.value('$', $);

appRoot
    .config(['$routeProvider', function ($routeProvider) {
        //Setup routes to load partial templates from server. TemplateUrl is the location for the server view (Razor .cshtml view)
        $routeProvider
            .when('/home', { templateUrl: '/Decko/home/main', controller: 'MainController' })
            .when('/admin', { templateUrl: '/Decko/admin', controller: 'AdminController' })
            .when('/settings', { templateUrl: '/Decko/home/settings', controller: 'SettingsController' })
            .when('/chat', { templateUrl: '/Decko/home/chat', controller: 'ChatController' })
            .when('/contact', { templateUrl: '/Decko/home/contact', controller: 'ContactController' })
            .when('/about', { templateUrl: '/Decko/home/about', controller: 'AboutController' })
            .when('/demo', { templateUrl: '/Decko/home/demo', controller: 'DemoController' })
            .when('/build', { templateUrl: '/Decko/home/build', controller: 'BuildController' })
            .when('/lobby', { templateUrl: '/Decko/home/lobby', controller: 'LobbyController' })
            .when('/game/:gameId', { templateUrl: '/Decko/home/game', controller: 'GameController' })
            .when('/angular', { templateUrl: '/Decko/home/angular' })
            .otherwise({ redirectTo: '/home' });
    }])
    .controller('RootController', ['$scope', '$rootScope', '$route', '$routeParams', '$location', '$', 'jsonFactory', 'SignalRSvc', function ($scope, $rootScope, $route, $routeParams, $location, $, jsonFactory, SignalRSvc) {
        $scope.$on('$routeChangeSuccess', function (e, current, previous) {
            $scope.activeViewPath = $location.path();
        });

        $rootScope.prefix = angular.element(document.querySelector('#prefix'));

        SignalRSvc.initialize();


        //$scope.$on('AuthInfo', function (event, info) {
        //    $scope.user = info;
        //    $scope.$apply();
        //});

        var data = $.parseJSON($("#data").val());

        $rootScope.username = data.username;
        $rootScope.userUID = data.userUID;

        $rootScope.navs = data.navs;
        $rootScope.progress = data.progress;
        $rootScope.notes = data.notes;
        $rootScope.messages = data.messages;
    }]);

angular.module('main.filters', [])
    .filter('joinBy', function () {
        return function (input, delimiter) {
            return (input || []).join(delimiter || ',');
        };
    });

appRoot.service('SignalRSvc', function ($, $rootScope) {
    var proxy = null;

    var initialize = function () {
        //Getting the connection object
        this.proxy = $.connection.deckoHub;

        //$.extend(this.proxy.client, {
        //    AuthInfo: function (authInfo) {
        //        $rootScope.$broadcast('AuthInfo', authInfo);
        //    }
        //});

        //this.proxy.on('AuthInfo', function (message) {
        //    alert();
        //});

        //Starting connection
        $.connection.hub.start().done(function () {
            console.log("Connected to DeckoHub");
        });
    };

    var sendRequest = function () {
        //Invoking greetAll method defined in hub
        this.proxy.invoke('Hello');
    };

    return {
        initialize: initialize,
        sendRequest: sendRequest
    };
});
