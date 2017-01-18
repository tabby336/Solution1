var host = "http://" + $("#hostName").val();
var anouncementGetUrl = host + "/Anouncement";
$("#colapseHeader").click(function() {
    $.get(anouncementGetUrl,
    function (data) {
        $("#anouncements").html(data);
    });
});
