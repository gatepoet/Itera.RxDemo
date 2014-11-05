$(function () {
    var myHub = $.connection.rxHub;
    $counter = $('#counter');
    myHub.server.observe('Time').subscribe(function (x) {
        $counter.html(x);
    });
    myHub.server.observe('Time')
        .where(function (x, i) {  return i % 5 === 0; })
        .subscribe(function (x) {
            $counter.css("background-color", "yellow");
            setTimeout(function () { $counter.css("background-color", "transparent"); }, 100);
        });
    $.connection.hub.start();

});