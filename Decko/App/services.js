// Define you service here. Services can be added to same module as 'main' or a seperate module can be created.

var appServices = angular.module('app.services', ['ngResource']);     //Define the services module

appServices.factory('jsonFactory', ['$http', '$rootScope', function ($http, $rootScope) {
    var service = {
        navs: function () {
            var url = $rootScope.prefix + 'Data/nav.json';
            var promise = $http.get(url).then(function (response) {
                return response.data;
            });
            return promise;
        }
    };
    return service;
}]);