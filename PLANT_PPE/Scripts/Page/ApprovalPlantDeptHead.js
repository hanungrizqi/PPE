Codebase.helpersOnLoad(['cb-table-tools-checkable', 'cb-table-tools-sections']);

var table = $("#tbl_ppe").DataTable({
    ajax: {
        url: $("#web_link").val() + "/api/PPE/Get_ListApprovalPDH_PPE?posid=" + $("#hd_PositionID").val(),
        //url: $("#web_link").val() + "/api/PPE/Get_ListApprovalPDH_PPE",
        dataSrc: "Data",
    },
    
    "columnDefs": [
        { "className": "dt-center", "targets": '_all' },
        { "className": "dt-nowrap", "targets": '_all' }
    ],
    scrollX: true,
    columns: [
        //{
        //    "data": null,
        //    render: function (data, type, row, meta) {
        //        return '<input type="checkbox" class="row-checkbox" data-id="' + row.PPE_NO + '">';
        //    }
        //},
        {
            "data": null,
            render: function (data, type, row, meta) {
                return meta.row + meta.settings._iDisplayStart + 1;
            }
        },
        { data: 'PPE_NO' },
        //{ data: 'EGI' },
        //{ data: 'EQUIP_NO' },
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
        //    data: 'UPLOAD_FORM_CAAB',
        //    render: function (data, type, row) {
        //        return '<td><input class="form-control form-control-sm input-file" type="file" accept=".pdf" multiple></td>';
        //    }
        //},
        //{
        //    data: 'ID',
        //    targets: 'no-sort', orderable: false,
        //    render: function (data, type, row) {
        //        action = `<div class="btn-group">`
        //        action += `<a href="/Approval/DetailPPE?idppe=${data}" class="btn btn-sm btn-info">Detail</a>`
        //        return action;
        //    }
        //},
        {
            data: 'PPE_NO',
            targets: 'no-sort', orderable: false,
            render: function (data, type, row) {
                action = `<div class="btn-group">`
                action += `<a href="/Approval/Detail_DeptHead?ppe=${data}" class="btn btn-sm btn-info">Detail</a>`
                return action;
            }
        }
    ],
    initComplete: function () {
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
});

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
    let attachmentFiles = [];
    $('.row-checkbox:checked').each(function () {
        let equipNo = $(this).closest('tr').find('td:eq(3)').text();
        selectedRows.push(equipNo);

        let files = $(this).closest('tr').find('td:eq(8)')[0].childNodes[0].files;
        for (let i = 0; i < files.length; i++) {
            attachmentFiles.push(files[i]);
        }
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
    let isAnyFileMissing = false;
    $('.row-checkbox:checked').each(function () {
        debugger
        let equipNo = $(this).closest('tr').find('td:eq(3)').text();
        let attachmentFile = $(this).closest('tr').find('td:eq(8)')[0].childNodes[0].files[0];
        console.log(attachmentFile);
        if (attachmentFile === undefined) {
            isAnyFileMissing = true;
            return false; 
        }
        let ppe = {
            PPE_NO: $(this).data('id'),
            UPDATED_BY: $("#hd_nrp").val(),
            REMARKS: $("#txt_remark").val(),
            EQUIP_NO: equipNo,
            POSISI_PPE: postStatus === "REJECT" ? "Plant Dept. Head" : "Project Manager Pengirim",
            STATUS: postStatus,
            APPROVAL_ORDER: postStatus === "REJECT" ? 4 : 4,
            URL_FORM_PM_PENGIRIM: "http://10.14.101.181/ReportServer_RPTPROD?/PPE/Rpt_PPE_PMPengirim&PPE_NO=" + $(this).data('id'),
        };
        dataPPE.push(ppe);
        if (!uniquePPE_NO.has(ppe.PPE_NO)) {
            debugger
            uniquePPE_NO.add(ppe.PPE_NO);
        }
    });

    if (isAnyFileMissing) {
        Swal.fire({
            title: 'Warning',
            text: "Please upload file for all selected rows.",
            icon: 'warning',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'OK',
            allowOutsideClick: false,
            allowEscapeKey: false
        });
        return;
    }
    
    $.ajax({
        url: $("#web_link").val() + "/api/Approval/Approve_PPE_DeptHead",
        data: JSON.stringify(dataPPE),
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {
            $("#overlay").show();
        },
        success: function (data) {
            if (data.Remarks == true) {
                debugger
                submitCAAB();
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

function submitCAAB() {
    debugger
    let selectedEquipNos = [];
    let selectedPpeNos = [];
    let attachmentFiles = [];
    $('.row-checkbox:checked').each(function () {
        let equipNo = $(this).closest('tr').find('td:eq(3)').text();
        let nomPPE = $(this).closest('tr').find('td:eq(1)').text();
        selectedEquipNos.push(equipNo);
        selectedPpeNos.push(nomPPE);
        let files = $(this).closest('tr').find('td:eq(8)')[0].childNodes[0].files;
        for (let i = 0; i < files.length; i++) {
            attachmentFiles.push(files[i]);
        }
    });

    console.log(selectedEquipNos);
    console.log(selectedPpeNos);
    console.log(attachmentFiles);

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
        };
        dataPPE.push(ppe);
        if (!uniquePPE_NO.has(ppe.PPE_NO)) {
            debugger
            uniquePPE_NO.add(ppe.PPE_NO);
        }
    });
    console.log(uniquePPE_NO);
    console.log(dataPPE);

    debugger
    let nomorPPE = selectedPpeNos;
    let attachmentFile = attachmentFiles;
    let nomorEQP = selectedEquipNos;

    let formData = new FormData();
    for (let i = 0; i < nomorPPE.length; i++) {
        formData.append('nomorPPE[]', nomorPPE[i]);
    }
    for (let i = 0; i < nomorEQP.length; i++) {
        formData.append('nomorEQP[]', nomorEQP[i]);
    }
    for (let i = 0; i < attachmentFile.length; i++) {
        formData.append('attachmentFiles', attachmentFile[i]);
    }

    console.log(nomorPPE);
    console.log(nomorEQP);
    console.log(attachmentFile);

    $.ajax({
        url: $("#web_link").val() + "/api/Approval/Upload_CAAB", //URI
        data: formData,
        type: "POST",
        contentType: false,
        processData: false,
        success: function (data) {
            if (data.Remarks == true) {
                sendMailPM_Pengirim(Array.from(uniquePPE_NO));
            } else if (data.Remarks == false) {
                Swal.fire({
                    title: 'Warning',
                    text: "File already exist.",
                    icon: 'warning',
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK',
                    allowOutsideClick: false,
                    allowEscapeKey: false
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
    })
}

function sendMailPM_Pengirim(uniquePPE_NO) {
    debugger
    var encodedPPENo = encodeURIComponent(uniquePPE_NO.join(','));
    debugger
    $.ajax({
        url: $("#web_link").val() + "/api/PPE/Sendmail_PM_Pengirim?ppe=" + encodedPPENo,
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
                        window.location.href = "/Approval/PlantDeptHead";
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
            POSISI_PPE: postStatus === "REJECT" ? "Plant Dept. Head" : "Project Manager Pengirim",
            STATUS: postStatus,
            APPROVAL_ORDER: postStatus === "REJECT" ? 4 : 4,
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
                debugger
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
                        window.location.href = "/Approval/PlantDeptHead";
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