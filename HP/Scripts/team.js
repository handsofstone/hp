﻿
// Javascript for Add Players
function moveDualList(srcList, destList, moveAll) {

    var keepers = false;
    if (srcList.id == "roster") {
        var pText = srcList.options[srcList.selectedIndex].innerHTML;
        if (pText.indexOf("[K]") > -1) {
            keepers = true;
        }
    }

    // Do nothing if nothing is selected
    if ((srcList.selectedIndex == -1) && (moveAll === false)) {
        return;
    } else if (keepers) {
        return;
    } else {
        copySelected(srcList, destList);
        //sortSelect(destList);
    }
}

function deleteOption(object, index) {
    object.options[index] = null;
}

function addOption(object, text, value) {
    var defaultSelected = true;
    var selected = true;
    var optionName = new Option(text, value, defaultSelected, selected);
    object.options[object.length] = optionName;
}

function copySelected(fromObject, toObject) {
    for (var i = 0, l = fromObject.options.length; i < l; i++) {
        if (fromObject.options[i].selected)
            addOption(toObject, fromObject.options[i].text, fromObject.options[i].value);
    }
    for (i = fromObject.options.length - 1; i > -1; i--) {
        if (fromObject.options[i].selected)
            deleteOption(fromObject, i);
    }
}
function populatePlayerIds() {
    playerIds = "";
    submittedPlayerIds = document.getElementById("roster");
    numberOfPlayers = submittedPlayerIds.length;
    var comma = '';
    for (var i = 0; i <= numberOfPlayers - 1; i++) {
        playerIds = playerIds + comma + submittedPlayerIds[i].value;
        comma = ',';
    }
    document.getElementById("playerIds").value = playerIds;
}

function selectAll() {

    objList = document.getElementById('roster');

    for (x = 0; x < objList.options.length; x++) {
        objList.options[x].selected = true;
    }

    document.getElementById('frmAddPlayers').submit();

}

function player(e) {
    this.active = $('.playerActive', e).is(':checked');
    this.lineupPlayerId = $('.lineupPlayerId', e).val();
    this.playerId = $('#playerId', e).val();
    this.position = $('#position,.position', e).val();
}

function getPlayers(id) {
    return $(id + ' tbody tr').map(function (i, e) {
        return new player(e);
    });
}

function getModel(id) {
    this.IntervalId = $('#SelectedIntervalId').val();
    this.TeamId = $('#TeamId').val();
    this.Players = getPlayers(id).toArray();
}


function submitLineup() {
    waiting(true);
    $.ajax({
        contentType: 'application/json, charset=utf-8',
        type: 'POST',
        url: '/Team/SubmitLineup', // we are calling json method
        dataType: 'json',
        data: JSON.stringify({ model: new getModel('#lineupTable') }),
        success: function (data) {
            lineupRow(data.Lineup);
        },
        error: function (ex) {
            alert('Failed to retrieve states.' + ex);
        },
        complete: function () {
            postLineupUpdate();
            waiting(false);
        }
    });
}

function resetLineup() {
    waiting(true);
    $.ajax({
        type: 'GET',
        url: '/Team/ResetLineup', // we are calling json method
        dataType: 'json',
        data: { teamId: $('#TeamId').val(), intervalId: $("#SelectedIntervalId").val() },
        success: function (data) {
            lineupRow(data.Lineup);
        },
        error: function (ex) {
            alert('Failed to reset lineup.' + ex);
        },
        complete: function (ex) {
            postLineupUpdate();
            waiting(false);
        }
    });
}

function waiting(data) {
    if (data) {
        $("body").css("cursor", "progress");
        enableButtons(!data);
    } else {
        $("body").css("cursor", "default");
    }

}

function enableButtons(data) {
    $('#lineupSubmit').prop('disabled', !data);
    $('#lineupReset').prop('disabled', !data);
    $('.playerActive').bootstrapToggle(data ? 'enable' : 'disable');
}

function showButtons(data) {
    if (data) {
        $('#lineupSubmit').show();
        $('#lineupReset').show();
    } else {
        $('#lineupSubmit').hide();
        $('#lineupReset').hide()
    }
}

function checkButtons() {
    $.ajax({
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        url: '/Team/EnableSubmit', // we are calling json method
        data: { teamId: $('#TeamId').val(), intervalId: $("#SelectedIntervalId").val() },
        dataType: 'json',
        success: function (data) {
            enableButtons(data);
            showButtons(data);
            $('th.submit-show, td.submit-show').toggleClass("hidden-xs", data);
            $('th.submit-hide, td.submit-hide').toggleClass("hidden-xs", !data);
        },
        error: function (ex) {
            alert('Failed to enable/disable submit.' + ex);
        }
    });
}
function saveRoster() {
    $.ajax({
        contentType: 'application/json, charset=utf-8',
        type: 'POST',
        url: '/Team/SaveRoster', // we are calling json method
        dataType: 'json',
        data: JSON.stringify({ model: new getModel('#rosterTable') }),
        success: function () {
            alert('success');
        },
        error: function (ex) {
            alert('Failed to retrieve states.' + ex);
        }
    });
    refreshLinup();
}
function setupPlots() {
    plotLE = $.plot($('#efficiency'), [[]], {
        series: {
            pie: {
                show: true,
                radius: 1,
                innerRadius: 0.7, label: {
                    show: true,
                    radius: 0.85,
                    formatter: labelFormatter2,
                    background: {
                        opacity: 0.5
                    }
                }
            }
        }
    });
    plotPD = $.plot('#distribution', [[]], {
        series: {
            pie: {
                show: true,
                radius: 1,
                label: {
                    show: true,
                    radius: 0.7,
                    formatter: labelFormatter,
                    background: {
                        opacity: 0.5
                    }
                }
            }
        },
        legend: {
            show: false
        }
    });
}

function labelFormatter(label, series) {
    return "<div style='font-size:8pt; text-align:center; padding:2px; color:white;'>" + label + "<br/>" + Math.round(series.percent) + "%</div>";
}
function labelFormatter2(label, series) {
    return "<div style='font-size:8pt; text-align:center; padding:2px; color:white;'>" + series.data[0][1] + "</div>";
}

function lineupAnalysis(data) {    
    if (data.ActivePoints != null) {
        plotLE.setData([{ 'data': data.ActivePoints, color: '#5bc0de' }, { 'data': data.MaxPoints - data.ActivePoints, color: '#ddd' }]);
        $('#efficiency-label').html(Math.round(data.ActivePoints / data.MaxPoints * 100));
    }
    else
    {
        plotLE.setData([]);
        $('#efficiency-label').html('N/A');
    }
    
    if (data.Distribution) 
        plotPD.setData(data.Distribution);
    else 
        plotPD.setData([]);
    plotLE.draw();
    plotPD.draw();
}
function refreshLineup() {
    $.ajax({
        type: 'GET',
        url: '/Team/Lineup', // we are calling json method
        dataType: 'json',
        data: { teamId: $('#TeamId').val(), intervalId: $("#SelectedIntervalId").val() },
        success: function (data) {
            $('#intervalStart').text(data.IntervalStartTime.replace(/\"/g, ""));
            lineupRow(data.Lineup);
            postLineupUpdate();
            enableButtons(data.CanSubmitLineup);
            showButtons(data.CanSubmitLineup);
            $('th.submit-show, td.submit-show').toggleClass("hidden-xs", data.CanSubmitLineup);
            $('th.submit-hide, td.submit-hide').toggleClass("hidden-xs", !data.CanSubmitLineup);
            lineupAnalysis(data);
        },
        error: function (ex) {
            alert('Failed to retrieve lineup.' + ex);
        },
        complete: function () {
            
            waiting(false);
        }
    });
}

function validateLineup() {
    var lineupFormat = { C: 2, R: 2, L: 2, D: 4, G: 1 };
    var players = getPlayers('#lineupTable');

    $.each(lineupFormat, function (k, v) {
        var activeCount = $("#lineupTable .playerRow:has(:checked):has(td .position[value=" + k + "])").length;
        var benchToggles = $("#lineupTable .playerRow:not(:has(:checked)):has(td .position[value=" + k + "]) .playerActive");
        var benchRows = $("#lineupTable .playerRow:not(:has(:checked)):has(td .position[value=" + k + "])");
        var showBench = $("#showBench").prop('checked');
        if (activeCount >= v)
        {
            benchToggles.bootstrapToggle('disable')
            benchRows.toggle(showBench)
        }
        else
        {
            benchToggles.bootstrapToggle('enable')
            benchRows.toggle(true)
        }
    });
}

function lineupRow(rows) {
    var r = new Array(), j = -1;
    for (var i = 0, size = rows.length; i < size; i++) {
        r[++j] = '<tr class="playerRow" id=';
        r[++j] = rows[i].PlayerId;
        r[++j] = '><td class="text-center"><input ';
        r[++j] = rows[i].Active ? "checked" : "";
        r[++j] = ' class="playerActive" data-toggle="toggle" data-size="mini" type="checkbox" value="true" data-width="90%" data-height="22px"/>';
        r[++j] = '<input class="lineupPlayerId" type="hidden" value="';
        r[++j] = rows[i].LineupPlayerId;
        r[++j] = '"/></td><td class="text-center">';
        r[++j] = rows[i].Position;
        r[++j] = '<input class="position" type="hidden" value="';
        r[++j] = rows[i].Position;
        r[++j] = '"/></td><td><span data-toggle="tooltip" data-placement="bottom" title="'
        r[++j] = rows[i].Number;
        r[++j] = ', ';
        r[++j] = rows[i].Team;
        r[++j] = '">';
        r[++j] = '<a target="_blank" href="https://www.nhl.com/player/';
        r[++j] = rows[i].PlayerId;
        r[++j] = '">';
        r[++j] = rows[i].Name;
        r[++j] = '</a>';
        r[++j] = '</span><input data-val="true" id="playerId" name="playerId" type="hidden" value="';
        r[++j] = rows[i].PlayerId;
        r[++j] = '"/></td><td class="text-center submit-show">'
        r[++j] = rows[i].GP;
        r[++j] = '</td><td class="text-right submit-show">';
        r[++j] = '<span data-toggle="tooltip" data-placement="bottom" title="';
        r[++j] = rows[i].Points.D.Description;
        r[++j] = '">';
        r[++j] = rows[i].Points.D.Value;
        r[++j] = '</span></td><td class="text-right submit-show">';
        r[++j] = '<span data-toggle="tooltip" data-placement="bottom" title="';
        r[++j] = rows[i].Points.I.Description;
        r[++j] = '">';
        r[++j] = rows[i].Points.I.Value;
        r[++j] = '</span></td><td class="text-right submit-show">';
        r[++j] = '<span data-toggle="tooltip" data-placement="bottom" title="';
        r[++j] = rows[i].Points.T.Description;
        r[++j] = '">';
        r[++j] = rows[i].Points.T.Value;
        r[++j] = '</span></td><td class="submit-hide hidden-xs">';
        r[++j] = ScheduleCell(rows[i].Games);
        r[++j] = '</td></tr>';
    }
    $('#lineup').html(r.join(''));


    //$('#lineupTable').DataTable();
}
function ScheduleCell(games) {
    var r = new Array(), j = -1;
    r[++j] = '<ul class="list-inline">';
    for (var i = 0, size = games.length; i < size; i++) {
        var gameDate = new Date(games[i].StartDate);
        r[++j] = '<li><span data-toggle="tooltip" data-placement="bottom" title="';
        r[++j] = gameDate.toDateString();
        r[++j] = '"><a  target="_blank" href="https://www.nhl.com/gamecenter/';
        r[++j] = games[i].GameId;
        r[++j] = '">'
        r[++j] = games[i].OpponentTeamCode;
        r[++j] = '</a></span></li>';
    }
    r[++j] = '</ul>';
    return r.join('');
}
function postLineupUpdate() {
    $("input.playerActive").change(function (event) {
        setSubmitted(false);
        validateLineup();
    });

    $('[data-toggle="tooltip"]').tooltip();
    $('[data-toggle="toggle"]').bootstrapToggle({
        on: "Active",
        off: "Bench",

    });

    validateLineup();
    setSubmitted();
}
function setSubmitted(flag) {
    flag = flag || $('.lineupPlayerId').first().val() != "";
    $('#submitted').toggle(flag);
    $('#unsubmitted').toggle(!flag);
}

var plotLE;
var plotPD;

function DraftPicks(picks) {
    var r = new Array(), j = -1;
    for (var i = 0, size = picks.length; i < size; i++) {        
        r[++j] = '<tr><td>';
        r[++j] = picks[i].Pick;
        r[++j] = '</td></tr>';
    }
    $('#picks').html(r.join(''));
}

function rosterDashboard() {
    $.ajax({
        type: 'GET',
        url: '/Team/RosterDashboard', // we are calling json method
        dataType: 'json',
        data: { teamId: $('#TeamId').val() },
        success: function (data) {
            DraftPicks(data.Picks)
        },
        error: function (ex) {
            alert('Failed to retrieve Roster Dashboard.' + ex);
        },
        complete: function () {
        }
    });
}

$(function () {
    $("#datepicker").datepicker();
});

$(function () {
    $('#myAssets').on('click', 'li', function () {
        $('#myAssetsOffered').append($(this));
    });
    $('#myAssetsOffered').on('click', 'li', function () {
        $('#myAssets').append($(this));
    });

    $('#partnerAssets').on('click', 'li', function () {
        $('#partnerAssetsRequested').append($(this));
    });
    $('#partnerAssetsRequested').on('click', 'li', function () {
        $('#partnerAssets').append($(this));
    });

});

$('#offersTable').on('click', '.clickable-row', function (event) {
    if ($(this).hasClass('active')) {
        $(this).removeClass('active');
    } else {
        $(this).addClass('active').siblings().removeClass('active');
    }

    var noneSelected = $('#offersTable tr.active').length == 0;
    var ownTradeSelected = $('#offersTable tr.active .fromTeamId').val() == $('#TeamId').val();    

    $('#rejectTrade').prop('disabled', noneSelected);
    $('#acceptTrade').prop('disabled', noneSelected || ownTradeSelected);
});

function tradeDashboard() {
    $.ajax({
        type: 'GET',
        url: '/Team/TradeDashboard', // we are calling json method
        dataType: 'json',
        data: { teamId: $('#TeamId').val() },
        success: function (data) {
            if (typeof data.Trades != 'undefined') {
                offers(data.Trades);
                $('#tradeCount, #trade span.badge').html(data.Trades.length);
            }
            else
                $('#tradeCount, #trade span.badge').html('');
            if (typeof data.Teams != 'undefined') partners(data.Teams);
            if (typeof data.TradableAssets != 'undefined') assets($('#myAssets'),data.TradableAssets);
        },
        error: function (ex) {
            alert('Failed to retrieve Roster Dashboard.' + ex);
        },
        complete: function () {
        }
    });
}
function offers(trades)
{
    var r = new Array(), j = -1;

    for (var i = 0, size = trades.length; i < size; i++) {
        r[++j] = '<tr id="trade';
        r[++j] = trades[i].Id;
        r[++j] = '" class="clickable-row offers-row" > <td>';
        r[++j] = trades[i].From;
        r[++j] = '<input class="fromTeamId" type="hidden" value="';
        r[++j] = trades[i].FromTeamId;
        r[++j] = '"/></td><td>';
        r[++j] = trades[i].To;
        r[++j] = '<input class="toTeamId" type="hidden" value="';
        r[++j] = trades[i].ToTeamId;
        r[++j] = '"/></td><td>';
        r[++j] = trades[i].ExpirationDate;
        r[++j] = '</td><td><ul class="list-group">';
        for (var i2 = 0, sendingSize = trades[i].Sending.length; i2 < sendingSize; i2++) {
            r[++j] = '<li class="list-group-item">';
            r[++j] = trades[i].Sending[i2].AssetName;
            r[++j] = '</li>';
        }
        r[++j] = '</ul></td><td><ul class="list-group">';
        for (var i2 = 0, sendingSize = trades[i].Receiving.length; i2 < sendingSize; i2++) {
            r[++j] = '<li class="list-group-item">';
            r[++j] = trades[i].Receiving[i2].AssetName;
            r[++j] = '</li>';
        } 
        r[++j] = '</ul></td><td>';
        r[++j] = trades[i].Status;
        r[++j] = '</td><td></td></tr>';
    }
    $("#offersTable").append(r.join(''));
}

function partners(teams) {
    var r = new Array(), j = -1;
    for (var i = 0, size = teams.length; i < size; i++) {
        r[++j] = '<option value="';
        r[++j] = teams[i].Id;
        r[++j] = '">'
        r[++j] = teams[i].Name;
        r[++j] = '</option>';
    }
    $('#selectTradePartner').html(r.join(''));
}

function assets(e, assets) {
    var r = new Array(), j = -1;
    for (var i = 0, size = assets.length; i < size; i++) {
        r[++j] = '<li class="drag ui-state-default" value=';
        r[++j] = assets[i].Id;
        r[++j] = '>';
        r[++j] = assets[i].AssetName;
        r[++j] = '</li>';
    }
    e.html(r.join(''));
}

function getAssets(e) {
    var a = new Array();
    e.children().each(function () {
        a.push($(this).val());
    })
    return a;
}

function refreshPartnerAssets() {
    $('#partnerAssetsRequested').empty();
    $.ajax({
        type: 'GET',
        url: '/Team/Assets', // we are calling json method
        dataType: 'json',
        data: { teamId: $('#selectTradePartner').val() },
        success: function (data) {
            assets($('#partnerAssets'),data)
        },
        error: function (ex) {
            alert('Failed to retrieve partner assets.' + ex);
        },
        complete: function () {
            waiting(false);
        }
    });
}

function getOffer() {
    this.fromTeamId = $('#TeamId').val();
    this.toTeamId = $('#selectTradePartner').val();
    this.Offering = getAssets($('#myAssetsOffered'));
    this.Requesting = getAssets($('#partnerAssetsRequested'));
}

function sendOffer() {
    var data = { json: JSON.stringify({ offer: new getOffer() }) };
    $.ajax({
        //contentType: 'application/json, charset=utf-8',
        type: 'POST',
        url: '/Team/SendOffer',
        dataType: 'json',
        data:  data,
        success: function (data) {
            alert('Trade submitted.');
            resetTradeDashboard();
        },
        error: function (ex) {

        }
    });
}

function updateOffer(isAccepted) {
    var data = {
        tradeId: $('[id^=trade] tr.active').attr('id').replace('trade', ''),
        accept: isAccepted
    };
    $.ajax({
        //contentType: 'application/json, charset=utf-8',
        type: 'POST',
        url: '/Team/UpdateOffer',
        dataType: 'json',
        data: data,
        success: function (data) {
            alert('Trade accepted.');
            resetTradeDashboard();
        },
        error: function (ex) {

        }
    });
}

function resetTradeDashboard()
{
    $('#myAssetsOffered').empty();
    $('#partnerAssetsRequested').empty();
    $('#offersTable tbody tr').remove();
    tradeDashboard();
}

$(document).ready(function () {

    $("#progressbar").progressbar({ value: false });

    $(document)
    .ajaxStart(function () {
        $('#lineupTable').hide();
        $("#progressbar").show();
    })
    .ajaxStop(function () {
        $("#progressbar").hide();
        $('#lineupTable').show();
    });

    setupPlots();

    refreshLineup();

    //Dropdownlist Selectedchange event
    $("#SelectedIntervalId").change(function () {
        waiting(true);
        refreshLineup();
        return false;
    });

    $("#showBench").change(function () {
        validateLineup();
    });

    $("#selectTradePartner").change(function () {
        refreshPartnerAssets();
    });

    rosterDashboard();
    tradeDashboard();

});