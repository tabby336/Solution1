function startUpThings() {
    loadProfileData();
}

function loadProfileData() {
    var url = "/Player";//?id=bade8051-f56d-4187-9726-8694c9ca6aee
    $("#profiledata").load(url, function() {
        $(".dropdown-button")
        .dropdown({
            inDuration: 300,
            outDuration: 125,
            constrain_width: !0,
            hover: !1,
            alignment: "left",
            gutter: 0,
            belowOrigin: !0
        });
    });
}

$(startUpThings);