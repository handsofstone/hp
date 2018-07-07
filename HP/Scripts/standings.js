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
    '<td class="text-center">{{Rank}}</td>' +
    '<td><a href="/Team/Index/{{TeamId}}">{{Name}}</a></td>' +
    '<td class="text-right">{{Gain}}</td>' +
    '<td class="text-right">{{Total}}</td>' +
    '</tr>';
    return row.compose(obj);
}

function getPoolId() {
    var id = $('#SelectedPoolId').val()
    if (id == undefined) {
        var url = window.location.pathname;
        id = url.substring(url.lastIndexOf('/') + 1);
    }
    return id;
}

$(document).ready(function () {

    //Dropdownlist Selectedchange event
    $("#SelectedSeasonID").change(function () {
        $.ajax({
            contentType: 'application/json, charset=utf-8',
            type: 'GET',
            url: '/Pool/StandingRows', // we are calling json method
            dataType: 'json',
            data: { poolId: getPoolId(), seasonId: $("#SelectedSeasonID").val() },
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
    tradeDashboard();
});

function tradeDashboard() {
    $.ajax({
        type: 'GET',
        url: '/Team/TradeDashboard', // we are calling json method
        dataType: 'json',
        //data: { teamId: $('#TeamId').val() },
        success: function (data) {
            if (typeof data.Trades != 'undefined') {
                offers(data.Trades);
            }
        },
        error: function (ex) {
            alert('Failed to retrieve Trade Dashboard.' + ex);
        }
    });
}

function offers(trades) {
    var r = new Array(), j = -1;

    for (var i = 0, size = trades.length; i < size; i++) {
        r[++j] = '<tr id="trade';
        r[++j] = trades[i].Id;
        r[++j] = '"><td class="text-nowrap">';
        r[++j] = trades[i].StatusDate;
        r[++j] = '</td><td>';
        r[++j] = trades[i].From;
        r[++j] = '<input class="fromTeamId" type="hidden" value="';
        r[++j] = trades[i].FromTeamId;
        r[++j] = '"/></td><td><ul class="list-group">';
        if (trades[i].Receiving != undefined) {
            for (var i2 = 0, receivingSize = trades[i].Receiving.length; i2 < receivingSize; i2++) {
                r[++j] = '<li class="list-group-item text-nowrap mx-1">';
                r[++j] = trades[i].Receiving[i2].AssetName;
                r[++j] = '</li>';
            }
        }
        r[++j] = '</ul></td><td>';
        r[++j] = trades[i].To;
        r[++j] = '<input class="toTeamId" type="hidden" value="';
        r[++j] = trades[i].ToTeamId;
        r[++j] = '"/></td><td><ul class="list-group">';
        if (trades[i].Sending != undefined) {
            for (var i2 = 0, sendingSize = trades[i].Sending.length; i2 < sendingSize; i2++) {
                r[++j] = '<li class="list-group-item text-nowrap mx-1">';
                r[++j] = trades[i].Sending[i2].AssetName;
                r[++j] = '</li>';
            }
        }
        r[++j] = '</ul></td><td>'
        r[++j] = trades[i].Comments;
        r[++j] = '</td ></tr > ';
    }
    $("#tradesTable").append(r.join(''));
}