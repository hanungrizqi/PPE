Codebase.helpersOnLoad(['jq-select2']);

$("document").ready(function () {
    $('.select2-modal').select2({
        dropdownParent: $('.modal')
    });
})

var table = $("#tbl_accprofile").DataTable({
    ajax: {
        url: $("#web_link").val() + "/api/Setting/Get_AccountProfile",
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
            data: 'ID',
            targets: 'no-sort', orderable: false,
            render: function (data, type, row) {
                action = `<div class="btn-group">`
                action += `<button type="button" value="${row.DSTRCT_CODE}" onclick="setSource(${row.ID}, this.value)" data-bs-toggle="modal" data-bs-target="#modal_update" class="btn btn-sm btn-info" title="Edit">Edit
                                </button>`
                action += `<button type="button" value="${row.ACCT_PROFILE}" onclick="deleteUser(${row.ID}, this.value)" class="btn btn-sm btn-danger" title="Delete">Delete
                                </button>`
                action += `</div>`
                return action;
            }
        }
    ],
    initComplete: function () {
        this.api()
            .columns(1)
            .every(function () {
                var column = this;
                var select = $('<select class="form-control form-control-sm" style="width:200px; display:inline-block; margin-left: 10px;"><option value="">-- DISTRICT --</option></select>')
                    .appendTo($("#tbl_accprofile_filter.dataTables_filter"))
                    .on('change', function () {
                        var val = $.fn.dataTable.util.escapeRegex($(this).val());

                        column.search(val ? '^' + val + '$' : '', true, false).draw();
                    });

                column
                    .data()
                    .unique()
                    .sort()
                    .each(function (d, j) {
                        select.append('<option value="' + d + '">' + d + '</option>');
                    });
            });
    },
});


function insertUser() {
    let obj = new Object();
    obj.ID_Role = $('#txt_group').val();
    obj.Username = $('#txt_nrp').val();

    $.ajax({
        url: $("#web_link").val() + "/api/Setting/Create_User", //URI
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

function deleteUser(role, nrp) {
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
                url: $("#web_link").val() + "/api/Setting/Delete_User?role=" + role + "&nrp=" + nrp, //URI
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

function setSource(id, distrik) {
    debugger
    $("#txt_id").val(id);
    $("#txt_distrik_update").val(distrik);
    $("#txt_lokasi_update").val(lokasi);
}

function updateSurce() {
    let obj = new Object
    obj.SOURCE = $('#txt_source_update').val();
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