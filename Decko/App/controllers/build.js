angular.module('main')
    .controller('BuildController', ['$scope', '$rootScope', '$timeout', '$', '$http', '$localStorage', function ($scope, $rootScope, $timeout, $, $http, $localStorage) {
        // This is what you will bind the Search to
        $scope.searchText = '';
        $scope.suggest = [];
        $scope.suggestActive = -1;
        $scope.$storage = $localStorage.$default({
            deckName: "New Deck",
            deckList: [],
            DeckInfo: {
                version: 0,
                format: "",
                currentFolder: "",
                parentID: "",
                editing: false
            },
            changed: false
        });

        $scope.tempDeckName = $scope.$storage.deckName;

        $scope.tempDeckInfo = {};
        $scope.$storage.DeckInfo.editing = false;

        $scope.DeckNameEditing = false;
        $scope.DeckNameActivate = function () {
            $scope.DeckNameEditing = true;
            $("#DeckNameEdit").focus();
            $scope.tempDeckName = $scope.$storage.deckName;
        };

        // Instantiate these variables outside the watch
        $scope.searchTextTimeout = false;

        $scope.DeckNameEditingComplete = function () {
            $scope.DeckNameEditing = false;
            if ($scope.tempDeckName != $scope.$storage.deckName) {
                $scope.$storage.DeckInfo.version = 0;
                $scope.$storage.changed = true;
            }
        };

        $scope.loadDeck = function (id) {
            $http.post("/Decko/deck/load/" + id).success(function(data) {
                $scope.$storage.DeckInfo = {
                    version: data.version,
                    format: data.format,
                    currentFolder: data.folderName,
                    parentID: data.parentID,
                    editing: false
                };
                $scope.$storage.deckName = data.name;
                var deck = $.parseJSON(data.decklist);
                $scope.$storage.deckList = [];
                for (var i = 0; i < deck.length; i++) {
                    var count = 1;
                    while (deck[i] == deck[i + 1]) {
                        count++;
                        i++;
                    }
                    $scope.AddCard(deck[i], count);
                };
                $scope.$storage.changed = false;
            });
        };

        $scope.saveDeck = function () {
            if ($scope.$storage.changed) {
                if ($scope.$storage.DeckInfo.currentFolder == "") {
                    $scope.tempDeckInfo = $scope.$storage.DeckInfo;
                    $scope.$storage.DeckInfo.editing = true;
                } else {
                    var saveList = [];
                    for (var i=0; i < $scope.$storage.deckList.length; i++) {
                        for (var j=0; j < $scope.$storage.deckList[i].Qty; j++) {
                            saveList.push($scope.$storage.deckList[i].id);
                        }
                    }
                    $scope.$storage.DeckInfo.editing = false;
                    $http.post(
                        "/Decko/api/deck/save",
                        {
                            user: $rootScope.userUID,
                            folder: $scope.$storage.DeckInfo.currentFolder,
                            name: $scope.$storage.deckName,
                            format: $scope.$storage.DeckInfo.format,
                            deckList: saveList,
                            parentID: $scope.$storage.DeckInfo.parentID,
                            version: parseInt($scope.$storage.DeckInfo.version)+1
                        }
                    ).success(function (data) {
                        $scope.$storage.DeckInfo.version = parseInt(data.version);
                        $scope.$storage.DeckInfo.parentID = data.parentID;
                        var fIndex = -1;
                        for (var i = 0; i < $scope.folders.length; i++) {
                            if ($scope.folders[i].Name == data.folderName) {
                                fIndex = i;
                                break;
                            }
                        }
                        var f = $scope.folders[fIndex];
                        var dIndex = -1;
                        for (var j = 0; j < f.Decks.length; j++) {
                            if (f.Decks[j].Name == data.name) {
                                dIndex = j;
                                f.Decks[j] = {
                                    ID: data.id,
                                    Name: data.name
                                };
                                break;
                            }
                        }
                        if (dIndex < 0) {
                            $scope.folders[fIndex].Decks.push({
                                ID: data.id,
                                Name: data.name
                            });
                        }
                        $scope.$storage.changed = false;
                    });
                }
            }
        };

        $scope.deckInfo = function () {
            $scope.tempDeckInfo = $scope.$storage.DeckInfo;
            $scope.$storage.DeckInfo.editing = true;
        };

        $scope.deleteDeck = function () {
            $scope.$storage.deckName = "New Deck Name Here";
            $scope.$storage.deckList = [];
            $scope.$storage.DeckInfo = {
                version: 0,
                format: "",
                currentFolder: "",
                parentID: "",
                editing: false
            };
            $scope.$storage.changed = false;
        };

        $scope.cancelSave = function () {
            $scope.$storage.DeckInfo.editing = false;
            $scope.$storage.DeckInfo = $scope.tempDeckInfo;
        };

        $scope.folders = $.parseJSON($("#folders").val());
        $("#DeckFolderSelect").val($scope.$storage.DeckInfo.currentFolder);

        $scope.NewFolder = {
            Active: false,
            Name: "",
            Save: function () {
                $http({
                    method: 'POST',
                    url: "/Decko/deck/folder/new/" + $rootScope.userUID + "/" + $scope.NewFolder.Name
                }).success(function (data, status, headers, config) {
                    $scope.folders.push({
                        Name: $scope.NewFolder.Name,
                        Decks: []
                    });
                    $scope.NewFolder.Name = "";
                });
                $scope.NewFolder.Active = false;
            },
            Close: function () {
                this.Active = false;
                this.Name = "";
            },
            Activate: function () {
                $scope.NewFolder.Active = true;
                $("#NewFolderName").focus();
            }
        };

        $scope.search = {
            timer: false,
            term: "",
            active: false,
            displayClick: function (e) {
                console.log('search display clicked');
                $("#searchInput").focus();
                this.active = true;
            },
            keyDown: function (e) {
                console.log('key: ' + e.keyCode);
                if (e.keyCode >= 37 && e.keyCode <= 40) { //direction
                    e.preventDefault();
                    if ($scope.suggest.length > 0)
                    {
                        switch (e.keyCode) {
                            case 40: //Down
                                if ($scope.suggestActive > -1) {
                                    $scope.suggest[$scope.suggestActive].Active = false;
                                }
                                $scope.suggestActive += 1;
                                $scope.suggestActive %= $scope.suggest.length;
                                $scope.suggest[$scope.suggestActive].Active = true;
                                break;
                            case 38: //Up
                                if ($scope.suggestActive > -1) {
                                    $scope.suggest[$scope.suggestActive].Active = false;
                                }
                                $scope.suggestActive -= 1;
                                if ($scope.suggestActive < 0) {
                                    $scope.suggestActive += $scope.suggest.length;
                                }
                                $scope.suggestActive %= $scope.suggest.length;
                                $scope.suggest[$scope.suggestActive].Active = true;
                                break;
                            case 37: //Left
                            case 39: //Right
                            default:
                                break;
                        };
                    }
                } else if (e.keyCode == 27) { // Escape
                    e.preventDefault();
                    $scope.search.term = "";
                    $scope.searchText = "";
                    $scope.suggestActive = -1;
                } else if (e.keyCode == 13) { // Enter
                    e.preventDefault();

                    if ($scope.suggest.length > 0 && $scope.suggestActive > -1) {
                        $scope.AddCard($scope.suggest[$scope.suggestActive].id);
                    }
                } else {
                    $scope.suggestActive = -1;
                }
            },
            searchBlur: function (e) {
                console.log('blurred');
                this.active = false;
            }
        };

        $scope.AddCard = function (id, count) {
            $scope.$storage.changed = true;
            count = count || 1;
            $http.post("/Decko/cards/deck/" + id)
            .success(function (data, status, headers, config) {
                var i = -1;
                for (var j = 0; j < $scope.$storage.deckList.length; j++) {
                    if ($scope.$storage.deckList[j].id == data.id) { i = j; }
                }
                if (i != -1) {
                    $scope.$storage.deckList[i].Qty += count;
                    if ($scope.$storage.deckList[i].Qty < 1)
                    {
                        $scope.$storage.deckList[i].Qty = 1;
                    }
                } else {
                    data.Qty = count;
                    $scope.$storage.deckList.push(data);
                }
            }).error(function (data, status, headers, config) {
                console.log(status + " - " + data);
            });
        };

        $scope.RemoveCard = function (id) {
            $scope.$storage.changed = true;
            var i = -1;
            for (var j = 0; j < $scope.$storage.deckList.length; j++) {
                if ($scope.$storage.deckList[j].id == id) { i = j; }
            }
            if (i != -1) {
                $scope.$storage.deckList[i].Qty--;
            }
        };

        $scope.$watch('searchText', function (val) {
            if ($scope.searchTextTimeout) $timeout.cancel($scope.searchTextTimeout);

            $scope.search.term = val;
            $scope.searchTextTimeout = $timeout(function () {
                $scope.searchText = $scope.search.term;
                if ($scope.searchText.length > 3)
                {
                    $http({
                        method: 'POST',
                        url: "/Decko/cards/name/" + $scope.searchText
                    }).success(function (data, status, headers, config) {
                        $scope.suggest = data;
                    });
                    //$.post(
                    //    "/Decko/cards/name/" + $scope.searchText,
                    //    function (data) {
                    //        $scope.$apply(function () {
                    //            $scope.suggest = data;
                    //        });
                    //    }
                    //);
                } else {
                    $scope.suggest = [];
                    $scope.suggestActive = -1;
                }
            }, 1000);
        })

    }]);