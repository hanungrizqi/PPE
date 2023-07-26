/*Codebase.helpersOnLoad(['jq-select2']);*/

$("document").ready(function () {
    //$('.select2-modal').select2({
    //    dropdownParent: $('.modal')
    //});
    getLokasi();
    getDistrik();
})

//function getLokasi() {
//    $.ajax({
//        url: $("#web_link").val() + "/api/Master/getLocation", //URI,
//        type: "GET",
//        cache: false,
//        success: function (result) {
//            $('#txt_location').empty();
//            text = '<option></option>';
//            $.each(result.Data, function (key, val) {
//                text += '<option value="' + val.EQUIP_LOCATION + '">' + val.EQUIP_LOCATION + '</option>';
//            });
//            $("#txt_location").append(text);
//        }
//    });
//}

function getLokasi() {
    $.ajax({
        url: $("#web_link").val() + "/api/Master/getLocation", //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            $('#txt_location').empty();
            var uniqueLocations = {}; // Objek untuk melacak nilai EQUIP_LOCATION yang unik
            text = '<option></option>';
            $.each(result.Data, function (key, val) {
                if (!uniqueLocations[val.EQUIP_LOCATION]) {
                    text += '<option value="' + val.EQUIP_LOCATION + '">' + val.EQUIP_LOCATION + '</option>';
                    uniqueLocations[val.EQUIP_LOCATION] = true; // Tandai nilai EQUIP_LOCATION sebagai unik
                }
            });
            $("#txt_location").append(text);
        }
    });
}


function getDistrik() {
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
        }
    });
}

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
                action += `<button type="button" value="${row.ACCT_PROFILE}" onclick="setSource(${row.ID}, this.value, '${row.DSTRCT_CODE}', '${row.DSTRCT_LOC}')" data-bs-toggle="modal" data-bs-target="#modal_update" class="btn btn-sm btn-info" title="Edit">Edit
                                </button>`
                action += `<button type="button" value="${row.ACCT_PROFILE}" onclick="deleteAcct(${row.ID}, this.value, '${row.DSTRCT_CODE}', '${row.DSTRCT_LOC}')" class="btn btn-sm btn-danger" title="Delete">Delete
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


function insertAcct() {
    let obj = new Object();
    obj.ACCT_PROFILE = $('#txt_accprofile').val();
    obj.DSTRCT_CODE = $('#txt_district').val();
    obj.DSTRCT_LOC = $('#txt_location').val();

    $.ajax({
        url: $("#web_link").val() + "/api/Setting/Create_Accprofile", //URI
        data: JSON.stringify(obj),
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
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
                        window.location.href = "/Setting/AccountProfile";
                    }
                });
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

function deleteAcct(id, accprofile, distrik, lokasi) {
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
                url: $("#web_link").val() + "/api/Setting/Delete_Acctprofile?id=" + id, //URI
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

function setSource(id, accprofile, distrik, lokasi) {
    debugger
    $("#txt_id").val(id);
    $("#txt_accprof_update").val(accprofile);
    $("#txt_distrik_update").val(distrik);
    $("#txt_lokasi_update").val(lokasi);
}

function updateSurce() {
    let obj = new Object
    obj.ACCT_PROFILE = $('#txt_accprof_update').val();
    obj.DSTRCT_CODE = $('#txt_distrik_update').val();
    obj.DSTRCT_LOC = $('#txt_lokasi_update').val();
    obj.ID = $('#txt_id').val();
    debugger
    $.ajax({
        url: $("#web_link").val() + "/api/Setting/Update_Accountprofile", //URI
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