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

    $("#DraftSeasonID").change(function () {
        draftDashboard();
    });

    $("#SelectedSeasonID").trigger('change');
    tradeDashboard();

    $('[data-toggle="tooltip"]').tooltip();

    rosters();

    draftDashboard();

});

function rosters() {
    var asset_tmpl = doT.template($('#tmpl_asset').text());
    var roster_tmpl = doT.template($('#tmpl_rosters').text());

    $.ajax({
        type: 'GET',
        url: '/Pool/Assets', // we are calling json method
        dataType: 'json',
        data: { poolId: getPoolId() },
        success: function (data) {
            $('#rosters').html(roster_tmpl(data));
        },
        complete: function () {
            $('[data-toggle="tooltip"]').tooltip();
        },
        error: function (ex) {
            alert('Failed to retrieve Pool Rosters.' + ex);
        }
    });
}

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

function draftDashboard() {
    //var order_tmpl = doT.template($('#tmpl_order').text());
    var picks_tmpl = doT.template($('#tmpl_picks').text());

    $.ajax({
        type: 'GET',
        url: '/Pool/DraftDashboard', // we are calling json method
        dataType: 'json',
        data: { poolId: getPoolId(), seasonId: $("#DraftSeasonID").val() },
        success: function (data) {
            //$('#orders').html(picks_tmpl(data.PickOrder));
            $('#picks').html(picks_tmpl(data.DraftPicks));
            addAutoComplete();
        },
        error: function (ex) {
            alert('Failed to retrieve Draft Dashboard.' + ex);
        },
        complete: function (data) {
        
}
    });


        
}

function addAutoComplete() {
    $(".nhlsearch").autocomplete({
        source: function (request, response) {
            $.ajax({
                type: 'GET',
                contentType: 'application/json, charset=utf-8',
                url: '/Pool/AvailablePlayer',
                dataType: 'json',
                data: { searchString: request.term, poolId: getPoolId() },
                success: function (data) {
                    response(labelAndValues(data));
                }
            });
        },
        select: function (event, ui) {
            var pick_tmpl = doT.template($('#tmpl_pickDisplay').text());

            $(this).collapse('toggle');
            var pickDisp = $(this).closest("tr").find("td div"); // get the pick row
            pickDisp.find('label').html(pick_tmpl(ui.item.value)); // update the player
            pickDisp.collapse('toggle'); // show the button
        }
    });

    // add event handler for edit buttons
    $(".editpick").click(function () {
        // remove player
        // clear selection
        var pickRow = $(this).closest("tr"); // get the pick row
        var pickSelect = pickRow.find("td input");
        pickSelect.val('');
    });
}

function editPick() {

}
function draftPlayer(pickId, playerId) {

}

function labelAndValues(data) {
    return $.map(data, function (item) {
        var player = new Player(item.Player);
        return {
            label: player.LastName + ", " + player.FirstName,
            value: player
        };
    });
}

function Player(delimitedString) {
    // Format of | delimted search result
    //'PlayerId', 'LastName', 'FirstName', 'Active', 'Rookie', 'Height', 'Weight', 'City', 'State', 'Country', 'BirthDate', 'TeamCode', 'Position', 'PlayerNo', 'Link'
    var values = delimitedString.split('|');
    this.PlayerId = values[0];
    this.LastName = values[1];
    this.FirstName = values[2];
    this.Active = values[3];
    this.Rookie = values[4];
    this.Height = values[5];
    this.Weight = values[6];
    this.City = values[7];
    this.State = values[8];
    this.Country = values[9];
    this.BirthDate = values[10];
    this.TeamCode = values[11];
    this.Position = values[12];
    this.PlayerNo = values[13];
    this.Link = values[14];
}

function convertSearchAssets(searchResult) {
    var assets = [];
    for (var i = 0, size = searchResult.length; i < size; i++) {
        var player = new Player(searchResult[i].Player);
            assets.push({
                data: player,
                PlayerId: player.PlayerId,
                AssetName: player.FirstName + ' ' + player.LastName + ' ' + player.TeamCode
            });
    }
    return assets;
}