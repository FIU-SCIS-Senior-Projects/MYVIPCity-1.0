;(function() {
  'use strict';

  angular
    .module('argshook.ngOn', [])
    .directive('ngOn', [ '$parse', ngOnDirective ]);


  function ngOnDirective($parse) {
    return {
      restrict: 'A',
      compile: function(e, attrs) {
        var ngOn = $parse(attrs.ngOn);

        return function(scope, element) {
          var eventsAndCallbacks = ngOn(scope);

          Object
            .keys(eventsAndCallbacks)
            .forEach(callHandler);

          function callHandler(eventName) {
            element.on(eventName, function(eventObj) {
              scope.$evalAsync(function() {
                callOrNot(eventName, eventObj);
              });
            });
          }

          function callOrNot(eventName, eventObj) {
            if (typeof eventsAndCallbacks[eventName] === 'function') {
              return eventsAndCallbacks[eventName].call(scope.$ctrl || scope, eventObj);
            }

            throw new Error('handler for event "' + eventName + '" is not a function');
          }
        };
      }
    };
  }
})();

