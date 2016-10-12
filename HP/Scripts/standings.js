$(document).ready(function () {
    //Dropdownlist Selectedchange event
    $("#SelectedSeasonID").change(function () {
        $.ajax({
            contentType: 'application/json, charset=utf-8',
            type: 'GET',
            url: '/Team/StandingRows', // we are calling json method
            dataType: 'json',
            data: { poolId: $('#SelectedPoolId').val(), seasonId: $("#SelectedSeasonID").val() },
            success: function (standings) {
                $('#lineup').empty();
                $('#intervalStart').text(date.replace(/\"/g, ""));
            },
            error: function (ex) {
                alert('Failed to retrieve start time.' + ex);
            }
        });
        return false;
    });

});