Codebase.helpersOnLoad(['cb-table-tools-checkable', 'cb-table-tools-sections']);
var table = $("#tbl_ppe_sm").DataTable({
    ajax: {
        url: $("#web_link").val() + "/api/PPE/List_ApprovalSM/" + $("#hd_PositionID").val(),
        dataSrc: "Data",
    },

    "columnDefs": [
        { "className": "dt-center", "targets": [0, 1, 2, 3, 4, 5, 6, 7] },
        { "className": "dt-nowrap", "targets": '_all' }
    ],
    scrollX: true,
    columns: [
        {
            "data": null,
            render: function (data, type, row, meta) {
                return '<input type="checkbox" class="row-checkbox" data-id="' + row.PPE_NO + '">';
            }
        },
        { data: 'PPE_NO' },
        { data: 'EGI' },
        { data: 'EQUIP_NO' },
        { data: 'DISTRICT_FROM' },
        { data: 'DISTRICT_TO' },
        {
            data: 'CREATED_DATE',
            render: function (data, type, row) {
                const tanggal = moment(data).format("YYYY-MM-DD");
                return tanggal;
            }
        },
        {
            data: 'STATUS',
            render: function (data, type, row) {
                text = `<span class="badge bg-success">${data}</span>`;
                return text;
            }
        },
        //{
        //    data: 'ID',
        //    targets: 'no-sort', orderable: false,
        //    render: function (data, type, row) {
        //        action = `<div class="btn-group">`
        //        action += `<a href="/Approval/DetailPPE?idppe=${data}" class="btn btn-sm btn-info">Detail</a>`
        //        return action;
        //    }
        //}
    ],
    initComplete: function () {
        var headerCheckbox = document.getElementById('checkAll');
        var rowCheckboxes = document.getElementsByClassName('row-checkbox');
        headerCheckbox.addEventListener('change', function () {
            var isChecked = headerCheckbox.checked;
            for (var i = 0; i < rowCheckboxes.length; i++) {
                rowCheckboxes[i].checked = isChecked;
            }
        });

        var firstPPE = this.api().column(1).data()[0];
        debugger
        this.api().column(1).order('asc').draw();

        this.api().columns(1).every(function () {
            var column = this;
            var select = $('<select class="form-control form-control-sm" style="width:200px; display:inline-block; margin-left: 10px;"></select>')
                .appendTo($("#tbl_ppe_sm_filter.dataTables_filter"))
                .on('change', function () {
                    var val = $.fn.dataTable.util.escapeRegex($(this).val());
                    column.search(val ? '^' + val + '$' : '', true, false).draw();
                });

            if (firstPPE) {
                select.append('<option value="' + firstPPE + '">' + firstPPE + '</option>');
            } else {
                select.append('<option value="-- PPE NUMBER --">-- PPE NUMBER --</option>');
            }
            column
                .data()
                .unique()
                .sort()
                .each(function (d, j) {
                    if (d !== firstPPE) {
                        select.append('<option value="' + d + '">' + d + '</option>');
                    }
                });
            if (firstPPE) {
                column.search('^' + firstPPE + '$', true, false).draw();
            } else {
                column.search('^-- PPE NUMBER --$', true, false).draw();
            }
        });
    },
});

table.on('draw', function () {
    var visibleCheckboxes = document.querySelectorAll('#tbl_ppe_sm tbody .row-checkbox:checked');

    visibleCheckboxes.forEach(function (checkbox) {
        checkbox.checked = false;
    });
});

function submitapprvl(postStatus) {
    debugger
    if ($("#txt_remark").val() == "" || $("#txt_remark").val() == null) {
        Swal.fire(
            'Warning',
            'Mohon sertakan Remarks Approval!',
            'warning'
        );
        return;
    }
    debugger
    let selectedRows = [];
    $('.row-checkbox:checked').each(function () {
        let equipNo = $(this).closest('tr').find('td:eq(3)').text();
        selectedRows.push(equipNo);
    });
    debugger
    if (selectedRows.length === 0) {
        Swal.fire(
            'Warning',
            'Tidak ada baris yang tercentang!',
            'warning'
        );
        return;
    }

    let dataPPE = [];
    let uniquePPE_NO = new Set();
    $('.row-checkbox:checked').each(function () {
        debugger
        let equipNo = $(this).closest('tr').find('td:eq(3)').text();
        let distrikFrom = $(this).closest('tr').find('td:eq(4)').text();
        let distrikTo = $(this).closest('tr').find('td:eq(5)').text();
        let ppe = {
            PPE_NO: $(this).data('id'),
            UPDATED_BY: $("#hd_nrp").val(),
            REMARKS: $("#txt_remark").val(),
            EQUIP_NO: equipNo,
            DISTRICT_FROM: distrikFrom,
            DISTRICT_TO: distrikTo,
            POSISI_PPE: postStatus === "REJECT" ? "SM" : "DONE",
            STATUS: postStatus,
            APPROVAL_ORDER: 9,
        };
        dataPPE.push(ppe);
        if (!uniquePPE_NO.has(ppe.PPE_NO)) {
            debugger
            uniquePPE_NO.add(ppe.PPE_NO);
        }
    });

    $.ajax({
        url: $("#web_link").val() + "/api/SM/Approve_SM",
        data: JSON.stringify(dataPPE),
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {
            $("#overlay").show();
        },
        success: function (data) {
            if (data.Remarks == true) {
                update_MSE600_SM();
            } if (data.Remarks == false) {
                Swal.fire(
                    'Error!',
                    'Message : ' + data.Message,
                    'error'
                );
                $("#overlay").hide();
            }
        },
        error: function (xhr) {
            alert(xhr.responseText);
            $("#overlay").hide();
        }
    });
}

function update_MSE600_SM() {
    debugger
    if ($("#txt_remark").val() == "" || $("#txt_remark").val() == null) {
        Swal.fire(
            'Warning',
            'Mohon sertakan Remarks Approval!',
            'warning'
        );
        return;
    }
    debugger
    let selectedRows = [];
    $('.row-checkbox:checked').each(function () {
        let equipNo = $(this).closest('tr').find('td:eq(3)').text();
        selectedRows.push(equipNo);
    });
    debugger
    if (selectedRows.length === 0) {
        Swal.fire(
            'Warning',
            'Tidak ada baris yang tercentang!',
            'warning'
        );
        return;
    }

    let dataPPE = [];
    let uniquePPE_NO = new Set();
    $('.row-checkbox:checked').each(function () {
        debugger
        let equipNo = $(this).closest('tr').find('td:eq(3)').text();
        let ppe = {
            PPE_NO: $(this).data('id'),
            UPDATED_BY: $("#hd_nrp").val(),
            REMARKS: $("#txt_remark").val(),
            EQUIP_NO: equipNo,
            APPROVAL_ORDER: 9,
        };
        dataPPE.push(ppe);
        if (!uniquePPE_NO.has(ppe.PPE_NO)) {
            debugger
            uniquePPE_NO.add(ppe.PPE_NO);
        }
    });

    $.ajax({
        url: $("#web_link").val() + "/api/MSE600/SM_Update",
        data: JSON.stringify(dataPPE),
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {
            $("#overlay").show();
        },
        success: function (data) {
            if (data.Remarks == true) {
                Swal.fire({
                    title: 'Saved',
                    text: "Your data has been saved!",
                    icon: 'success',
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK',
                    allowOutsideClick: false,
                    allowEscapeKey: false
                }).then((result) => {
                    if (result.isConfirmed) {
                        window.location.href = "/SM/Approval";
                    }
                })
            } if (data.Remarks == false) {
                Swal.fire(
                    'Error!',
                    'Message : ' + data.Message,
                    'error'
                );
                $("#overlay").hide();
            }

        },
        error: function (xhr) {
            alert(xhr.responseText);
            $("#overlay").hide();
        }
    });
}

function rejectApprvl(postStatus) {
    debugger
    if ($("#txt_remark").val() == "" || $("#txt_remark").val() == null) {
        Swal.fire(
            'Warning',
            'Mohon sertakan Remarks Approval!',
            'warning'
        );
        return;
    }
    debugger
    let selectedRows = [];
    $('.row-checkbox:checked').each(function () {
        let equipNo = $(this).closest('tr').find('td:eq(3)').text();
        selectedRows.push(equipNo);
    });
    debugger
    if (selectedRows.length === 0) {
        Swal.fire(
            'Warning',
            'Tidak ada baris yang tercentang!',
            'warning'
        );
        return;
    }

    let dataPPE = [];
    let uniquePPE_NO = new Set();
    $('.row-checkbox:checked').each(function () {
        debugger
        let equipNo = $(this).closest('tr').find('td:eq(3)').text();
        let ppe = {
            PPE_NO: $(this).data('id'),
            UPDATED_BY: $("#hd_nrp").val(),
            REMARKS: $("#txt_remark").val(),
            EQUIP_NO: equipNo,
            POSISI_PPE: postStatus === "REJECT" ? "SM" : "DONE",
            STATUS: postStatus,
            APPROVAL_ORDER: 9,
        };
        dataPPE.push(ppe);
        if (!uniquePPE_NO.has(ppe.PPE_NO)) {
            debugger
            uniquePPE_NO.add(ppe.PPE_NO);
        }
    });

    $.ajax({
        url: $("#web_link").val() + "/api/Approval/Reject_Approval",
        data: JSON.stringify(dataPPE),
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {
            $("#overlay").show();
        },
        success: function (data) {
            if (data.Remarks == true) {
                Swal.fire({
                    title: 'Saved',
                    text: "Your data has been saved!",
                    icon: 'success',
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK',
                    allowOutsideClick: false,
                    allowEscapeKey: false
                }).then((result) => {
                    if (result.isConfirmed) {
                        window.location.href = "/SM/Approval";
                    }
                })
            } if (data.Remarks == false) {
                Swal.fire(
                    'Error!',
                    'Message : ' + data.Message,
                    'error'
                );
                $("#overlay").hide();
            }

        },
        error: function (xhr) {
            alert(xhr.responseText);
            $("#overlay").hide();
        }
    });
}