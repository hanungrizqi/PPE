/*Codebase.helpersOnLoad(['js-ckeditor5']);*/

$(document).ready(function () {
    getContent()
})

function getContent() {
    $.ajax({
        url: $("#web_link").val() + "/api/Setting/Get_Agreement", //URI
        /*data: JSON.stringify(obj),*/
        dataType: "json",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            editor.setData(data.Data);
        },
        error: function (xhr) {
            alert(xhr.responseText);
        }
    })
}

function submitEditor() {
    debugger
    let obj = new Object
    obj.ID = 99;
    obj.CONTENT = editor.getData();
    
    $.ajax({
        url: $("#web_link").val() + "/api/Setting/Create_Agreement", //URI
        data: JSON.stringify(obj),
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            debugger
            if (data.Remarks == true) {
                Swal.fire({
                    title: 'Saved',
                    text: "Data has been Saved.",
                    icon: 'success',
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK',
                    allowOutsideClick: false,
                    allowEscapeKey: false
                }).then((result) => {
                    if (result.isConfirmed) {
                        window.location.href = "/Setting/Agreement";
                    }
                });
            } if (data.Remarks == false) {
                Swal.fire(
                    'Error!',
                    'Message : ' + data.Message,
                    'error'
                );
            }

        },
        error: function (xhr) {
            alert(xhr.responseText);
        }
    })
}
