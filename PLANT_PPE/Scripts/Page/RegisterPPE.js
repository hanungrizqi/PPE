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

function getEqNumber() {
    $.ajax({
        url: $("#web_link").val() + "/api/Master/Get_EqNumber/" + selectedDistrict + "/" + $("#hd_nrp").val(), //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            $('#txt_eqNumber').empty();
            text = '<option></option>';
            $.each(result.Data, function (key, val) {
                text += '<option value="' + val.EQUIP_NO + '" data-egi="' + val.EQUIP_GRP_ID + '" data-eqclass="' + val.EQUIP_CLASS + '" data-Sn="' + val.SERIAL_NUMBER + '">' + val.EQUIP_NO + '</option>';
            });
            $("#txt_eqNumber").append(text);

        }
    });
}

$("#savePPEtoTable").click(function () {
    var date = $("#txt_date").val();
    var formattedDate = formatDate(date);

    var ppeNo = $("#txt_noPPE").val();
    var eqNumber = $("#txt_eqNumber").val();
    var ppeDescription = $("#txt_ppeDesc").val();
    var egi = $("#txt_egi").val();
    var eqClass = $("#txt_eqClass").val();
    var serialNumber = $("#txt_serialNo").val();
    var districtFrom = $("#txt_districtFrom").val();
    var locFrom = $("#txt_locFrom").val();
    var districtTo = $("#txt_districtTo").val();
    var locTo = $("#txt_locTo").val();
    var remark = $("#txt_remark").val();
    var attch = $("#txt_attach").val();

    var rowCount = $("#table_equipment tbody tr").length + 1;
    var row = "<tr>" +
        "<td class='text-center'>" + rowCount + "</td>" +
        "<td>" + formattedDate + "</td>" +
        "<td>" + ppeNo + "</td>" +
        "<td>" + eqNumber + "</td>" +
        "<td>" + ppeDescription + "</td>" +
        "<td>" + egi + "</td>" +
        "<td>" + eqClass + "</td>" +
        "<td>" + serialNumber + "</td>" +
        "<td>" + districtFrom + "</td>" +
        "<td>" + locFrom + "</td>" +
        "<td>" + districtTo + "</td>" +
        "<td>" + locTo + "</td>" +
        "<td>" + remark + "</td>" +
        "<td>" + attch + "</td>" +
        "</tr>";

    $("#table_equipment tbody").append(row);

    // Clear input
    //$("#txt_date").val("");
    //$("#txt_noPPE").val("");
    //$("#txt_eqNumber").val("").trigger("change");
    //$("#txt_ppeDesc").val("");
    //$("#txt_egi").val("");
    //$("#txt_eqClass").val("");
    //$("#txt_serialNo").val("");
    //$("#txt_districtFrom").val("").trigger("change");
    //$("#txt_locFrom").val("").trigger("change");
    //$("#txt_districtTo").val("").trigger("change");
    //$("#txt_locTo").val("").trigger("change");
});

function formatDate(date) {
    var parts = date.split("-");
    var day = parts[2];
    var month = parts[1];
    var year = parts[0];
    return day + "/" + month + "/" + year;
}

$("#savePPE").click(function () {
    var tableData = [];
    $("#table_equipment tbody tr").each(function () {
        var rowData = [];
        $(this).find("td").each(function () {
            rowData.push($(this).text());
        });
        tableData.push(rowData);
    });

    $.ajax({
        url: $("#web_link").val() + "/api/PPE/Create_PPE", //URI
        data: tableData,
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            debugger
            if (data.Remarks == true) {
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
                        window.location.href = "/Home/Index";
                    }
                });
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

    console.log(tableData);
});
