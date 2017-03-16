# ng-on

## `<div ng-on="{ 'event-name': handlerFn }"></div>`

Directive for Angular 1.x to easily bind custom (or not) events

Angular provides some useful directives for attaching event handlers easily:

* `ng-click="$ctrl.clickHandler()"`;
* `ng-change="$ctrl.changeHandler($event)"`;
* `ng-mouseenter="$ctrl.mouseenterHandler($event)"` etc.

But for custom events it's a tad more work, you need to set event listener on element yourself.

You might do something like this:

```js
angular
    .module('myModule')
    .directive('myDirective', function() {
        return {
            link: function(scope, element) {
                element.on('custom-event', function(event) {
                    console.log('handling custom event', event.type);
                });
            }
        };
    });
```

or do something similar in controller and injected `$element`.

this is fine for few bindings but gets tedious for setting up many events on many different elements.

Enter the angularisher way of doing this with `ng-on`!

```html
<div ng-on="{ 'event-name': handlerFn }"></div>
```

* `event-name` - name of event you want to listen to. Can be any event that element fires (also regular `click`, `mouseenter` etc.);
* `handlerFn` - a reference to function which is in components scope (i.e. controller of directive/component must expose this function). The function is called with `event` attribute.


## Installation

* `npm i ng-on --save`
* `angular.module('yourModule', ['argshook.ngOn'])`


## Examples


notice the use of `component` no need for `link` anymore:

```js
angular
    .module('myModule')
    .component('myComponent', {
        template: '<div ng-on="{ \'custom-event\': $ctrl.customEventHandler }">{{$ctrl.eventValue}}</div>',
        controller: function() {
            this.customEventHandler = function(event) {
                this.eventValue = event.value;
            };
        }
    });
```

you can also define multiple event handlers (this time `ng-on` value is in controller):

```js
angular
    .module('myModule')
    .component('myComponent', {
        template: '<div ng-on="$ctrl.eventsObj">{{$ctrl.eventValue}}</div>',
        controller: function() {
            this.eventsObj = {
                'custom-event': customEventHandler
                'another-custom-event': anotherCustomEventHandler,
                click: clickHandler // you can handle regular DOM events too
            };

            function customEventHandler(event) {
                this.eventValue = event.value;
            };

            this.anotherCustomEventHandler = function(event) {
                console.log(event.type); // <= another-custom-event
            };

            this.clickHandler = function() {
                console.log(event.type); // <= click
            };
        }
    });
```
