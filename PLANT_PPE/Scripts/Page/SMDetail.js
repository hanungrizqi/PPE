$("document").ready(function () {
    getDetail();
})

var table_eqp = $("#table_eqp").DataTable({
    "columnDefs": [
        { "className": "dt-center", "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10] },
        { "className": "dt-nowrap", "targets": [0, 1, 4, 9, 10] }
    ],
    scrollX: true,
});

function getDetail() {
    debugger
    var encodedPPENo = encodeURIComponent($("#txt_noPPE").val());
    debugger
    $.ajax({
        url: $("#web_link").val() + "/api/PPE/Detail_SM?ppe_no=" + encodedPPENo,
        type: "GET",
        cache: false,
        success: function (result) {
            var dataPPE = result.Data;
            $("#txt_noPPE").val(dataPPE.PPE_NO);
            $("#txt_date").val(moment(dataPPE.DATE).format("YYYY-MM-DD"));
            $("#txt_districtFrom").val(dataPPE.DISTRICT_FROM);
            $("#txt_districtTo").val(dataPPE.DISTRICT_TO);
        }
    });
}

function submitApproval(postStatus) {
    debugger
    var dataEquipment = [];
    var emptyFields = false;
    $.each($("#table_eqp tbody tr"), function () {
        debugger
        let ppe = {
            PPE_NO: $("#txt_noPPE").val(),
            EQUIP_NO: $(this).find("td:nth-child(1)").text(),
            EGI: $(this).find("td:nth-child(2)").text(),
            EQUIP_CLASS: $(this).find("td:nth-child(3)").text(),
            SERIAL_NO: $(this).find("td:nth-child(4)").text(),
            PPE_DESC: $(this).find("td:nth-child(5)").text(),
            DISTRICT_FROM: $(this).find("td:nth-child(6)").text(),
            DISTRICT_TO: $(this).find("td:nth-child(7)").text(),
            LOC_FROM: $(this).find("td:nth-child(8)").text(),
            LOC_TO: $(this).find("td:nth-child(9)").text(),
            DATE_RECEIVED_SM: $(this).find("input[name='txt_DateRecivSM']").val(),
            BERITA_ACARA_SM: $(this).find("input[name='txt_BA']").val(),
            POSISI_PPE: postStatus === "REJECT" ? "SM" : "DONE",
            UPDATED_BY: $("#hd_nrp").val(),
            STATUS: postStatus,
            APPROVAL_ORDER: 9,
        };
        if (!ppe.BERITA_ACARA_SM || !ppe.DATE_RECEIVED_SM) {
            emptyFields = true;
            return false;
        }
        dataEquipment.push(ppe);
    });
    console.log(dataEquipment);
    if (emptyFields) {
        Swal.fire(
            'Warning!',
            'Silahkan isi Date Receive atau Berita Acara!!',
            'warning'
        );
        return;
    }
    
    $.ajax({
        url: $("#web_link").val() + "/api/SM/Approve_SM",
        data: JSON.stringify(dataEquipment),
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {
            $("#overlay").show();
        },
        success: function (data) {
            if (data.Remarks == true) {
                debugger
                submitBA();
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

function submitBA() {
    debugger
    let selectedEquipNos = [];
    let selectedPpeNos = [];
    let attachmentFiles = [];
    $.each($("#table_eqp tbody tr"), function () {
        debugger
        let equipNo = $(this).find("td:nth-child(1)").text();
        let nomPPE = $("#txt_noPPE").val();
        selectedEquipNos.push(equipNo);
        selectedPpeNos.push(nomPPE);

        let files = $(this).find("td:nth-child(11)")[0].childNodes[1].files;
        for (let i = 0; i < files.length; i++) {
            attachmentFiles.push(files[i]);
        }
    });

    console.log(selectedEquipNos);
    console.log(selectedPpeNos);
    console.log(attachmentFiles);

    debugger
    let nomorPPE = selectedPpeNos;
    let attachmentFile = attachmentFiles;
    let nomorEQP = selectedEquipNos;

    let formData = new FormData();
    for (let i = 0; i < nomorPPE.length; i++) {
        formData.append('nomorPPE[]', nomorPPE[i]);
    }
    for (let i = 0; i < nomorEQP.length; i++) {
        formData.append('nomorEQP[]', nomorEQP[i]);
    }
    for (let i = 0; i < attachmentFile.length; i++) {
        formData.append('attachmentFiles', attachmentFile[i]);
    }

    debugger
    console.log(nomorPPE);
    console.log(nomorEQP);
    console.log(attachmentFile);

    $.ajax({
        url: $("#web_link").val() + "/api/SM/Upload_BA", //URI
        data: formData,
        type: "POST",
        contentType: false,
        processData: false,
        success: function (data) {
            if (data.Remarks == true) {
                sendMailPM_Pengirim(Array.from(uniquePPE_NO));
            } else if (data.Remarks == false) {
                Swal.fire({
                    title: 'Warning',
                    text: "File already exist.",
                    icon: 'warning',
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK',
                    allowOutsideClick: false,
                    allowEscapeKey: false
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
    })
}