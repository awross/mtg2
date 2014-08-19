appRoot.service('gameSvc', function ($, $rootScope) {
    var proxy = null;

    return {
        initialize: function () {
            //Creating proxy
            this.proxy = $.connection.gameHub;

            //Publishing an event when server pushes a greeting message
            this.proxy.on('getState', function (/*message*/) {
                $rootScope.$broadcast("getState"/*, message*/);
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
                var msg = 'gameSvc from:' + stateConversion[state.oldState] + ' to:' + stateConversion[state.newState];
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
                        $.connection.hub.start().done(function () {
                            theProxy = $.connection.gameHub;
                        });
                    }, 1000);
                }
            });

            $.connection.hub.start().done(function () {
                console.log("GameHub started")
            });
        },
        mulligan: function (message) {
            //Invoking greetAll method defined in hub
            this.proxy.invoke('mulligan', message);
        },
        keepHand: function (message) {
            //Invoking greetAll method defined in hub
            this.proxy.invoke('keepHand', message);
        }
    };
});