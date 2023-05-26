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
                text += '<option value="' + val.EQUIP_NO + '">' + val.EQUIP_NO + '</option>';
            });
            $("#txt_eqNumber").append(text);
        }
    });
}