$(function () {
    $("#search").keyupAsObservable()
        .select(function (e) { return e.currentTarget.value; })
        .where(function (text) { return text.length > 1; })
        .throttle(500)
        .subscribe(function (text) {
            $.post("/api/Values", '=' + text)
                .done(function (data) {
                    $("#search-results").html(data.join('<br/>'));
                })
        });
});