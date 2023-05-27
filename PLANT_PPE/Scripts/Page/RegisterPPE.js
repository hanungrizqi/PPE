Codebase.helpersOnLoad(['jq-select2']);

$("document").ready(function () {
    var txtDateInput = document.getElementById("txt_date");
    var today = new Date();
    var formattedDate = today.toISOString().split("T")[0];
    txtDateInput.value = formattedDate;
    txtDateInput.setAttribute("max", formattedDate);
    getDistrict();
    getDistrictTO();
})

$("#txt_eqNumber").on("change", function () {
    let egi = $(this).find(':selected').attr('data-egi');
    $("#txt_egi").val(egi);
    let eqClass = $(this).find(':selected').attr('data-eqclass');
    $("#txt_eqClass").val(eqClass);
    let Sn = $(this).find(':selected').attr('data-Sn');
    $("#txt_serialNo").val(Sn);
})

//Start District From
var selectedDistrict;
function getDistrict() {
    $.ajax({
        url: $("#web_link").val() + "/api/Master/getDistrict", //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            $('#txt_districtFrom').empty();
            text = '<option></option>';
            $.each(result.Data, function (key, val) {
                text += '<option value="' + val.DSTRCT_CODE + '">' + val.DSTRCT_CODE + '</option>';
            });
            $("#txt_districtFrom").append(text);
            $("#txt_districtFrom").change(function () {
                selectedDistrict = $(this).val();
                getLoc();
                getEqNumber();
            });
            
        }
    });
}

function getLoc() {
    $.ajax({
        url: $("#web_link").val() + "/api/Master/getLoc/" + selectedDistrict, //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            $('#txt_locFrom').empty();
            text = '<option></option>';
            $.each(result.Data, function (key, val) {
                text += '<option value="' + val.EQUIP_LOCATION + '">' + val.EQUIP_LOCATION + '</option>';
            });
            $("#txt_locFrom").append(text);
        }
    });
}
//End District From
//Start District To
var selectedDistrictTO;
function getDistrictTO() {
    $.ajax({
        url: $("#web_link").val() + "/api/Master/getDistrict", //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            $('#txt_districtTo').empty();
            text = '<option></option>';
            $.each(result.Data, function (key, val) {
                text += '<option value="' + val.DSTRCT_CODE + '">' + val.DSTRCT_CODE + '</option>';
            });
            $("#txt_districtTo").append(text);
            $("#txt_districtTo").change(function () {
                selectedDistrictTO = $(this).val();
                getLocTO();
            });

        }
    });
}

function getLocTO() {
    $.ajax({
        url: $("#web_link").val() + "/api/Master/getLoc/" + selectedDistrictTO, //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            $('#txt_locTo').empty();
            text = '<option></option>';
            $.each(result.Data, function (key, val) {
                text += '<option value="' + val.EQUIP_LOCATION + '">' + val.EQUIP_LOCATION + '</option>';
            });
            $("#txt_locTo").append(text);
        }
    });
}
//End District To

function getEqNumber() {
    $.ajax({
        url: $("#web_link").val() + "/api/Master/Get_EqNumber/" + selectedDistrict + "/" + $("#hd_nrp").val(), //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            $('#txt_eqNumber').empty();
            text = '<option></option>';
            $.each(result.Data, function (key, val) {
                //text += '<option value="' + val.EQUIP_NO + '">' + val.EQUIP_NO + '</option>';
                text += '<option value="' + val.EQUIP_NO + '" data-egi="' + val.EQUIP_GRP_ID + '" data-eqclass="' + val.EQUIP_CLASS + '" data-Sn="' + val.SERIAL_NUMBER + '">' + val.EQUIP_NO + '</option>';
            });
            $("#txt_eqNumber").append(text);

        }
    });
}
function savePPEtoTable() {
    debugger
    let dataPPE = new Object();
    dataPPE.DATE = $("#txt_date").val();
    dataPPE.PPE_NO = $("#txt_noPPE").val();
    dataPPE.DISTRICT_FROM = $("#txt_districtFrom").val();
    dataPPE.DISTRICT_TO = $("#txt_districtTo").val();
    dataPPE.LOC_FROM = $("#txt_locFrom").val();
    dataPPE.LOC_TO = $("#txt_locTo").val();
    dataPPE.EQUIP_NO = $("#txt_eqNumber").val();
    dataPPE.PPE_DESC = $("#txt_ppeDesc").val();
    dataPPE.EGI = $("#txt_egi").val();
    dataPPE.EQUIP_CLASS = $("#txt_eqClass").val();
    dataPPE.SERIAL_NO = $("#txt_serialNo").val();
    dataPPE.REMARKS = $("#txt_remark").val();
    dataPPE.PATH_ATTACHMENT = $("#txt_attach").val();
    
    $.ajax({
        url: $("#web_link").val() + "/api/PPE/Save_Temporary_PPE", //URI
        data: JSON.stringify(dataPPE),
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.Remarks == true) {
                initializeDataTable();
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
    });
}

function initializeDataTable() {
    debugger
    var table = $("#table_equipment").DataTable({
        ajax: {
            url: $("#web_link").val() + "/api/PPE/Get_TemporaryPPE",
            dataSrc: "Data",
        },

        "columnDefs": [
            { "className": "dt-center", "targets": [0, 1, 2, 3, 5, 6, 7, 8, 9, 10] },
            { "className": "dt-nowrap", "targets": '_all' }
        ],
        scrollX: true,
        columns: [
            { data: 'PPE_NO' },
            {
                data: 'DATE',
                render: function (data, type, row) {
                    const tanggal = moment(data).format("DD/MM/YYYY");
                    return tanggal;
                }
            },
            { data: 'EQUIP_NO' },
            { data: 'PPE_DESC' },
            { data: 'EGI' },
            { data: 'EQUIP_CLASS' },
            { data: 'SERIAL_NO' },
            { data: 'DISTRICT_FROM' },
            { data: 'LOC_FROM' },
            { data: 'DISTRICT_TO' },
            { data: 'LOC_TO' },
            { data: 'REMARKS' },
            { data: 'PATH_ATTACHMENT' },
        ],

    });
    return table;
}

//$("#savePPEtoTable").click(function () {
//    var date = $("#txt_date").val();
//    var ppeNo = $("#txt_noPPE").val();
//    var eqNumber = $("#txt_eqNumber option:selected").val();
//    var ppeDescription = $("#txt_ppeDesc").val();
//    var egi = $("#txt_egi").val();
//    var eqClass = $("#txt_eqClass").val();
//    var serialNumber = $("#txt_serialNo").val();
//    var districtFrom = $("#txt_districtFrom option:selected").val();
//    var locFrom = $("#txt_locFrom option:selected").val();
//    var districtTo = $("#txt_districtTo option:selected").val();
//    var locTo = $("#txt_locTo option:selected").val();

//    var rowCount = $("#table_equipment tbody tr").length + 1;
//    var row = "<tr>" +
//        "<td class='text-center'>" + rowCount + "</td>" +
//        "<td>" + date + "</td>" +
//        "<td>" + ppeNo + "</td>" +
//        "<td>" + eqNumber + "</td>" +
//        "<td>" + ppeDescription + "</td>" +
//        "<td>" + egi + "</td>" +
//        "<td>" + eqClass + "</td>" +
//        "<td>" + serialNumber + "</td>" +
//        "<td>" + districtFrom + "</td>" +
//        "<td>" + locFrom + "</td>" +
//        "<td>" + districtTo + "</td>" +
//        "<td>" + locTo + "</td>" +
//        "</tr>";

//    $("#table_equipment tbody").append(row);

//    // Clear input fields after adding the row
//    $("#txt_date").val("");
//    $("#txt_noPPE").val("");
//    $("#txt_eqNumber").val("").trigger("change");
//    $("#txt_ppeDesc").val("");
//    $("#txt_egi").val("");
//    $("#txt_eqClass").val("");
//    $("#txt_serialNo").val("");
//    $("#txt_districtFrom").val("").trigger("change");
//    $("#txt_locFrom").val("").trigger("change");
//    $("#txt_districtTo").val("").trigger("change");
//    $("#txt_locTo").val("").trigger("change");
//});
