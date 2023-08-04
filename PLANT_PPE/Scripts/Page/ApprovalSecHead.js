Codebase.helpersOnLoad(['cb-table-tools-checkable', 'cb-table-tools-sections']);
var table = $("#tbl_ppe").DataTable({
    ajax: {
        //url: $("#web_link").val() + "/api/PPE/Get_ListApprovalPPE_SECHEAD/" + $("#hd_PositionID").val(),
        url: $("#web_link").val() + "/api/PPE/Get_ListApprovalPPE_SECHEAD",
        dataSrc: "Data",
    },

    "columnDefs": [
        { "className": "dt-center", "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8] },
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
                text = `<span class="badge bg-info">${data}</span>`;
                return text;
            }
        },
        {
            data: 'ID',
            targets: 'no-sort', orderable: false,
            render: function (data, type, row) {
                action = `<div class="btn-group">`
                action += `<a href="/Approval/DetailPPE?idppe=${data}" class="btn btn-sm btn-info">Detail</a>`
                //action += `<button onClick="printReport('${row.PPE_NO}')" type="button" class="btn btn-primary btn-sm">Print</button>`
                return action;
            }
        }
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
        this.api()
            .columns(1)
            .every(function () {
                var column = this;
                var select = $('<select class="form-control form-control-sm" style="width:200px; display:inline-block; margin-left: 10px;"><option value="">-- PPE NUMBER --</option></select>')
                    .appendTo($("#tbl_ppe_filter.dataTables_filter"))
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
    //initComplete: function () {
    //    var headerCheckbox = document.getElementById('checkAll');
    //    var rowCheckboxes = document.getElementsByClassName('row-checkbox');
    //    headerCheckbox.addEventListener('change', function () {
    //        var isChecked = headerCheckbox.checked;
    //        for (var i = 0; i < rowCheckboxes.length; i++) {
    //            rowCheckboxes[i].checked = isChecked;
    //        }
    //    });

    //    var firstPPE = this.api().column(1).data()[0];
    //    debugger
    //    this.api().column(1).order('asc').draw();

    //    this.api().columns(1).every(function () {
    //        var column = this;
    //        var select = $('<select class="form-control form-control-sm" style="width:200px; display:inline-block; margin-left: 10px;"></select>')
    //            .appendTo($("#tbl_ppe_filter.dataTables_filter"))
    //            .on('change', function () {
    //                var val = $.fn.dataTable.util.escapeRegex($(this).val());
    //                column.search(val ? '^' + val + '$' : '', true, false).draw();
    //            });

    //        if (firstPPE) {
    //            select.append('<option value="' + firstPPE + '">' + firstPPE + '</option>');
    //        } else {
    //            select.append('<option value="-- PPE NUMBER --">-- PPE NUMBER --</option>');
    //        }
    //        column
    //            .data()
    //            .unique()
    //            .sort()
    //            .each(function (d, j) {
    //                if (d !== firstPPE) {
    //                    select.append('<option value="' + d + '">' + d + '</option>');
    //                }
    //            });
    //        if (firstPPE) {
    //            column.search('^' + firstPPE + '$', true, false).draw();
    //        } else {
    //            column.search('^-- PPE NUMBER --$', true, false).draw();
    //        }
    //    });
    //},
});

$('#tbl_ppe').on('click', '.print-link', function () {
    var ppeNo = $(this).closest('tr').find('td:nth-child(2)').text();
    var printUrl = "/Reports/ReportSH.aspx?PPE_NO=" + ppeNo;
    window.location.href = printUrl;
});

function printReport(ppeno) {
    debugger
    var printUrl = "http://10.14.101.181/ReportServer_RPTPROD?/PPE/Rpt_PPE_SecHead&PPE_NO=" + ppeno;
    window.open(printUrl, '_blank');
}

table.on('draw', function () {
    var visibleCheckboxes = document.querySelectorAll('#tbl_ppe tbody .row-checkbox:checked');

    visibleCheckboxes.forEach(function (checkbox) {
        checkbox.checked = false;
    });
});

function submitApproval(postStatus) {
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
            POSISI_PPE: postStatus === "REJECT" ? "Sect. Head" : "Plant Manager",
            STATUS: postStatus,
            URL_FORM_PLNTMNGR: "http://10.14.101.181/ReportServer_RPTPROD?/PPE/Rpt_PPE_PlantManager&PPE_NO=" + $(this).data('id'),
        };
        dataPPE.push(ppe);
        debugger
        if (!uniquePPE_NO.has(ppe.PPE_NO)) {
            debugger
            uniquePPE_NO.add(ppe.PPE_NO);
        }
    });
    console.log(uniquePPE_NO);
    console.log(dataPPE);

    $.ajax({
        url: $("#web_link").val() + "/api/Approval/Approve_Section_Head",
        data: JSON.stringify(dataPPE),
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {
            $("#overlay").show();
        },
        success: function (data) {
            sendMailPlant_Manager(Array.from(uniquePPE_NO));
        },
        error: function (xhr) {
            alert(xhr.responseText);
            $("#overlay").hide();
        }
    });
}

function sendMailPlant_Manager(uniquePPE_NO) {
    debugger
    var encodedPPENo = encodeURIComponent(uniquePPE_NO.join(','));
    debugger
    $.ajax({
        url: $("#web_link").val() + "/api/PPE/Sendmail_Plant_Manager?ppe=" + encodedPPENo,
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.Remarks) {
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
                        window.location.href = "/Approval/SectionHead";
                    }
                });
            } else {
                Swal.fire(
                    'Error!',
                    'Message: ' + data.Message,
                    'error'
                );
            }
        },
        error: function (xhr) {
            alert(xhr.responseText);
        }
    });
}

function rejectApproval(postStatus) {
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
            POSISI_PPE: postStatus === "REJECT" ? "Sect. Head" : "Plant Manager",
            STATUS: postStatus,
        };
        dataPPE.push(ppe);
        debugger
        if (!uniquePPE_NO.has(ppe.PPE_NO)) {
            debugger
            uniquePPE_NO.add(ppe.PPE_NO);
        }
    });
    console.log(uniquePPE_NO);
    console.log(dataPPE);

    $.ajax({
        url: $("#web_link").val() + "/api/Approval/Reject_Section_Head",
        data: JSON.stringify(dataPPE),
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {
            $("#overlay").show();
        },
        success: function (data) {
            if (data.Remarks) {
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
                        window.location.href = "/Approval/SectionHead";
                    }
                });
            } else {
                Swal.fire(
                    'Error!',
                    'Message: ' + data.Message,
                    'error'
                );
            }
        },
        error: function (xhr) {
            alert(xhr.responseText);
            $("#overlay").hide();
        }
    });
}