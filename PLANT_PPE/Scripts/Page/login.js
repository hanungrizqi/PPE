var e = Swal.mixin({ buttonsStyling: !1, customClass: { confirmButton: "btn btn-alt-success m-5", cancelButton: "btn btn-alt-danger m-5", input: "form-control" } });

$("document").ready(function () {
    addRoled();
})

function PostLogin() {
    var obj = new Object();
    obj.Username = $("#login-username").val();
    obj.Password = $("#login-password").val();
    obj.Roled = $("#roled").val();
    $.ajax({
        url: $("#web_link").val() + "/api/Login/Get_Login", //URI
        data: JSON.stringify(obj),
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {
            debugger
            $("#overlay").show();
        },
        success: function (data) {
            if (data.Remarks == true) {
                MakeSession(obj.Username, obj.Roled);
            }
            else {
                swal.fire({
                    title: "Error!",
                    text: "Username or Password incorrect.",
                    icon: 'error',
                });
                $("#overlay").hide();
            }

        },
        error: function (xhr) {
            swal.fire({
                title: "Error!",
                text: 'Message : ' + xhr.responseText,
                icon: 'error',
            });
        }
    })
}

function MakeSession(nrp, role) {
    var obj = {
        NRP: nrp,
        Roled: role
    };

    $.ajax({
        type: "POST",
        url: "/Login/MakeSession", //URI
        dataType: "json",
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.Remarks == true) {
                window.location.href = "/Home/index";
            }
            else {
                swal.fire({
                    title: "Error!",
                    text: data.Message,
                    icon: 'error',
                });
                $("#overlay").hide();
            }
        },
        error: function (xhr) {
            alert(xhr.responseText);
        }
    });

}

function addJobsite() {
    $.ajax({
        url: $("#web_link").val() + "/api/Master/Get_JobsiteByUsername?username=" + $("#login-username").val(), //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            $('#jobSite').empty();
            text = '<option></option>';
            $.each(result.Data, function (key, val) {
                text += '<option value="' + val.DSTRCT_CODE + '">' + val.DSTRCT_CODE + '</option>';
            });
            $("#jobSite").append(text);
        }
    });
}

function addRoled() {
    $.ajax({
        url: $("#web_link").val() + "/api/Master/Get_Roled", //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            $('#roled').empty();
            text = '<option></option>';
            $.each(result.Data, function (key, val) {
                text += '<option value="' + val.RoleName + '">' + val.RoleName + '</option>';
            });
            $("#roled").append(text);
        }
    });
}
