
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
        success: function () {
            alert('success');
        },
        error: function (ex) {
            alert('Failed to retrieve states.' + ex);
        },
        complete: function () {
            waiting(false);
        }
    });
}

function resetLineup() {
    waiting(true);
    $.ajax({
        type: 'GET',
        url: '/Team/ResetLineup', // we are calling json method
        dataType: 'html',
        data: { teamId: $('#TeamId').val(), intervalId: $("#SelectedIntervalId").val() },
        success: function (players) {
            populatePlayers(players);
        },
        error: function (ex) {
            alert('Failed to retrieve lineup.' + ex);
        },
        complete: function (ex) {
            waiting(false);
        }
    });
}

function waiting(data) {
    if (data) {
        $("body").css("cursor", "progress");
        enableButtons(false);
    } else {
        checkButtons();
        $("body").css("cursor", "default");
    }

}

function enableButtons(data) {
    $('#lineupSubmit').prop('disabled', !data);
    $('#lineupReset').prop('disabled', !data);
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

$(document).ready(function () {
    checkButtons();

    //Dropdownlist Selectedchange event
    $("#SelectedIntervalId").change(function () {
        waiting(true);
        refreshLineup();
        $.ajax({
            type: 'GET',
            url: '/Team/IntervalStartTime', // we are calling json method
            dataType: 'html',
            data: { intervalId: $("#SelectedIntervalId").val() },
            success: function (date) {
                $('#intervalStart').text(date.replace(/\"/g, ""));
            },
            error: function (ex) {
                alert('Failed to retrieve start time.' + ex);
            }
        });
        return false;
    });

    $("input.playerActive").change(function (event) {
        validateLineup();
    });

    validateLineup();


});

function refreshLineup() {
    $('#IntervalRoster').empty();
    $.ajax({
        type: 'GET',
        url: '/Team/Lineup', // we are calling json method
        dataType: 'html',
        data: { teamId: $('#TeamId').val(), intervalId: $("#SelectedIntervalId").val() },
        success: function (players) {
            populatePlayers(players)
        },
        error: function (ex) {
            alert('Failed to retrieve lineup.' + ex);
        },
        complete: function () {
            waiting(false);
        }
    });
}

function populatePlayers(players) {
    $('#lineup').empty();
    $('#lineup').html(players)
    $("input.playerActive").bootstrapToggle();
    $("input.playerActive").change(function (event) {
        validateLineup();
    });
    validateLineup();

}
function validateLineup() {
    var lineupFormat = { C: 2, R: 2, L: 2, D: 4, G: 1 };
    var players = getPlayers('#lineupTable');

    $.each(lineupFormat, function (k, v) {
        var activeCount = $("#lineupTable .playerRow:has(:checked):has(td .position[value=" + k + "])").length;
        var benchToggles = $("#lineupTable .playerRow:not(:has(:checked)):has(td .position[value=" + k + "]) .playerActive");
        if (activeCount >= v)
            benchToggles.bootstrapToggle('disable')
        else
            benchToggles.bootstrapToggle('enable')
    });
}

function sortLineup() {

}
