$(function () {
    $(document).mousemoveAsObservable()
        .where(function (e) {
            return e.ctrlKey === true;
        })
        .select(function (e) {
            return {
                top: e.clientY,
                left: e.clientX
            };
        })
        .subscribe(function (offset) {
            $("#button").offset(offset);
        });

});