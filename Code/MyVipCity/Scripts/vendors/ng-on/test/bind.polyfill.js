Function.prototype.bind = function() {
  var args = [].slice.call(arguments),
      fn = this,
      context = args.shift();

  return function() {
    fn.apply(this, args);
  };
};

