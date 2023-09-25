//KP1PT015
//KP1PT015
//KP1PT015
//KP1PT02
//RT1D01
//RT11
//AS11
//KP0161
//KP0111

Codebase.helpersOnLoad(['jq-select2']);

$("document").ready(function () {
    var txtDateInput = document.getElementById("txt_date");
    var today = new Date();
    var formattedDate = today.toISOString().split("T")[0];
    txtDateInput.value = formattedDate;
    txtDateInput.setAttribute("max", formattedDate);
    getDistrict();
    getDistrictTO();
    $('#modal-terms').on('show.bs.modal', function () {
        getContent();
    });
    $('#modal-terms').on('hidden.bs.modal', function () {
        var agreeCheckbox = document.getElementById('val-terms');
        agreeCheckbox.checked = true; //kalo diklik accept, cek box jadi aktif
    });

    $('#modal-terms').on('click', '.btn.btn-alt-primary', function () {
        var agreeCheckbox = document.getElementById('val-terms');
        agreeCheckbox.disabled = false; //kalo baca modal, cek box aktip
    });
})

$("#txt_eqNumber").on("change", function () {
    let egi = $(this).find(':selected').attr('data-egi');
    $("#txt_egi").val(egi);
    let eqClass = $(this).find(':selected').attr('data-eqclass');
    $("#txt_eqClass").val(eqClass);
    let Sn = $(this).find(':selected').attr('data-Sn');
    $("#txt_serialNo").val(Sn);
    let desc = $(this).find(':selected').attr('data-desc');
    $("#txt_ppeDesc").val(desc);
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
                text += '<option value="' + val.TABLE_CODE + '">' + val.TABLE_CODE + '</option>';
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
                text += '<option value="' + val.TABLE_CODE + '">' + val.TABLE_CODE + '</option>';
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
                text += '<option value="' + val.EQUIP_NO + '" data-egi="' + val.EQUIP_GRP_ID + '" data-eqclass="' + val.EQUIP_CLASS + '" data-Sn="' + val.SERIAL_NUMBER + '" data-desc="' + val.DESCRIPTION + '">' + val.EQUIP_NO + '</option>';
            });
            $("#txt_eqNumber").append(text);

        }
    });
}

var savePPEtoTableClicked = false;
function savePPEtoTable() { 
    var agreeCheckbox = document.getElementById('val-terms');
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

    if (districtFrom == "" || locFrom == "" || districtTo == "" || locTo == "" || eqNumber == "" || ppeDescription == "") {
        Swal.fire(
            'Warning!',
            'Mohon lengkapi data!',
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
    } else if (attch != "") {
        var file = $("#txt_attach")[0].files[0];

        var formData = new FormData();
        formData.append("attachment", file);
        
        debugger
        let pep = new Object();
        pep.PPE_NO = $("#txt_noPPE").val(),
        pep.EQUIP_NO = $("#txt_eqNumber").val(),
        $.ajax({
            url: $("#web_link").val() + "/api/PPE/Cek_History_Part", //URI
            data: JSON.stringify(pep),
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                debugger
                if (data.Remarkss == true) {
                    debugger
                    Swal.fire({
                        title: 'Warning!',
                        text: data.Messages,
                        icon: 'warning',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK',
                        allowOutsideClick: false,
                        allowEscapeKey: false
                    })
                } if (data.Remarkss == false) {
                    $.ajax({
                        url: $("#web_link").val() + "/api/PPE/UploadAttachment", // URI
                        type: "POST",
                        data: formData,
                        processData: false,
                        contentType: false,
                        success: function (response) {
                            debugger
                            var attachmentUrl = response.AttachmentUrl;

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
                                "<td><a href='" + attachmentUrl + "' target='_blank'>View Attachment</a></td>" +
                                "<td><button class='btn btn-sm btn-danger' onclick='removeRow(this)'>DELETE</button></td>" +
                                "</tr>";
                            $("#table_equipment tbody").append(row);
                            savePPEtoTableClicked = true;
                            
                            var toastElement = document.getElementById('toast-example-1');
                            var toast = new bootstrap.Toast(toastElement);
                            toast.show();

                            // Clear input fields
                            $("#txt_eqNumber").val("").trigger("change");
                            $("#txt_ppeDesc").val("");
                            $("#txt_egi").val("");
                            $("#txt_eqClass").val("");
                            $("#txt_serialNo").val("");
                            $("#txt_remark").val("");
                            $("#txt_attach").val("");
                        },
                        error: function (xhr) {
                            alert(xhr.responseText);
                        }
                    });
                }

            },
            error: function (xhr) {
                alert(xhr.responseText);
            }
        });
    } else if (attch == "") {
        debugger
        let pep = new Object();
        pep.PPE_NO = $("#txt_noPPE").val(),
        pep.EQUIP_NO = $("#txt_eqNumber").val(),
        $.ajax({
            url: $("#web_link").val() + "/api/PPE/Cek_History_Part", //URI
            data: JSON.stringify(pep),
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                debugger
                if (data.Remarkss == true) {
                    debugger
                    Swal.fire({
                        title: 'Warning!',
                        text: data.Messages,
                        icon: 'warning',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK',
                        allowOutsideClick: false,
                        allowEscapeKey: false
                    })
                } if (data.Remarkss == false) {
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
                        "<td><button class='btn btn-sm btn-danger' onclick='removeRow(this)'>DELETE</button></td>" +
                        "</tr>";
                    $("#table_equipment tbody").append(row);
                    savePPEtoTableClicked = true;
                    
                    var toastElement = document.getElementById('toast-example-1');
                    var toast = new bootstrap.Toast(toastElement);
                    toast.show();

                    // Clear input fields
                    $("#txt_eqNumber").val("").trigger("change");
                    $("#txt_ppeDesc").val("");
                    $("#txt_egi").val("");
                    $("#txt_eqClass").val("");
                    $("#txt_serialNo").val("");
                    $("#txt_remark").val("");
                    $("#txt_attach").val("");
                }

            },
            error: function (xhr) {
                alert(xhr.responseText);
            }
        });
    }
    $("#txt_districtTo").prop("disabled", true);
    //$("#txt_locTo").prop("disabled", true);
};

function formatDate(date) {
    var parts = date.split("-");
    var day = parts[2];
    var month = parts[1];
    var year = parts[0];
    return day + "/" + month + "/" + year;
}

function savePPE(postStatus) {
    debugger
    if (!savePPEtoTableClicked) {
        Swal.fire(
            'Warning!',
            'Tidak bisa Submit sebelum Save.',
            'warning'
        );
        return;
    }
    var tableData = [];

    $("#table_equipment tbody tr").each(function () {
        debugger
        var url = window.location.origin;
        var rowData = {};
        var cells = $(this).find("td");

        rowData.DATE = $("#txt_date").val();
        rowData.PPE_NO = cells.eq(2).text();
        rowData.EQUIP_NO = cells.eq(3).text();
        rowData.PPE_DESC = cells.eq(4).text();
        rowData.EGI = cells.eq(5).text();
        rowData.EQUIP_CLASS = cells.eq(6).text();
        rowData.SERIAL_NO = cells.eq(7).text();
        rowData.DISTRICT_FROM = cells.eq(8).text();
        rowData.LOC_FROM = cells.eq(9).text();
        rowData.DISTRICT_TO = cells.eq(10).text();
        rowData.LOC_TO = cells.eq(11).text();
        rowData.REMARKS = cells.eq(12).text();
        rowData.PATH_ATTACHMENT = cells.eq(13).find("a").attr("href");
        rowData.CREATED_BY = $("#txt_createBy").val();
        rowData.CREATED_POS_BY = $("#hd_PositionID").val();
        rowData.STATUS = postStatus;
        rowData.APPROVAL_ORDER = 1;
        rowData.URL_FORM_SH = "http://10.14.101.181/ReportServer_RPTPROD?/PPE/Rpt_PPE_SecHead&PPE_NO=" + $("#txt_noPPE").val();
        
        if (postStatus == "CREATED") {
            rowData.POSISI_PPE = "Sect. Head";
        } else {
            rowData.POSISI_PPE = $("#txt_posPPE").val();
        }

        tableData.push(rowData);
    });

    $.ajax({
        url: $("#web_link").val() + "/api/PPE/Create_PPE", // URI
        data: JSON.stringify(tableData),
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {
            $("#overlay").show();
        },
        success: function (data) {
            if (data.Remarks == true) {
                debugger
                Insert_Approved_Create(tableData);
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
            $("#overlay").hide();
        }
    });

    console.log(tableData);
}

function getContent() {
    $.ajax({
        url: $("#web_link").val() + "/api/Setting/Get_Agreement", //URI
        dataType: "json",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var content = data.Data;
            $("#agreeModals").html(content);

        },
        error: function (xhr) {
            alert(xhr.responseText);
        }
    });
}

function Insert_Approved_Create(tableData) {
    debugger
    $.ajax({
        url: $("#web_link").val() + "/api/PPE/Insert_Approved_Create",
        data: JSON.stringify(tableData),
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.Remarks == true) {
                debugger
                sendMailSection_head($("#txt_noPPE").val());
            } else if (data.Remarks == false) {
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


function sendMailSection_head(ppenosh) {
    debugger
    var encodedPPENo = encodeURIComponent(ppenosh);
    debugger
    $.ajax({
        url: $("#web_link").val() + "/api/PPE/Sendmail_Section_Head?ppe=" + encodedPPENo,
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.Remarks) {
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
                        window.location.href = "/PPE/Register";
                    }
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
    });
}
