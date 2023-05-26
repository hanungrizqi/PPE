Codebase.helpersOnLoad(['jq-select2']);

$("document").ready(function () {
    var txtDateInput = document.getElementById("txt_date");
    var today = new Date();
    var formattedDate = today.toISOString().split("T")[0];
    txtDateInput.value = formattedDate;
    txtDateInput.setAttribute("max", formattedDate);
})