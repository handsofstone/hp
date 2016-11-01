String.prototype.compose = (function () {
    var re = /\{{(.+?)\}}/g;
    return function (o) {
        return this.replace(re, function (_, k) {
            return typeof o[k] != 'undefined' ? o[k] : '';
        });
    }
}());

function populateStandings(standings) {
    $('#standingsTable').children('tbody').empty();
    $.each(standings, function (i, standing) {
        $('#standingsTable').children('tbody').append(standingsRow(standing));
    });
}

function standingsRow(obj) {
    var row = '<tr>' +
    '<td>{{Rank}}</td>' +
    '<td>{{Name}}</td>' +
    '<td>{{Gain}}</td>' +
    '<td>{{Total}}</td>' +
    '</tr>';
    return row.compose(obj);
}
$(document).ready(function () {
    //Dropdownlist Selectedchange event
    $("#SelectedSeasonID").change(function () {
        $.ajax({
            contentType: 'application/json, charset=utf-8',
            type: 'GET',
            url: '/Pool/StandingRows', // we are calling json method
            dataType: 'json',
            data: { poolId: $('#SelectedPoolId').val(), seasonId: $("#SelectedSeasonID").val() },
            success: function (standings) {
                populateStandings(standings);
            },
            error: function (ex) {
                alert('Failed to retrieve start time.' + ex);
            }
        });
        return false;
    });

    $("#SelectedSeasonID").trigger('change');

});