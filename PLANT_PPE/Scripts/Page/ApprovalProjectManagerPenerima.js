Codebase.helpersOnLoad(['cb-table-tools-checkable', 'cb-table-tools-sections']);

var table = $("#tbl_ppe_penerima").DataTable({
    ajax: {
        url: $("#web_link").val() + "/api/PPE/Get_ListApprovalPM_Penerima/" + $("#hd_PositionID").val(),
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
                text = `<span class="badge bg-success">${data}</span>`;
                return text;
            }
        },
        {
            data: 'ID',
            targets: 'no-sort', orderable: false,
            render: function (data, type, row) {
                action = `<div class="btn-group">`
                action += `<a href="/Approval/DetailPPE?idppe=${data}" class="btn btn-sm btn-info">Detail</a>`
                return action;
            }
        }
    ],
    initComplete: function () {
        var headerCheckbox = document.getElementById('checkAll2');
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
                    .appendTo($("#tbl_ppe_penerima_filter.dataTables_filter"))
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
    $('#modal-terms').on('show.bs.modal', function () {
        getContent();
    });
    $('#modal-terms').on('hidden.bs.modal', function () {
        var agreeCheckbox = document.getElementById('val-terms');
        agreeCheckbox.checked = true; 
    });

    $('#modal-terms').on('click', '.btn.btn-alt-primary', function () {
        var agreeCheckbox = document.getElementById('val-terms');
        agreeCheckbox.disabled = false; 
    });
})

table.on('draw', function () {
    var visibleCheckboxes = document.querySelectorAll('#tbl_ppe_pengirim tbody .row-checkbox:checked');

    visibleCheckboxes.forEach(function (checkbox) {
        checkbox.checked = false;
    });
});

function getContent() {
    $.ajax({
        url: $("#web_link").val() + "/api/Setting/Get_Agreement", //URI
        dataType: "json",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var content = data.Data;
            $("#agreementss").html(content);

        },
        error: function (xhr) {
            alert(xhr.responseText);
        }
    });
}

function submitApprovalPenerima(postStatus) {
    debugger
    var agreeCheckbox = document.getElementById('val-terms');
    if ($("#txt_remark").val() == "" || $("#txt_remark").val() == null) {
        Swal.fire(
            'Warning',
            'Mohon sertakan Remarks Approval!',
            'warning'
        );
        return;
    } else if (!agreeCheckbox.checked) {
        Swal.fire(
            'Warning!',
            'Anda harus menyetujui Syarat & Ketentuan sebelum melanjutkan.',
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
            POSISI_PPE: postStatus === "REJECT" ? "Project Manager Penerima" : "Division Head ENG",
            STATUS: postStatus,
            APPROVAL_ORDER: 6,
            URL_FORM_DIVHEAD_ENG: "http://10.14.101.181/ReportServer_RPTPROD?/PPE/Rpt_PPE_DivHead_Eng&PPE_NO=" + $(this).data('id'),
        };
        dataPPE.push(ppe);
        if (!uniquePPE_NO.has(ppe.PPE_NO)) {
            debugger
            uniquePPE_NO.add(ppe.PPE_NO);
        }
    });

    $.ajax({
        url: $("#web_link").val() + "/api/Approval/Approve_PPE_PMPenerima",
        data: JSON.stringify(dataPPE),
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {
            $("#overlay").show();
        },
        success: function (data) {
            if (data.Remarks == true) {
                sendMailDivhead_Eng(Array.from(uniquePPE_NO));
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

function sendMailDivhead_Eng(uniquePPE_NO) {
    debugger
    var encodedPPENo = encodeURIComponent(uniquePPE_NO.join(','));
    debugger
    $.ajax({
        url: $("#web_link").val() + "/api/PPE/Sendmail_Divhead_Eng?ppe=" + encodedPPENo,
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
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
                        window.location.href = "/Approval/ProjectManagerPenerima";
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
        }
    });
}

function rejectApproval(postStatus) {
    debugger
    var agreeCheckbox = document.getElementById('val-terms');
    if ($("#txt_remark").val() == "" || $("#txt_remark").val() == null) {
        Swal.fire(
            'Warning',
            'Mohon sertakan Remarks Approval!',
            'warning'
        );
        return;
    } else if (!agreeCheckbox.checked) {
        Swal.fire(
            'Warning!',
            'Anda harus menyetujui Syarat & Ketentuan sebelum melanjutkan.',
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
            POSISI_PPE: postStatus === "REJECT" ? "Project Manager Penerima" : "Division Head ENG",
            STATUS: postStatus,
            APPROVAL_ORDER: 6,
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
                        window.location.href = "/Approval/ProjectManagerPenerima";
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