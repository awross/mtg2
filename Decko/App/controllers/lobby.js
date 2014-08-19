angular.module('main')
    .controller('LobbyController', ['$scope', '$rootScope', '$', '$http', '$localStorage', '$document', 'lobbySvc', function ($scope, $rootScope, $, $http, $localStorage, $document, lobbySvc) {
        //$scope.$storage = $localStorage.$default({
        //    //games: []
        //});
        $scope.games = [];
        $scope.formats = $.parseJSON($("#Formats").val());
        $scope.structures = $.parseJSON($("#Structures").val());
        $scope.folders = $.parseJSON($("#folders").val());
        $scope.numPlayers = 2;

        $scope.DeckID = "";
        $scope.format = "";
        $scope.structure = "";

        $scope.FormatChanged = function () {
            console.log($scope.format);
        };

        $scope.$on("joinLobby", function (e, message) {
            $scope.$apply(function () {
                for (var i = 0; i < $scope.games.length; i++) {
                    var g = $scope.games[i];
                    if (g.LobbyID == message.LobbyID) {
                        g.PlayerList.push(message.UserName);
                        if (message.UserName == $rootScope.username) {
                            g.Closed = true;
                        }
                        break;
                    }
                }
            });
        });

        $scope.$on("addLobby", function (e, message) {
            $scope.$apply(function () {
                $scope.games.push(message);
            });
        });

        $scope.$on("addGame", function (e, message) {
            $scope.$apply(function () {
                AddGame(message);
            });
        });

        $scope.AddLobby = function () {
            lobbySvc.AddLobby({
                UserID: $rootScope.userUID,
                DeckID: $scope.DeckID,
                DeckName: "DECK_NAME_HERE",
                Structure: $scope.structure,
                Format: $scope.format,
                Players: $("#slider-snap-inc-amount").text(),
            });
        };

        $scope.JoinLobby = function (_lobbyID) {
            console.log("joining game...")
            lobbySvc.JoinLobby({
                UserID: $rootScope.userUID,
                LobbyID: _lobbyID,
                DeckID: $scope.DeckID,
                DeckName: "JOINING_DECK_NAME"
            });
        };

        $scope.IsReady = function (lobby) {
            return lobby.PlayerList.length == lobby.MaxPlayers;
        };
        $scope.IsClosed = function (lobby) {
            return lobby.PlayerList.indexOf($rootScope.username) >= 0 || lobby.PlayerList.length == lobby.MaxPlayers;
        };

        AddGame = function (game) {
            for (var i = 0; i < $rootScope.navs.length; i++) {
                if ($rootScope.navs[i].name == "Lobby") {
                    $rootScope.navs[i].SubNavs.push({
                        name: game.Name,
                        path: "#/game/" + game.UID,
                        icon: ""
                    });
                    break;
                }
            }
        };

        $document.ready(function () {
            $http.post("/Decko/lobby/list/na"
            ).success(function (data) {
                $scope.games = data;
            });
        });

        lobbySvc.initialize();

    }]);