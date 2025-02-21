function setCompensationId(Id) {
    $.ajax({
        url: '@Url.Action("getCompensationId","FAP")',
        datatype: "json",
        type: "post",
        data: { ID: Id },
        success: function (data) {
            if (data[0].isRequiredData == 1) {
                $("#dateofReleased").removeAttr('disabled');
                $("#dateofDeath").removeAttr('disabled');
                $("#admittedDate").removeAttr('disabled');
            }
            else {

            }
        },
        error: function () {

        }
    });
}

function CalculateAge(DOB) {
    alert(DOB);

}

