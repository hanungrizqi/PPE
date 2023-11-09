$("document").ready(function () {
    getDistrict()
    getLoc()
})

var table = $("#tbl_dstrct").DataTable({
    ajax: {
        url: $("#web_link").val() + "/api/Setting/Get_District_Location",
        dataSrc: "Data",
    },
    "columnDefs": [
        { "className": "dt-center", "targets": [0, 1, 2] }
    ],
    scrollX: true,
    columns: [
        { data: 'TABLE_DESC' },
        { data: 'TABLE_CODE' },
        {
            data: 'ID',
            targets: 'no-sort', orderable: false,
            render: function (data, type, row) {
                action = `<div class="btn-group">`
                action += `<button type="button" value="${row.TABLE_DESC}" onclick="deleteDist(${row.ID}, this.value)" class="btn btn-sm btn-danger" title="Delete">Delete
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
                    .appendTo($("#tbl_dstrct_filter.dataTables_filter"))
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

function getDistrict() {
    debugger
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
        }
    });
}

function getLoc() {
    debugger
    $.ajax({
        url: $("#web_link").val() + "/api/Master/getAllLocation", //URI,
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
        }
    });
}

function insertDist() {
    let obj = new Object();
    obj.TABLE_CODE = $('#txt_loc').val();
    obj.TABLE_DESC = $('#txt_district').val();

    $.ajax({
        url: $("#web_link").val() + "/api/Setting/Create_Dist", //URI
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

function deleteDist(id, dist) {
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
                url: $("#web_link").val() + "/api/Setting/Delete_Dist?id=" + id, //URI
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
