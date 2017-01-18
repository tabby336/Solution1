var host = "http://" + $("#hostName").val();
var getAnouncements = function ()
{
    $.get(anouncementGetUrl,
    function (data) {
        $("#anouncements").html(data);
    });
}

getAnouncements();
var anouncementGetUrl = host + "/Anouncement";
$("#colapseHeader").click(function() {
    getAnouncements();
});
