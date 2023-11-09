var table = $("#tbl_accp").DataTable({
    ajax: {
        url: $("#web_link").val() + "/api/Setting/Acc_profile",
        dataSrc: "Data",
    },
    "columnDefs": [
        { "className": "dt-center", "targets": [0, 1, 2, 3, 4, 5, 6, 7] },
        { "className": "dt-nowrap", "targets": [1, 2] }
    ],
    scrollX: true,
    columns: [
        { data: 'DSTRCT_CODE' },
        { data: 'EQUIPMENT_LOCATION' },
        { data: 'EQUIPMENT_DESC' },
        { data: 'ACTIVE_FLAG' },
        { data: 'PRODUCTION_EQUIPMENT' },
        { data: 'SUPPORT_EQUIPMENT' },
        { data: 'WORKSHOP_EQUIPMENT' },
        {
            data: 'ID',
            targets: 'no-sort', orderable: false,
            render: function (data, type, row) {
                action = `<div class="btn-group">`
                action += `<button type="button" value="${row.DSTRCT_CODE}" onclick="setAccountprofile(${row.ID}, this.value, '${row.EQUIPMENT_LOCATION}', '${row.EQUIPMENT_DESC}', '${row.ACTIVE_FLAG}', '${row.PRODUCTION_EQUIPMENT}', '${row.SUPPORT_EQUIPMENT}', '${row.WORKSHOP_EQUIPMENT}')" data-bs-toggle="modal" data-bs-target="#modal_update" class="btn btn-sm btn-info" title="Edit">Edit
                                </button>`
                action += `<button type="button" onclick="deleteDist(${row.ID})" class="btn btn-sm btn-danger" title="Delete">Delete
                                </button>`
                action += `</div>`
                return action;
            }
        }
    ],
    initComplete: function () {
        this.api()
            .columns(0)
            .every(function () {
                var column = this;
                var select = $('<select class="form-control form-control-sm" style="width:200px; display:inline-block; margin-left: 10px;"><option value="">-- DISTRICT --</option></select>')
                    .appendTo($("#tbl_accp_filter.dataTables_filter"))
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

$("document").ready(function () {
    getDistrict(function () {
        getLoc();
    });
})

var selectedDistrict;
function getDistrict(callback) {
    $.ajax({
        url: $("#web_link").val() + "/api/Master/getDistrict", //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            $('#txt_district').empty();
            text = '<option></option>';
            $.each(result.Data, function (key, val) {
                text += '<option value="' + val.DSTRCT_CODE + '">' + val.DSTRCT_CODE + '</option>';
            });
            $("#txt_district").append(text);
            $('#txt_district').select2({
                dropdownParent: $('#modal-insert')
            });
            $("#txt_district").change(function () {
                selectedDistrict = $(this).val();
                if (typeof callback === "function") {
                    callback();
                }
            });

        }
    });
}


var selectedLoc;
function getLoc() {
    $.ajax({
        url: $("#web_link").val() + "/api/Master/getLoc/" + selectedDistrict, //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            $('#txt_loc').empty();
            text = '<option></option>';
            $.each(result.Data, function (key, val) {
                text += '<option value="' + val.TABLE_CODE + '">' + val.TABLE_CODE + '</option>';
            });
            $("#txt_loc").append(text);
            $('#txt_loc').select2({
                dropdownParent: $('#modal-insert')
            });
            $("#txt_loc").change(function () {
                selectedLoc = $(this).val();
                equipmentDesc();
            });
        }
    });
}

function equipmentDesc() {
    $.ajax({
        url: $("#web_link").val() + "/api/Setting/equipmentDesc?loc=" + selectedLoc, //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            var eq = result.Data;
            $("#txt_desc").val(eq.TABLE_DESC);
        }
    });
}

function insertDist() {
    let obj = new Object();
    obj.DSTRCT_CODE = $('#txt_district').val();
    obj.EQUIPMENT_LOCATION = $('#txt_loc').val();
    obj.EQUIPMENT_DESC = $('#txt_desc').val();
    obj.ACTIVE_FLAG = $('#flag').val();
    obj.PRODUCTION_EQUIPMENT = $('#prodeqp').val();
    obj.SUPPORT_EQUIPMENT = $('#suppeqp').val();
    obj.WORKSHOP_EQUIPMENT = $('#wseqp').val();

    $.ajax({
        url: $("#web_link").val() + "/api/Setting/Create_Equiplocation", //URI
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
                url: $("#web_link").val() + "/api/Setting/Delete_Equiplocation?id=" + id, //URI
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

function setAccountprofile(id, distrik, eqploc, eqpdesc, flagg, prod_eqp, supp_eqp, ws_eqp) {
    debugger
    districtUpdate(distrik, eqploc);

    $("#txt_id").val(id);
    $("#flag_update").val(flagg);
    $("#prodeqp_update").val(prod_eqp);
    $("#suppeqp_update").val(supp_eqp);
    $("#wseqp_update").val(ws_eqp);
}

function districtUpdate(distrik, eqploc) {
    debugger
    $.ajax({
        url: $("#web_link").val() + "/api/Master/getDistrict", //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            debugger
            $('#txt_district_update').empty();
            text = '<option></option>';
            var dataStatus = distrik;
            $.each(result.Data, function (key, val) {
                if (val.DSTRCT_CODE == dataStatus) {
                    text += '<option selected value="' + val.DSTRCT_CODE + '">' + val.DSTRCT_CODE + '</option>';
                } else {
                    text += '<option value="' + val.DSTRCT_CODE + '">' + val.DSTRCT_CODE + '</option>';
                }
            });
            $("#txt_district_update").append(text);
            $('#txt_district_update').select2({
                dropdownParent: $('#modal_update')
            });
            locUpdate($("#txt_district_update").val(), eqploc)
        }
    });
}

$('#txt_district_update').on('change', function () {
    var selectedValue = $(this).val();
    var eqploc = '';
    locUpdate2(selectedValue, eqploc);
});

$('#txt_loc_update').on('change', function () {
    var selectedValue = $(this).val();
    descUpdate(selectedValue);
});

function locUpdate(dist, eqploc) {
    debugger
    $.ajax({
        url: $("#web_link").val() + "/api/Master/getLoc/" + dist, //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            debugger
            $('#txt_loc_update').empty();
            text = '<option></option>';
            $.each(result.Data, function (key, val) {
                if (val.TABLE_CODE.trim() == eqploc) {
                    text += '<option selected value="' + val.TABLE_CODE + '">' + val.TABLE_CODE + '</option>';
                } else {
                    text += '<option value="' + val.TABLE_CODE + '">' + val.TABLE_CODE + '</option>';
                }
            });
            $("#txt_loc_update").append(text);
            $('#txt_loc_update').select2({
                dropdownParent: $('#modal_update')
            });
            descUpdate($("#txt_loc_update").val())
        }
    });
}

function locUpdate2(dist, eqploc) {
    debugger
    $.ajax({
        url: $("#web_link").val() + "/api/Master/getLoc/" + dist, //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            debugger
            $('#txt_loc_update').empty();
            text = '<option></option>';
            $.each(result.Data, function (key, val) {
                if (val.TABLE_CODE.trim() == eqploc) {
                    text += '<option selected value="' + val.TABLE_CODE + '">' + val.TABLE_CODE + '</option>';
                } else {
                    text += '<option value="' + val.TABLE_CODE + '">' + val.TABLE_CODE + '</option>';
                }
            });
            $("#txt_loc_update").append(text);
            $('#txt_loc_update').select2({
                dropdownParent: $('#modal_update')
            });
        }
    });
}

function descUpdate(param) {
    debugger
    $.ajax({
        url: $("#web_link").val() + "/api/Setting/equipmentDesc?loc=" + param, //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            debugger
            var eq = result.Data;
            $("#txt_desc_update").val(eq.TABLE_DESC);
        }
    });
}

function updateAccountDL() {
    debugger
    let obj = new Object
    obj.ID = $('#txt_id').val();
    obj.DSTRCT_CODE = $('#txt_district_update').val();
    obj.EQUIPMENT_LOCATION = $('#txt_loc_update').val();
    obj.EQUIPMENT_DESC = $('#txt_desc_update').val();
    obj.ACTIVE_FLAG = $('#flag_update').val();
    obj.PRODUCTION_EQUIPMENT = $('#prodeqp_update').val();
    obj.SUPPORT_EQUIPMENT = $('#suppeqp_update').val();
    obj.WORKSHOP_EQUIPMENT = $('#wseqp_update').val();

    $.ajax({
        url: $("#web_link").val() + "/api/Setting/Update_Equiplocation", //URI
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