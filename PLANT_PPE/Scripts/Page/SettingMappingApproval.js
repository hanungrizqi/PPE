$("document").ready(function () {
    //approvalFrom()
    //approvalTo()
    approvalFrom(function () {
        approvalTo();
    });
    approvalAction()
    currPosID()
    nextPosID()
})

var table = $("#tbl_mappingapproval").DataTable({
    ajax: {
        url: $("#web_link").val() + "/api/Setting/Get_MappingApproval",
        dataSrc: "Data",
    },
    "columnDefs": [
        { "className": "dt-center", "targets": '_all' },
        { "className": "dt-nowrap", "targets": '_all' /*[1, 2]*/ }
    ],
    scrollX: true,
    columns: [
        {
            data: 'APPROVAL_ACTION',
            render: function (data, type, row) {
                if (data === 0) {
                    return 'REJECT';
                } else if (data === 1) {
                    return 'APPROVE';
                } else {
                    return '';
                }
            }
        },
        { data: 'APPROVAL_ORDER' },
        { data: 'APPROVAL_FROM' },
        { data: 'APPROVAL_TO' },
        { data: 'CURR_POSITION_ID' },
        { data: 'NEXT_POSITION_ID' },
        { data: 'APPROVAL_STATUS' },
        { data: 'CURRENT_STATUS' },
        {
            data: 'ID',
            targets: 'no-sort', orderable: false,
            render: function (data, type, row) {
                action = `<div class="btn-group">`
                action += `<button type="button" value="${row.APPROVAL_ACTION}" onclick="setAccountprofile(${row.ID}, this.value, '${row.APPROVAL_ORDER}', '${row.APPROVAL_FROM}', '${row.APPROVAL_TO}', '${row.CURR_POSITION_ID}', '${row.NEXT_POSITION_ID}', '${row.APPROVAL_STATUS}', '${row.CURRENT_STATUS}')" data-bs-toggle="modal" data-bs-target="#modal_update" class="btn btn-sm btn-info" title="Edit">Edit
                                </button>`
                action += `<button type="button" onclick="deleteDist(${row.ID})" class="btn btn-sm btn-danger" title="Delete">Delete
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

var selectedApprovalFrom;
function approvalFrom(callback) {
    $.ajax({
        url: $("#web_link").val() + "/api/Master/getDistrict_and_ALL", //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            $('#approval_from').empty();
            //text = '<option value="ALL">ALL</option>';
            text = '<option></option>';
            $.each(result.Data, function (key, val) {
                text += '<option value="' + val.DSTRCT_CODE + '">' + val.DSTRCT_CODE + '</option>';
            });
            $("#approval_from").append(text);
            $("#approval_from").change(function () {
                selectedApprovalFrom = $(this).val();
                approvalTo();
                if (typeof callback === "function") {
                    callback();
                }
            });
            $('#approval_from').select2({
                dropdownParent: $('#modal-insert')
            });
        }
    });
}

function approvalTo() {
    console.log($("#approval_from").val())
    debugger
    $.ajax({
        url: $("#web_link").val() + "/api/Master/getDistrict_and_ALL", //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            $('#approval_to').empty();
            //text = '<option value="ALL">ALL</option>';
            text = '<option></option>';
            $.each(result.Data, function (key, val) {
                if (val.DSTRCT_CODE !== selectedApprovalFrom) {
                    text += '<option value="' + val.DSTRCT_CODE + '">' + val.DSTRCT_CODE + '</option>';
                }
            });
            $("#approval_to").append(text);
            $('#approval_to').select2({
                dropdownParent: $('#modal-insert') 
            });
        }
    });
}

function approvalAction() {
    var selectElement = document.getElementById("approval_action");

    selectElement.innerHTML = '';

    var optionApprove = document.createElement("option");
    optionApprove.value = "1";
    optionApprove.text = "APPROVE";
    selectElement.appendChild(optionApprove);

    var optionReject = document.createElement("option");
    optionReject.value = "0";
    optionReject.text = "REJECT";
    selectElement.appendChild(optionReject);

    $(selectElement).select2({
        dropdownParent: $('#modal-insert') // This line attaches the Select2 dropdown to the modal
    });
}

function currPosID() {
    $.ajax({
        url: $("#web_link").val() + "/api/Setting/Get_Position", //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            $('#curr_pos_id').empty();
            text = '<option></option>';
            $.each(result.Data, function (key, val) {
                var optionText = val.EMPLOYEE_ID + " - " + val.POSITION_ID;
                text += '<option value="' + val.POSITION_ID + '">' + optionText + '</option>';

            });
            $("#curr_pos_id").append(text);
            $('#curr_pos_id').select2({
                dropdownParent: $('#modal-insert') // This line attaches the Select2 dropdown to the modal
            });
        }
    });
}

function nextPosID() {
    $.ajax({
        url: $("#web_link").val() + "/api/Setting/Get_Position", //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            $('#next_pos_id').empty();
            text = '<option></option>';
            $.each(result.Data, function (key, val) {
                var optionText = val.EMPLOYEE_ID + " - " + val.POSITION_ID;
                text += '<option value="' + val.POSITION_ID + '">' + optionText + '</option>';

            });
            $("#next_pos_id").append(text);
            $('#next_pos_id').select2({
                dropdownParent: $('#modal-insert') // This line attaches the Select2 dropdown to the modal
            });
        }
    });
}

function insertDist() {
    debugger

    let obj = new Object();
    obj.APPROVAL_ACTION = $('#approval_action').val();
    obj.APPROVAL_ORDER = $('#approval_order').val();
    obj.APPROVAL_FROM = $('#approval_from').val();
    obj.APPROVAL_TO = $('#approval_to').val();
    obj.CURR_POSITION_ID = $('#curr_pos_id').val();
    obj.NEXT_POSITION_ID = $('#next_pos_id').val();
    obj.APPROVAL_STATUS = $('#approval_status').val();
    obj.CURRENT_STATUS = $('#current_status').val();

    $.ajax({
        url: $("#web_link").val() + "/api/Setting/Create_MappingApproval", //URI
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

$('#modal-insert').on('hidden.bs.modal', function () {
    $(this)
        .find("input,textarea,select")
        .val('')
        .trigger('change') // For Select2 to trigger change event
        .end()
        .find("input[type=checkbox], input[type=radio]")
        .prop("checked", "")
        .end();
});

function deleteDist(id) {
    debugger
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
                url: $("#web_link").val() + "/api/Setting/Delete_MappingApproval?id=" + id, //URI
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

function setAccountprofile(id, aprv_action, aprv_order, aprv_from, aprv_to, cur_posid, next_posid, aprv_status, curr_stats) {
    debugger
    approvalFromUpdate(aprv_from);
    approvalToUpdate(aprv_to);
    currPosIDUpdate(cur_posid);
    nextPosIDUpdate(next_posid);
    approvalActionUpdate(aprv_action);

    $("#txt_id").val(id);
    $("#approval_order_update").val(aprv_order);
    $("#approval_status_update").val(aprv_status);
    $("#current_status_update").val(curr_stats);
}

function approvalActionUpdate(aprv_action) {
    debugger
    var selectElement = document.getElementById("approval_action_update");
    selectElement.innerHTML = '';

    var optionApprove = document.createElement("option");
    optionApprove.value = "1";
    optionApprove.text = "APPROVE";

    var optionReject = document.createElement("option");
    optionReject.value = "0";
    optionReject.text = "REJECT";

    if (aprv_action == "1") {
        optionApprove.selected = true;
        selectElement.appendChild(optionApprove);
        selectElement.appendChild(optionReject);
    } else if (aprv_action == "0") {
        optionReject.selected = true;
        selectElement.appendChild(optionReject);
        selectElement.appendChild(optionApprove);
    }

    $(selectElement).select2({
        dropdownParent: $('#modal_update')
    });
}


function approvalFromUpdate(aprv_from) {
    debugger
    $.ajax({
        url: $("#web_link").val() + "/api/Master/getDistrict_and_ALL", //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            debugger
            $('#approval_from_update').empty();
            text = '<option></option>';
            var dataStatus = aprv_from;
            //text += '<option value="ALL">ALL</option>';
            text = '<option></option>';
            $.each(result.Data, function (key, val) {
                if (val.DSTRCT_CODE == dataStatus) {
                    text += '<option selected value="' + val.DSTRCT_CODE + '">' + val.DSTRCT_CODE + '</option>';
                } else {
                    text += '<option value="' + val.DSTRCT_CODE + '">' + val.DSTRCT_CODE + '</option>';
                }
            });
            $("#approval_from_update").append(text);
            $('#approval_from_update').select2({
                dropdownParent: $('#modal_update')
            });
        }
    });
}

function approvalToUpdate(aprv_to) {
    debugger
    $.ajax({
        url: $("#web_link").val() + "/api/Master/getDistrict_and_ALL", //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            debugger
            $('#approval_to_update').empty();
            text = '<option></option>';
            var dataStatus = aprv_to;
            //text += '<option value="ALL">ALL</option>';
            text = '<option></option>';
            $.each(result.Data, function (key, val) {
                if (val.DSTRCT_CODE == dataStatus) {
                    text += '<option selected value="' + val.DSTRCT_CODE + '">' + val.DSTRCT_CODE + '</option>';
                } else {
                    text += '<option value="' + val.DSTRCT_CODE + '">' + val.DSTRCT_CODE + '</option>';
                }
            });
            $("#approval_to_update").append(text);
            $('#approval_to_update').select2({
                dropdownParent: $('#modal_update')
            });
        }
    });
}

function currPosIDUpdate(cur_posid) {
    debugger
    $.ajax({
        url: $("#web_link").val() + "/api/Setting/Get_Position", //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            debugger
            $('#curr_pos_id_update').empty();
            text = '<option></option>';
            var dataStatus = cur_posid;
            $.each(result.Data, function (key, val) {
                if (val.POSITION_ID == dataStatus) {
                    var optionText = val.EMPLOYEE_ID + " - " + val.POSITION_ID;
                    text += '<option selected value="' + val.POSITION_ID + '">' + val.EMPLOYEE_ID + " - " + val.POSITION_ID + '</option>';
                } else {
                    text += '<option value="' + val.POSITION_ID + '">' + val.EMPLOYEE_ID + " - " + val.POSITION_ID + '</option>';
                }
            });
            $("#curr_pos_id_update").append(text);
            $('#curr_pos_id_update').select2({
                dropdownParent: $('#modal_update')
            });
        }
    });
}

function nextPosIDUpdate(next_posid) {
    debugger
    $.ajax({
        url: $("#web_link").val() + "/api/Setting/Get_Position", //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            debugger
            $('#next_pos_id_update').empty();
            text = '<option></option>';
            var dataStatus = next_posid;
            $.each(result.Data, function (key, val) {
                if (val.POSITION_ID == dataStatus) {
                    var optionText = val.EMPLOYEE_ID + " - " + val.POSITION_ID;
                    text += '<option selected value="' + val.POSITION_ID + '">' + val.EMPLOYEE_ID + " - " + val.POSITION_ID + '</option>';
                } else {
                    text += '<option value="' + val.POSITION_ID + '">' + val.EMPLOYEE_ID + " - " + val.POSITION_ID + '</option>';
                }
            });
            $("#next_pos_id_update").append(text);
            $('#next_pos_id_update').select2({
                dropdownParent: $('#modal_update')
            });
        }
    });
}

function updateAccountDL() {
    debugger
    if ($("#approval_from_update").val() == "" || $("#approval_from_update").val() == null) {
        Swal.fire(
            'Warning',
            'Pastikan Approval From terisi!',
            'warning'
        );
        return;
    } else if ($("#approval_to_update").val() == "" || $("#approval_to_update").val() == null) {
        Swal.fire(
            'Warning',
            'Pastikan Approval To terisi!',
            'warning'
        );
        return;
    }

    let obj = new Object
    obj.ID = $('#txt_id').val();
    obj.APPROVAL_ACTION = $('#approval_action_update').val();
    obj.APPROVAL_ORDER = $('#approval_order_update').val();
    obj.APPROVAL_FROM = $('#approval_from_update').val();
    obj.APPROVAL_TO = $('#approval_to_update').val();
    obj.CURR_POSITION_ID = $('#curr_pos_id_update').val();
    obj.NEXT_POSITION_ID = $('#next_pos_id_update').val();
    obj.APPROVAL_STATUS = $('#approval_status_update').val();
    obj.CURRENT_STATUS = $('#current_status_update').val();

    $.ajax({
        url: $("#web_link").val() + "/api/Setting/Update_MappingApproval", //URI
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