$(function () {
    var myHub = $.connection.chatHub;

    $.connection.hub.start();
    var subscription;
    $("#join-chat-room").click(function () {
        var room = $("#room").val();

        if (subscription)
            subscription.isStopped = true;

        subscription = myHub.server
           .observe(room + "-messages")
           .subscribe(function (message) {
               console.log(message);
               $("#chat-section").append("<span style='font-weight: bold'>" + message.From + "</span> says:<br /><span>" + message.Text + "</span><br />")
           });

        $("#room-name").text(room);

        myHub.server
           .observe(room + "-users")
           .subscribe(function (message) {
               $("#participants").text(message);
           });

        myHub.server.join(
            room,
            $("#username").val());

    });
    $("#chat-input").keydownAsObservable()
        .where(function (e) {
            return e.keyCode === 13;
        })
        .subscribe(function () { $("#send-message").click(); });

    $("#send-message").click(function () {
        myHub.server.send(
            $("#room").val(),
            $("#chat-input").val());
    });

});