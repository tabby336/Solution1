$(document)
	.ready(function () {

	    Validation();

	    //
	    $("#submitBtn")
			.click(function () {

			    var isValid = $('#loginForm')
					.valid();

			    if (isValid)
			        doLogin();
			});
	});

function doLogin() {
    var _username = $('#userTB').val();
    var _password = $('#passTB').val();

    var postForm = { //Fetch form data
        username: _username.toString(),
        password: _password.toString()
    };

    $.ajax({
        type: "POST", // type of request
        url: '/auth/local', //path of the request
        data: postForm, //the data to send to the server (JSON)
        contentType: "application/x-www-form-urlencoded;odata=verbose", // data content type (header)

        // the function called on Success (no error returned bu the server)
        success: function (result) {
            // success on login, so redirect. This does not affect the session. If user tricks this, still cannot access the game because of the policies.
            if (result.status == 1) {
                window.location.href = "/game";
            }

        },
        // the function called on error (error returned from server or TimeOut Expired)
        error: function (err) {

            var response = JSON.parse(err.responseText);
            var $htmlToDisplay = $('<span class="white-text" id = "error-message">' + response.message + '</span>');
            Materialize.toast($htmlToDisplay, 2000, 'card-panel red'); //create a toast 1\with class = card-panel red class, 2 seconds
        },
        timeout: 3000 // the time limit to wait for a response from the server, milliseconds
    });
}



function Validation() {
    $('#loginForm')
		.validate({
		    rules: {
		        userTB: {
		            required: true,
		            minlength: 3,
		            maxlength: 50
		        },
		        passTB: {
		            required: true,
		            minlength: 6
		        }
		    },
		    messages: {
		        userTB: {
		            required: "*Please fill in your user name.",
		            minlength: "*Your username was created with at least 3 characters.",
		            maxlength: "*Your username was created with at most 50 characters."
		        },
		        passTB: {
		            required: "*SO, you dont't have a password?",
		            minlength: "*Your created password was created as decent (at least 6 characters long)."
		        }
		    },
		    validClass: 'valid',
		    errorClass: 'invalid error',
		    errorElement: 'div',
		    errorPlacement: function (error, element) {
		        var placement = $(element)
					.data('error');
		        if (placement) {
		            $(placement)
						.append(error);
		        } else {
		            error.insertAfter(element);
		        }
		    }
		});
}