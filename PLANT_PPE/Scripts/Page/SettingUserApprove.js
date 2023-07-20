Codebase.helpersOnLoad(['jq-select2']);

$("document").ready(function () {
    $('.select2-modal').select2({
        dropdownParent: $('.modal')
    });

    showTable();
})

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

//function showTable() {
    var table = $("#tbl_userApprove").DataTable({
        ajax: {
            url: $("#web_link").val() + "/api/Setting/Get_UserApproveSetting",
            dataSrc: "Data",
        },
        "columnDefs": [
            { "className": "dt-center", "targets": [0, 1, 2, 3, 4] }
        ],
        scrollX: true,
        columns: [
            { data: 'Employee_id' },
            { data: 'Name' },
            { data: 'Position_id' },
            { data: 'dstrct_code' },
            { data: 'sub_menu' },
            {
                data: 'id',
                targets: 'no-sort', orderable: false,
                render: function (data, type, row) {
                    action = `<div class="btn-group">`
                    action += `<button type="button" value="${row.id}" onclick="deleteUser(${row.id}, this.value)" class="btn btn-sm btn-danger" title="Delete">Delete
                                </button>`
                    action += `</div>`
                    return action;
                }
            }
        ],
        initComplete: function () {
            var api = this.api();

            // For each column
            api
                .columns([0, 1, 2, 3, 4])
                .eq(0)
                .each(function (colIdx) {
                    // Set the header cell to contain the input element
                    var cell = $('.filters').eq(
                        $(api.column(colIdx).header()).index()
                    );
                    var title = $(cell).text();
                    $(cell).html('<input type="text" placeholder="' + title + '" />');

                    // On every keypress in this input
                    $(
                        'input',
                        $('.filters').eq($(api.column(colIdx).header()).index())
                    )
                        .off('keyup change')
                        .on('change', function (e) {
                            // Get the search value
                            $(this).attr('title', $(this).val());
                            var regexr = '({search})'; //$(this).parents('th').find('select').val();

                            var cursorPosition = this.selectionStart;
                            // Search the column for that value
                            api
                                .column(colIdx)
                                .search(
                                    this.value != ''
                                        ? regexr.replace('{search}', '(((' + this.value + ')))')
                                        : '',
                                    this.value != '',
                                    this.value == ''
                                )
                                .draw();
                        })
                        .on('keyup', function (e) {
                            e.stopPropagation();

                            $(this).trigger('change');
                            $(this)
                                .focus()[0]
                                .setSelectionRange(cursorPosition, cursorPosition);
                        });
                });
        }
    });
//}

function insertUserApprove() {
    let obj = new Object();
    obj.Employee_id = $('#txt_nrp').val();
    obj.Position_id = $('#txt_positionID').val();
    obj.Name = $('#txt_name').val();
    obj.sub_menu = $('#txt_menu').val();
    obt.dstrct_code = $('#txt_distrik').val();

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
