﻿Codebase.helpersOnLoad(['jq-select2']);

$("document").ready(function () {
    $('.select2-modal').select2({
        dropdownParent: $('.modal')
    });
})

var table = $("#tbl_district").DataTable({
    ajax: {
        url: $("#web_link").val() + "/api/Setting/Get_District",
        dataSrc: "Data",
    },
    "columnDefs": [
        { "className": "dt-center", "targets": [0, 1, 2, 3] }
    ],
    scrollX: true,
    columns: [
        { data: 'ACCT_PROFILE' },
        { data: 'DSTRCT_CODE' },
        { data: 'DSTRCT_LOC' },
        {
            //render: function (data, type, row) {
            //    action = `<div class="btn-group">`
            //    action += `<button type="button" onclick="deleteDstrct(${row.ID})" class="btn btn-sm btn-danger" title="Delete">Delete
            //                    </button>`
            //    action += `</div>`
            //    return action;
            //}
            data: 'ID',
            targets: 'no-sort', orderable: false,
            render: function (data, type, row) {
                action = `<div class="btn-group">`
                action += `<button type="button" value="${row.ACCT_PROFILE}" onclick="setAccountprofile(${row.ID}, this.value, '${row.DSTRCT_CODE}')" data-bs-toggle="modal" data-bs-target="#modal_update" class="btn btn-sm btn-info" title="Edit">Edit
                                </button>`
                action += `<button type="button" onclick="deleteSource(${row.ID})" class="btn btn-sm btn-danger" title="Delete">Delete
                                </button>`
                action += `</div>`
                return action;
            }
        }

    ],

});

function insertDistrict() {
    let obj = new Object();
    obj.DSTRCT_CODE = $('#txt_dstrct').val(); 
    obj.LOCATION = $('#txt_location').val();

    console.log(obj);

    $.ajax({
        url: $("#web_link").val() + "/api/Setting/Create_District", //URI
        data: JSON.stringify(obj),
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.Remarks == true) {
                Swal.fire(
                    'Saved!',
                    'Data has been Saved.',
                    'success'
                );
                $('#modal-insert').modal('hide');
                $('.select2-modal').val('').trigger('change');
                $('.form-control').val('');

                table.ajax.reload();
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

function deleteDstrct(id) {
    Swal.fire({
        title: "Are you sure?",
        text: "You will not be able to recover this data!",
        icon: "warning",
        showCancelButton: !0,
        customClass: { confirmButton: "btn btn-alt-danger m-1", cancelButton: "btn btn-alt-secondary m-1" },
        confirmButtonText: "Yes, delete it!",
        html: !1,
        preConfirm: function (e) {
            return new Promise(function (e) {
                setTimeout(function () {
                    e();
                }, 50);
            });
        },
    }).then(function (n) {
        if (n.value == true) {
            $.ajax({
                url: $("#web_link").val() + "/api/Setting/Delete_District?id=" + id, //URI
                type: "POST",
                success: function (data) {
                    if (data.Remarks == true) {
                        Swal.fire("Deleted!", "Your Data has been deleted.", "success");
                        table.ajax.reload();
                    } if (data.Remarks == false) {
                        Swal.fire("Cancelled", "Message : " + data.Message, "error");
                    }

                },
                error: function (xhr) {
                    alert(xhr.responseText);
                }
            })
        } else {
            Swal.fire("Cancelled", "Your Data is safe", "error");
        }
    });
}

function setAccountprofile(id, source) {
    debugger
    $("#txt_id").val(id);
    $("#txt_accountprofile_update").val(source);
}

function updateAccountDL() {
    debugger
    let obj = new Object
    obj.ACCT_PROFILE = $('#txt_accountprofile_update').val();
    obj.ID = $('#txt_id').val();

    $.ajax({
        url: $("#web_link").val() + "/api/Setting/Update_Source", //URI
        data: JSON.stringify(obj),
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.Remarks == true) {
                Swal.fire(
                    'Saved!',
                    'Data has been Saved.',
                    'success'
                );
                $('#modal_update').modal('hide');
                table.ajax.reload();
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