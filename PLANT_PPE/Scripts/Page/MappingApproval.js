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
                text += '<option value="' + val.EQUIP_LOCATION + '">' + val.EQUIP_LOCATION + '</option>';
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
                text += '<option value="' + val.EQUIP_LOCATION + '">' + val.EQUIP_LOCATION + '</option>';
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

/*
function Editgetapprv_from() {
    $.ajax({
        url: $("#web_link").val() + "/api/Setting/Get_DistrictMap", //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            
            *//*text = '<option>PILIH</option>';*//*
            $.each(result.Data, function (key, val) {
                text += '<option value="' + val.DSTRCT_CODE + '">' + val.DSTRCT_CODE + '</option>';
            });
            $("#editapprv_from").append(text);

        }

    });
}

function Editgetapprv_to() {
    $.ajax({
        url: $("#web_link").val() + "/api/Setting/Get_DistrictMap", //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            
            *//*text = '<option>PILIH</option>';*//*
            $.each(result.Data, function (key, val) {
                text += '<option value="' + val.DSTRCT_CODE + '">' + val.DSTRCT_CODE + '</option>';
            });
            $("#editapprv_to").append(text);

        }


    });


}

function Editgetloc_from() {

    var district = $("#editapprv_from").val();

    $.ajax({
        url: $("#web_link").val() + "/api/Setting/Get_DistrictLocation?dstrct=" + district, //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            
            *//*text = '<option>PILIH</option>';*//*
            $.each(result.Data, function (key, val) {
                text += '<option value="' + val.LOCATION + '">' + val.LOCATION + '</option>';
            });
            $("#editloc_from").append(text);

        }

    });
}

function Editgetloc_to() {

    var district = $("#editapprv_to").val();

    $.ajax({
        url: $("#web_link").val() + "/api/Setting/Get_DistrictLocation?dstrct=" + district, //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            
            *//*text = '<option>PILIH</option>';*//*
            $.each(result.Data, function (key, val) {
                text += '<option value="' + val.LOCATION + '">' + val.LOCATION + '</option>';
            });
            $("#editloc_to").append(text);

        }

    });
}

function EditgetCurrPosition() {


    $.ajax({
        url: $("#web_link").val() + "/api/Setting/Get_Position",
        type: "GET",
        cache: false,
        success: function (result) {
            
            *//*text = '<option>PILIH</option>';*//*
            $.each(result.Data, function (key, val) {
                text += '<option value="' + val.POSITION_ID + '">' + val.POSITION_FULL + '</option>';
            });
            $("#editcurr_position").append(text);

        }

    });
}

function EditgetNextPosition() {


    $.ajax({
        url: $("#web_link").val() + "/api/Setting/Get_Position",
        type: "GET",
        cache: false,
        success: function (result) {
           
            *//*text = '<option>PILIH</option>';*//*
            $.each(result.Data, function (key, val) {
                text += '<option value="' + val.POSITION_ID + '">' + val.POSITION_FULL + '</option>';
            });
            $("#editnext_position").append(text);

        }

    });
}*/

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

function updateMapApproval() {
    let obj = new Object();
    obj.APPROVAL_NO = $('#apprv_no').val();
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
        url: $("#web_link").val() + "/api/Setting/Update_MappingApproval", //URI
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
function showInsert(){
    $('#buttonUpdate').hide();
    $('#buttonInsert').show();
}
function editMapApproval(approveNo) {

    $('#buttonUpdate').show();
    $('#buttonInsert').hide();


    var text1;
    var text2;
    var text3;
    var text4;
    var text5;
    var text6;
    var text7;
    var text8;
    var text9;
    var text10;
    var text11;
    
    
    $.ajax({
        url: $("#web_link").val() + "/api/Setting/Get_MappingbyId", //URI,
        type: "GET",
        data: {
            ApproveNum: approveNo
            },
        cache: false,
        success: function (result) {
            console.log(result);
            $.each(result.Data, function (key, val) {

                if (val.APPROVAL_ACTION == 0) {
                    text1 = '<option selected value="' + val.APPROVAL_ACTION + '"> REJECT </option>';
                } else if (val.APPROVAL_ACTION == 1) {
                    text1 = '<option selected value="' + val.APPROVAL_ACTION + '">  APPROVE  </option>';
                }
                
                text2 = val.APPROVAL_ORDER;
                text3 = '<option  value="' + val.APPROVAL_FROM + '" selected >' + val.APPROVAL_FROM + '</option>';
                text4 = '<option  value="' + val.APPROVAL_TO + '" selected >' + val.APPROVAL_TO + '</option>'; 
                text5 = '<option  value="' + val.LOCATION_FROM + '" selected >' + val.LOCATION_FROM + '</option>';
                text6 = '<option  value="' + val.LOCATION_TO + '" selected >' + val.LOCATION_TO + '</option>'; 
                text7 = val.CURR_POSITION_ID; 
                text8 = val.NEXT_POSITION_ID ; 
                text9 = val.APPROVAL_STATUS;
                text10 = val.CURRENT_STATUS;
                text11 = val.APPROVAL_NO;



            });

            $('#apprv_no').val(text11);
            $('#txt_action').append(text1);
            $('#apprv_order').val(text2);
            $('#apprv_from').append(text3);
            $('#apprv_to').append(text4);
            $('#loc_from').append(text5);
            $('#loc_to').append(text6);
            /*$('#curr_position').append(text7);
            $('#next_position').append(text8);*/
            $('#apprv_status').val(text9);
            $('#curr_status').val(text10);


            $.ajax({
                url: $("#web_link").val() + "/api/Setting/Get_PositionById",
                type: "GET",
                data: {
                    id : text7
                    },
                cache: false,
                success: function (result) {
                    
                    
                    $.each(result.Data, function (key, val) {
                        text += '<option value="' + val.POSITION_ID + '" selected>' + val.POSITION_FULL + '</option>';
                    });
                    $("#curr_position").append(text);

                }

            });

            $.ajax({
                url: $("#web_link").val() + "/api/Setting/Get_PositionById",
                type: "GET",
                data: {
                    id : text8
                },
                cache: false,
                success: function (result) {
                  
                    $.each(result.Data, function (key, val) {
                        text += '<option value="' + val.POSITION_ID + '" selected >' + val.POSITION_FULL + '</option>';
                    });
                    $("#next_position").append(text);

                }

            });



            getloc_from();
            getloc_to();
            /*Editgetapprv_from();
            Editgetapprv_to();
            Editgetloc_from();
            Editgetloc_to();
            EditgetCurrPosition();
            EditgetNextPosition();*/

        }
        
    });


}

function deleteMapApproval(apprvNumber) {
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
                url: $("#web_link").val() + "/api/Setting/Delete_MappingApproval?id=" + apprvNumber , //URI
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
                action += `<button onClick="editMapApproval('${row.APPROVAL_NO}')" type="button" class="btn btn-dark btn-sm" data-bs-toggle="modal" data-bs-target="#modal-insert">Edit</button>
                           <button type="button" onclick="deleteMapApproval('${row.APPROVAL_NO}')" class="btn btn-sm btn-danger" title="Delete">Del</button> `

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