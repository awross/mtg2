angular.module('main')
.controller('GameController', ['$scope', '$rootScope', '$routeParams', '$', 'gameSvc', '$http', '$localStorage', '$document', function ($scope, $rootScope, $routeParams, $, gameSvc, $http, $localStorage, $document) {
    //Activate = function () {
    //    console.log("Game activated!!");
    //};

    //$scope.$on("Activate", function (e, message) {
    //    $scope.$apply(function () {
    //        Activate();
    //    });
    //});
    $scope.$storage = $localStorage.$default({
        cards: {}
    });

    //$scope.$storage.cards = {};

    $scope.CardName = function (cardID) {
        if (typeof ($scope.$storage.cards[cardID]) === 'undefined') {
            $scope.$storage.cards[cardID] = {
                id: cardID,
                multiverseID: 0,
                setcode: "XXX",
                layout: "",
                typeline: "",
                type: "",
                subtype: "",
                name: "",
                originalTypeline: "",
                cmc: "",
                rarity: "",
                artist: "",
                color: "",
                cost: "",
                text: "",
                originalText: "",
                flavor: "",
                imageName: "",
                power: "",
                toughness: ""
            };
            $http.post("/Decko/card/id/" + cardID)
            .success(function (data) {
                $scope.$storage.cards[cardID] = data;
            });
        }
        return $scope.$storage.cards[cardID].name;
    };
    $scope.CardCost = function (cardID) {
        if (typeof ($scope.$storage.cards[cardID]) === 'undefined') {
            $scope.$storage.cards[cardID] = {
                id: cardID,
                multiverseID: 0,
                setcode: "XXX",
                layout: "",
                typeline: "",
                type: "",
                subtype: "",
                name: "",
                originalTypeline: "",
                cmc: "",
                rarity: "",
                artist: "",
                color: "",
                cost: "",
                text: "",
                originalText: "",
                flavor: "",
                imageName: "",
                power: "",
                toughness: ""
            };
            $http.post("/Decko/card/id/" + cardID)
            .success(function (data) {
                $scope.$storage.cards[cardID] = data;
            });
        }
        return $scope.$storage.cards[cardID].cost;
    };

    $scope.game = null;
    $scope.player = null;
    $scope.gameID = $routeParams.gameId;

    $scope.ShowMsg = false;

    $scope.height = $(window).height() - 40;
    $scope.handHeight = 96;
    $scope.tableHeight = function() {
        return $scope.height - $scope.handHeight;
    };

    //$(window).resize(function () {

    //});
    
    $scope.infoMode = "";
    $scope.infoText = "";

    $scope.ActivePlayer = function () {
        var a = "";
        if (typeof ($scope.game) !== 'undefined' && $scope.game != null) {
            a = $scope.game.ActivePlayer;
        }
        return $rootScope.username == a;
    };

    GameSetup = function () {
        $scope.infoMode = $scope.game.StateStr;
        $scope.infoText = "";
        switch ($scope.game.StateStr) {
            case "COINFLIP":
                if ($scope.ActivePlayer()) {
                    $scope.infoText = "Would you like to play first?";
                } else {
                    $scope.infoText = "Waiting for " + $scope.game.ActivePlayer + " to choose whether or not to play...";
                }
                break;
            default:
                break;
        };
    };

    $scope.playDecide = function (d) {
        $http.post("/Decko/game/StartGame/" + $scope.gameID + "/" + d)
        .success(function (data) {
            console.log(data);
            console.log("Starting game");
        });
    };

    $scope.mulligan = function () {
        gameSvc.mulligan({
            gameID: $scope.gameID,
            playerID: $rootScope.userUID
        });
    };

    $scope.keepHand = function () {
        gameSvc.keepHand({
            gameID: $scope.gameID,
            playerID: $rootScope.userUID
        });
    };

    $scope.$on("getState", function (e, message) {
        $scope.$apply(function () {
            $http.post("/Decko/game/GetState/", $scope.gameID + "/" + $rootScope.userUID)
            .success(function (data) {
                $scope.game = data.Game;
                $scope.player = data.Player;
                GameSetup();
            });
        });
    });



    $document.ready(function () {
        $http.post("/Decko/game/GetState/" + $scope.gameID + "/" + $rootScope.userUID)
        .success(function (data) {
            $scope.game = data.Game;
            $scope.player = data.Player;
            GameSetup();
        });
        //$http.post("/Decko/game/" + $scope.gameID)
        //.success(function (data) {
        //    $scope.game = data;
        //    GameSetup();
        //});
    });

    gameSvc.initialize();
}]);