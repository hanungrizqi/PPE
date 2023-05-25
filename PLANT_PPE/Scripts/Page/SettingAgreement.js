//Codebase.helpersOnLoad(['js-ckeditor', 'js-simplemde']);
/*Codebase.helpersOnLoad(['js-ckeditor5']);*/

$(document).ready(function () {
    getContent()
    //let editor;
    //ClassicEditor.create(document.querySelector('#editor'))
    //    .then(neweditor => {
    //        editor = neweditor
    //    })
    //    .catch(error => { console.log(error) })
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
    obj.ID = 99
    obj.CONTENT = editor.getData();
    
    //debugger
    $.ajax({
        url: $("#web_link").val() + "/api/Setting/Create_Agreement", //URI
        data: JSON.stringify(obj),
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            debugger
            if (data.Remarks == true) {
                Swal.fire(
                    'Saved!',
                    'Data has been Saved.',
                    'success'
                );
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
