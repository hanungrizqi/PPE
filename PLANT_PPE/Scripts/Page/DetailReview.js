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
            $("#txt_status").val(dataPPE.STATUS);
            $("#viewAttach").append("<a href='" + dataPPE.PATH_ATTACHMENT + "' target='_blank'>View Attachment</a>");
            $("#viewCAAB").append("<a href='  ' target='_blank'>View Document CAAB</a>");
        }
    });
}