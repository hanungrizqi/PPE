Codebase.helpersOnLoad(['jq-select2']);

$("document").ready(function () {
    $('.select2-modal').select2({
        dropdownParent: $('.modal')
    });

   
})

function getReview() {

}

var table = $("#tbl_reviewppe").DataTable({
    ajax: {
        url: $("#web_link").val() + "/api/PPE/Get_FirstNoPPE",
        dataSrc: "Data",
    },
    "columnDefs": [
        { "className": "dt-center", "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8 ,9] }
    ],
    scrollX: true,
    columns: [
        { data: 'CREATED_DATE' },
        { data: 'PPE_NO' },
        { data: 'EGI' },
        { data: 'EQUIP_CLASS' },
        { data: 'EQUIP_NO' },
        { data: 'SERIAL_NO' },
        { data: 'DISTRICT_FROM' },
        { data: 'DISTRICT_TO' },
        { data: 'POSISI_PPE' },
        {
            data: 'ID',
            targets: 'no-sort', orderable: false,
            render: function (data, type, row) {
                action = `<div class="btn-group">`
                action += `<a href="/PPE/DetailPPE?idppe=${data}" class="btn btn-sm btn-info">Detail</a>`

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
/*
editMapApproval('${row.ID}')*/