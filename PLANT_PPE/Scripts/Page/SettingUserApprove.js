Codebase.helpersOnLoad(['jq-select2']);


$("document").ready(function () {
    $('.select2-modal').select2({
        dropdownParent: $('#modal-insert') 
    });
})

var table = $("#tbl_usrapprv").DataTable({
    ajax: {
        url: $("#web_link").val() + "/api/Setting/Get_UserApproveSetting",
        dataSrc: "Data",
    },
    "columnDefs": [
        { "className": "dt-center", "targets": '_all' },
        { "className": "dt-nowrap", "targets": '_all' }
    ],
    scrollX: true,
    columns: [
        { data: 'Position_id' },
        { data: 'Employee_id' },
        { data: 'Name' },
        { data: 'sub_menu' },
        { data: 'dstrct_code' },
        {
            data: 'id',
            targets: 'no-sort', orderable: false,
            render: function (data, type, row) {
                action = `<div class="btn-group">`
                action += `<button type="button" onclick="deleteUser(${row.id})" class="btn btn-sm btn-danger" title="Delete">Delete
                                </button>`
                action += `</div>`
                return action;
            }
        }
    ],
    initComplete: function () {
        this.api().columns().every(function () {
            var column = this;
            var select = $('<input type="text" class="form-control form-control-sm" placeholder="Search..." />')
                .appendTo($(column.header()))
                .on('keyup', function () {
                    column.search(this.value).draw();
                });
        });
    },
});

$("#txt_nrp").on("change", function () {
    $.ajax({
        url: $("#web_link").val() + "/api/Master/Get_Employee/" + this.value,
        type: "GET",
        cache: false,
        success: function (result) {
            $("#txt_name").val(result.Data.NAME);
            $("#txt_positionID").val(result.Data.POSITION_ID);
            $("#txt_distrik").val(result.Data.DSTRCT_CODE);

        }
    });
})

function insertUserApprove() {
    let obj = new Object();
    obj.Employee_id = $('#txt_nrp').val();
    obj.Position_id = $('#txt_positionID').val();
    obj.Name = $('#txt_name').val();
    obj.sub_menu = $('#txt_menu').val();
    obj.dstrct_code = $('#txt_distrik').val();

    $.ajax({
        url: $("#web_link").val() + "/api/Setting/Create_UserApprove", //URI
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

function deleteUser(id) {
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
                url: $("#web_link").val() + "/api/Setting/Delete_UserApprove?id=" + id, //URI
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