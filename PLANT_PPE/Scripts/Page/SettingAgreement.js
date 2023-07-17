$(document).ready(function () {
    getContent()
})

function getContent() {
    $.ajax({
        url: $("#web_link").val() + "/api/Setting/Get_Agreement", //URI
        dataType: "json",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            editor.setData(data.Data);
            var dateParts = data.DATE.split('T');
            var date = dateParts[0];
            var formattedDate = formatDate(date);
            document.getElementById('getModifiedDate').textContent = ' lastmodified: ' + formattedDate;
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

function formatDate(dateString) {
    var dateParts = dateString.split('-');
    var day = dateParts[2];
    var month = dateParts[1];
    var year = dateParts[0];
    return day + '-' + month + '-' + year;
}
