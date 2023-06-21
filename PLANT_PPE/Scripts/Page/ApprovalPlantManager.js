Codebase.helpersOnLoad(['cb-table-tools-checkable', 'cb-table-tools-sections']);
var table = $("#tbl_ppe").DataTable({
    ajax: {
        //url: $("#web_link").val() + "/api/PPE/Get_ListApprovalPM_PPE",
        url: $("#web_link").val() + "/api/PPE/Get_ListApprovalPM_PPE/" + $("#hd_PositionID").val(),
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
                /*return meta.row + meta.settings._iDisplayStart + 1;*/
                //return '<input type="checkbox">';
                //return '<input type="checkbox" class="row-checkbox">';
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
    $('.row-checkbox:checked').each(function () {
        //selectedRows.push($(this).data('id'));
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
    $('.row-checkbox:checked').each(function () {
        debugger
        let equipNo = $(this).closest('tr').find('td:eq(3)').text();
        let ppe = {
            PPE_NO: $(this).data('id'),
            UPDATED_BY: $("#hd_nrp").val(),
            REMARKS: $("#txt_remark").val(),
            EQUIP_NO: equipNo,
            POSISI_PPE: postStatus === "REJECT" ? "Plant Manager" : "Plant Dept. Head",
            // kolom lain jika diperlukan
            STATUS: postStatus,
            APPROVAL_ORDER: postStatus === "REJECT" ? 3 : 3,
        };
        dataPPE.push(ppe);
    });

    $.ajax({
        url: $("#web_link").val() + "/api/Approval/Approve_PPE",
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
                        window.location.href = "/Approval/PlantManager";
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