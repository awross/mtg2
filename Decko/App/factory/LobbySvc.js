appRoot.service('lobbySvc', function ($, $rootScope, $timeout) {
    var proxy = null;
    var connTimeout = false;

    return {
        initialize: function () {
            //Creating proxy
            this.proxy = $.connection.lobbyHub;

            //Publishing an event when server pushes a greeting message
            this.proxy.on('addLobby', function (message) {
                $rootScope.$broadcast("addLobby", message);
            });

            this.proxy.on('joinLobby', function (lobbyID, _username) {
                $rootScope.$broadcast("joinLobby", { LobbyID: lobbyID, UserName: _username });
            });

            this.proxy.on('addGame', function (game) {
                $rootScope.$broadcast("addGame", game);
            });

            this.proxy.connection.stateChanged(function (state) {
                if (this.connTimeout) $timeout.cancel(this.connTimeout);
                var theProxy = this.proxy;
                var stateConversion = {
                    0: 'connecting',
                    1: 'connected',
                    2: 'reconnecting',
                    4: 'disconnected'
                };
                var msg = 'lobbySvc from:' + stateConversion[state.oldState] + ' to:' + stateConversion[state.newState];
                console.log(msg);
                if (state.newState == 1) {
                    $.gritter.removeAll();
                }
                $.gritter.add({
                    title: 'SignalR Connection Change',
                    text: msg,
                    sticky: (state.newState == 2 || state.newState == 0)
                });
                if (state.newState == 4) {
                    this.connTimeout = $timeout(function () {
                        $.connection.hub.start().done(function() {
                            theProxy = $.connection.lobbyHub;
                        });
                    }, 1000);
                }
            });

            $.connection.hub.start({ waitForPageLoad: false }).done(function () {
                console.log("Lobby started")
            });


        },
        AddLobby: function (newLobby) {
            //Invoking greetAll method defined in hub
            this.proxy.invoke('addLobby', newLobby);
        },
        JoinLobby: function (joinLobby) {
            this.proxy.invoke('joinLobby', joinLobby);
        }
    };
});