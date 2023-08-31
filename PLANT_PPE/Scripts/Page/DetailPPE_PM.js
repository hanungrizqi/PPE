$("document").ready(function () {
    getDetail();
})

function getDetail() {
    $.ajax({
        //url: $("#web_link").val() + "/api/PPE/Get_PPEDetail/" + (URLEncoder.encode($("#txt_noPPE").val, "UTF-8")), //URI,
        url: $("#web_link").val() + "/api/PPE/Get_PPEDetail/" + $("#id_ppe").val(), //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            //debugger
            var dataPPE = result.Data;
            $("#txt_noPPE").val(dataPPE.PPE_NO);
            $("#txt_eqNumber").val(dataPPE.EQUIP_NO);
            $("#txt_ppeDesc").val(dataPPE.PPE_DESC);
            $("#txt_egi").val(dataPPE.EGI);
            $("#txt_eqClass").val(dataPPE.EQUIP_CLASS);
            $("#txt_serialNo").val(dataPPE.SERIAL_NO);
            $("#txt_date").val(moment(dataPPE.DATE).format("YYYY-MM-DD"));
            $("#txt_districtFrom").val(dataPPE.DISTRICT_FROM);
            $("#txt_districtTo").val(dataPPE.DISTRICT_TO);
            $("#txt_locFrom").val(dataPPE.LOC_FROM);
            $("#txt_locTo").val(dataPPE.LOC_TO);
            $("#txt_note").val(dataPPE.REMARKS);
            $("#txtstatuss").val(dataPPE.STATUS);
        }
    });
}

function submitApproval(postStatus) {
    debugger
    if ($("#txt_note").val() == "" || $("#txt_note").val() == null) {
        Swal.fire(
            'Warning',
            'Mohon sertakan Remarks Approval!',
            'warning'
        );
        return;
    }
    let distrikfrom = $("#txt_districtFrom").val();
    let statss = $("#txtstatuss").val();
    debugger
    let dataEQP = new Object();
    dataEQP.PPE_NO = $("#txt_noPPE").val();
    dataEQP.EQUIP_NO = $("#txt_eqNumber").val();
    dataEQP.REMARKS = $("#txt_note").val();
    dataEQP.POSISI_PPE = postStatus === "REJECT" ? "Plant Manager" : "Plant Dept. Head";
    dataEQP.UPDATED_BY = $("#hd_nrp").val();
    dataEQP.STATUS = postStatus;
    dataEQP.APPROVAL_ORDER = postStatus === "REJECT" ? 3 : 3;
    dataEQP.URL_FORM_PLNTDH = "http://10.14.101.181/ReportServer_RPTPROD?/PPE/Rpt_PPE_PlantDeptHead&PPE_NO=" + $("#txt_noPPE").val();

    debugger
    let NomorPPEM = dataEQP.PPE_NO;
    console.log(NomorPPEM);
    console.log(dataEQP);

    $.ajax({
        url: $("#web_link").val() + "/api/DetailApproval/Approve_PPE_PlantManager",
        data: JSON.stringify(dataEQP),
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {
            $("#overlay").show();
        },
        success: function (data) {
            //debugger
            //sendMailPlant_DeptHead(NomorPPEM);
            debugger
            if (distrikfrom === "KPHO" && statss !== "PLANT ADM & DEV MANAGER APPROVED") {
                sendMailPlant_Manager(NomorPPEM);
            } else if (distrikfrom === "KPHO" && statss === "PLANT ADM & DEV MANAGER APPROVED") {
                sendMailPM_penerima(NomorPPEM);
            } else {
                sendMailPlant_DeptHead(NomorPPEM);
            }
        },
        error: function (xhr) {
            alert(xhr.responseText);
            $("#overlay").hide();
        }
    });
}

function sendMailPlant_DeptHead(NomorPPEM) {
    var encodedPPENo = encodeURIComponent(NomorPPEM.replace(/\//g, '%2F'));
    debugger
    $.ajax({
        url: $("#web_link").val() + "/api/DetailApproval/Sendmail_Plant_DeptHead?ppe=" + encodedPPENo,
        //url: $("#web_link").val() + "/api/DetailApproval/Sendmail_Plant_Manager",
        //data: JSON.stringify(NomorPPEM),
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
                        window.location.href = "/Approval/PlantManager";
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

function sendMailPlant_Manager(NomorPPEM) {
    var encodedPPENo = encodeURIComponent(NomorPPEM.replace(/\//g, '%2F'));
    debugger
    $.ajax({
        url: $("#web_link").val() + "/api/DetailApproval/Sendmail_Plant_Manager?ppe=" + encodedPPENo,
        //url: $("#web_link").val() + "/api/DetailApproval/Sendmail_Plant_Manager",
        //data: JSON.stringify(NomorPPEM),
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
                        window.location.href = "/Approval/PlantManager";
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

function sendMailPM_penerima(NomorPPEM) {
    var encodedPPENo = encodeURIComponent(NomorPPEM.replace(/\//g, '%2F'));
    debugger
    $.ajax({
        url: $("#web_link").val() + "/api/DetailApproval/Sendmail_PM_Penerima?ppe=" + encodedPPENo,
        //url: $("#web_link").val() + "/api/DetailApproval/Sendmail_Plant_Manager",
        //data: JSON.stringify(NomorPPEM),
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
                        window.location.href = "/Approval/PlantManager";
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

function rejectApproval(postStatus) {
    debugger
    if ($("#txt_note").val() == "" || $("#txt_note").val() == null) {
        Swal.fire(
            'Warning',
            'Mohon sertakan Remarks Approval!',
            'warning'
        );
        return;
    }
    debugger
    let dataEQP = new Object();
    dataEQP.PPE_NO = $("#txt_noPPE").val();
    dataEQP.EQUIP_NO = $("#txt_eqNumber").val();
    dataEQP.REMARKS = $("#txt_note").val();
    dataEQP.POSISI_PPE = postStatus === "REJECT" ? "Plant Manager" : "Plant Dept. Head",
    dataEQP.UPDATED_BY = $("#hd_nrp").val();
    dataEQP.STATUS = postStatus;
    dataEQP.APPROVAL_ORDER = postStatus === "REJECT" ? 3 : 3;
    dataEQP.URL_FORM_PLNTMNGR = "http://10.14.101.181/ReportServer_RPTPROD?/PPE/Rpt_PPE_PlantManager&PPE_NO=" + $("#txt_noPPE").val();

    debugger
    let NomorPPEM = dataEQP.PPE_NO;
    console.log(NomorPPEM);
    console.log(dataEQP);

    $.ajax({
        url: $("#web_link").val() + "/api/DetailApproval/Reject_PPE_PlantManager",
        data: JSON.stringify(dataEQP),
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {
            $("#overlay").show();
        },
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
                        window.location.href = "/Approval/PlantManager";
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
            $("#overlay").hide();
        }
    });
}