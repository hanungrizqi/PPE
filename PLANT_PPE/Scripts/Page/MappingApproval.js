Codebase.helpersOnLoad(['jq-select2']);

$("document").ready(function () {
    $('.select2-modal').select2({
        dropdownParent: $('.modal')
    });

    getapprv_from();
    getapprv_to();
    getCurrPosition();
    getNextPosition();
})



function getapprv_from() {
    $.ajax({
        url: $("#web_link").val() + "/api/Setting/Get_DistrictMap", //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            $('#apprv_from').empty();
            text = '<option>PILIH</option>';
            $.each(result.Data, function (key, val) {
                text += '<option value="' + val.DSTRCT_CODE + '">' + val.DSTRCT_CODE + '</option>';
            });
            $("#apprv_from").append(text);

        }

    });
}

function getapprv_to() {
    $.ajax({
        url: $("#web_link").val() + "/api/Setting/Get_DistrictMap", //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            $('#apprv_to').empty();
            text = '<option>PILIH</option>';
            $.each(result.Data, function (key, val) {
                text += '<option value="' + val.DSTRCT_CODE + '">' + val.DSTRCT_CODE + '</option>';
            });
            $("#apprv_to").append(text);

        }

        
    });

    
}

function getloc_from() {

    var district = $("#apprv_from").val();

    $.ajax({
        url: $("#web_link").val() + "/api/Setting/Get_DistrictLocation?dstrct=" + district, //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            $('#loc_from').empty();
            text = '<option>PILIH</option>';
            $.each(result.Data, function (key, val) {
                text += '<option value="' + val.LOCATION + '">' + val.LOCATION + '</option>';
            });
            $("#loc_from").append(text);

        }

    });
}

function getloc_to() {

    var district = $("#apprv_to").val();

    $.ajax({
        url: $("#web_link").val() + "/api/Setting/Get_DistrictLocation?dstrct=" + district, //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            $('#loc_to').empty();
            text = '<option>PILIH</option>';
            $.each(result.Data, function (key, val) {
                text += '<option value="' + val.LOCATION + '">' + val.LOCATION + '</option>';
            });
            $("#loc_to").append(text);

        }

    });
}

function getCurrPosition() {


    $.ajax({
        url: $("#web_link").val() + "/api/Setting/Get_Position" ,
        type: "GET",
        cache: false,
        success: function (result) {
            $('#curr_position').empty();
            text = '<option>PILIH</option>';
            $.each(result.Data, function (key, val) {
                text += '<option value="' + val.POSITION_ID + '">' + val.POSITION_FULL + '</option>';
            });
            $("#curr_position").append(text);

        }

    });
}

function getNextPosition() {


    $.ajax({
        url: $("#web_link").val() + "/api/Setting/Get_Position",
        type: "GET",
        cache: false,
        success: function (result) {
            $('#next_position').empty();
            text = '<option>PILIH</option>';
            $.each(result.Data, function (key, val) {
                text += '<option value="' + val.POSITION_ID + '">' + val.POSITION_FULL + '</option>';
            });
            $("#next_position").append(text);

        }

    });
}

function insertMapApproval() {
    let obj = new Object();
    obj.APPROVAL_ACTION = $('#txt_action').val(); 
    obj.APPROVAL_ORDER = $('#apprv_order').val();
    obj.APPROVAL_FROM = $('#apprv_from').val();
    obj.APPROVAL_TO = $('#apprv_to').val();
    obj.CURR_POSITION_ID = $('#curr_position').val();
    obj.NEXT_POSITION_ID = $('#next_position').val();
    obj.APPROVAL_STATUS = $('#apprv_status').val();
    obj.CURRENT_STATUS = $('#curr_status').val();
    obj.LOCATION_FROM = $('#loc_from').val();
    obj.LOCATION_TO = $('#loc_to').val();

    console.log(obj);

    $.ajax({
        url: $("#web_link").val() + "/api/Setting/Create_MappingApproval", //URI
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


var table = $("#tbl_mappingApproval").DataTable({
    ajax: {
        url: $("#web_link").val() + "/api/Setting/Get_MappingApproval",
        dataSrc: "Data",
    },
    "columnDefs": [
        { "className": "dt-center", "targets": [0, 1,2,3,4,5,6,7,8] }
    ],
    scrollX: true,
    columns: [
        { data: 'APPROVAL_ACTION' },
        { data: 'APPROVAL_ORDER' },
        { data: 'APPROVAL_FROM' },
        { data: 'APPROVAL_TO' },
        { data: 'LOCATION_FROM' },
        { data: 'LOCATION_TO' },
        { data: 'CURR_POSITION_ID' },
        { data: 'NEXT_POSITION_ID' },
        { data: 'APPROVAL_STATUS' },
        { data: 'CURRENT_STATUS' },
        {
            render: function (data, type, row) {
                action = `<div class="btn-group">`
                action += `<button type="button" onclick="deleteDstrct(${row.APPROVAL_NO})" class="btn btn-sm btn-danger" title="Delete">Delete
                                </button>`
                action += `</div>`
                return action;
            }
        }
        
    ],

});


/*
function deleteDstrct(id) {
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
                url: $("#web_link").val() + "/api/Setting/Delete_District?id=" + id, //URI
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
}*/