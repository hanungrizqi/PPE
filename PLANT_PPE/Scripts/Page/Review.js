Codebase.helpersOnLoad(['jq-select2']);

$("document").ready(function () {
    $('.select2-modal').select2({
        dropdownParent: $('.modal')
    });

   
})

var table2
function getdetail(nomor_equip) {
    console.log(nomor_equip);
    table2 = $("#tbl_detail").DataTable({
        ajax: {
            url: $("#web_link").val() + "/api/PPE/Get_History",
            type: "GET",
            data: {
                Equip_No : nomor_equip
            },
            order: [[3, 'asc']],
            dataSrc: "Data",
        },
        "columnDefs": [
            { "className": "dt-center", "targets": [0, 1, 2, 3] }
        ],
        scrollX: true,
        columns: [

            { data: 'Posisi_Ppe' },
            { data: 'Approved_By' },
            { data: 'Equip_No' },
            {
                data: 'Approved_Date',
                render: function (data, type, row) {
                    const tanggal = moment(data).format("DD/MM/YYYY HH:MM:SS");
                    return tanggal;
                }
            }
            
        ]
    });
}

function detailClose() {
    var text = `<thead class="text-center">
                    <tr>
                        <th>POSITION</th>
                        <th>STATUS</th>
                        <th>PIC</th>
                        @*<th>REMAKS</th>*@
                        <th>APPROVAL DATE</th>
                    </tr>
                </thead>
                <tbody>
                </tbody>`;
    table2.destroy();
    table2.val(text);
}

function printReport(ppeno) {
    var printUrl = "http://10.14.101.181/ReportServer_RPTPROD?/PPE/ReviewPPE&PPE_NO=" + ppeno;
    window.open(printUrl, '_blank');
}

var table = $("#tbl_reviewppe").DataTable({
    dom: 'Bfrtip',
    buttons: [
        {
            extend: "pdfHtml5",
            title: "REVIEW PPE",
            exportOptions: {
                columns: [0, 1, 2, 3, 4, 5, 6, 7, 8]
            },
            customize: function (doc) {
                doc.content[1].margin = [0, 0, 0, 0]
            },
            orientation: 'landscape'
        },
    ],
    ajax: {
        url: $("#web_link").val() + "/api/PPE/Get_FirstNoPPE",
        dataSrc: "Data",
    },
    "columnDefs": [
        { "className": "dt-center", "targets": [0, 1, 2, 3, 4, 5, 6, 7] },
        { "className": "dt-nowrap", "targets": '_all' }
    ],
    scrollX: true,
    columns: [
        {
            data: 'CREATED_DATE',
            render: function (data, type, row) {
                const tanggal = moment(data).format("DD/MM/YYYY");
                return tanggal;
            }
        },
        { data: 'PPE_NO' },
        { data: 'EGI' },
        { data: 'EQUIP_NO' },
        { data: 'DISTRICT_FROM' },
        { data: 'DISTRICT_TO' },
        { data: 'POSISI_PPE' },
        {
            data: 'ID',
            targets: 'no-sort', orderable: false,
            render: function (data, type, row) {
                action = `<div class="btn-group">`
                action += `<button onClick="getdetail('${row.EQUIP_NO}')" type="button" class="btn btn-dark btn-sm" data-bs-toggle="modal" data-bs-target="#modal-detail">Detail</button>`
                action += `<button onClick="printReport('${row.PPE_NO}')" type="button" class="btn btn-primary btn-sm">Print</button>`
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
                var select = $('<select class="form-control form-control-sm" style="width:200px; display:inline-block; margin-left: 10px;"><option value="">-- PPE NUMBER --</option></select>')
                    .appendTo($("#tbl_reviewppe_filter.dataTables_filter"))
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