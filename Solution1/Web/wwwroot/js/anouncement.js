var host = "http://" + $("#hostName").val();
var anouncementGetUrl = host + "/Anouncement";

var getHtmlTemplate = function (title, text) {
    var template = '<div class="recent-activity-list chat-out-list" style="padding-top: 5px;">' +
              '<div class="card  teal lighten-2">' +
                  '<div class="card-content white-text">' +
                      '<span class="card-title">' + title + '</span>' +
                      '<p>' +
                          text +
                      '</p>' +
                  '</div>' +
              '</div>' +
          '</div>';
    return template;
}

var getAnouncements = function ()
{
    $.get(anouncementGetUrl,
    function (data) {
        for (var i = 0; i < data.length; i++) {
            $("#anouncements").append(getHtmlTemplate(data[i].title, data[i].text));
        }
    });
}

getAnouncements();
$("#colapseHeader").click(function() {
    getAnouncements();
});
