var dnnuclear = dnnuclear || {};

(function (dnnuclear) {
    dnnuclear.laHub = $.connection.logAnalyzerHub;

    dnnuclear.laHub.state.moduleid = 0;
    dnnuclear.laHub.state.userid = -1;
    dnnuclear.connectionId = -1;

    $(document).ready(function () {
        dnnuclear.progress = $("#logAnalyzerProgress");
    });

    dnnuclear.updateProgress = function (val) {
        var progBar = dnnuclear.progress.find(".progress-bar");
        if (val >= 0) {
            progBar.css('width', val + '%').attr('aria-valuenow', val).text(val + '%');
        }
    }

    dnnuclear.analyze = function (method, moduleId, jsonIn, callback) {
        var SF = $.ServicesFramework(moduleId);
        var serviceBase = SF.getServiceRoot("dotnetnuclear.loganalyzer");
        jQuery.ajax({
            type: "POST",
            url: serviceBase + method,
            beforeSend: SF.setModuleHeaders,
            data: jsonIn,
            dataType: "json"
        }).done(function (response) {
            if (callback) { callback(response); }
        }).fail(function (xhr, result, error) {
            console.log("error: " + error);
        });
    };

    dnnuclear.laHub.client.progress = function (procId, i) {
        dnnuclear.updateProgress(i);
    };

    dnnuclear.laHub.client.procStart = function (procId, id) {
        dnnuclear.updateProgress(-1);
        dnnuclear.progress.show();
        console.log(id);
    };

    dnnuclear.laHub.client.procComplete = function (procId) {
        dnnuclear.updateProgress(100);
        setTimeout(function () {
            dnnuclear.progress.hide();
        }, 2000);
    };

    //dnnuclear.TaskHub.cancelDone = function (procId) {
    //    var $tr = $('#' + procId);
    //    $tr.find('td:nth-child(2)').find('div.progress').removeClass('active');
    //    $tr.find('td:nth-child(3)').text('Canceled');
    //    $tr.find('td:last').html('');
    //};

    //$('#btnRun').bind('click', function (e) {
    //    e.preventDefault();

    //    var $option = $('#createTask').find('option:selected');
    //    var value = parseInt($option.val(), 10);
    //    if (!isNaN(value)) {
    //        var trId = new Date().getTime();
    //        //Create Tr
    //        var $tr = $('<tr id="' + trId + '"><td>' + $option.text() + '</td><td><p>0%</p><div class="progress progress-striped active"><div class="bar" style="width: 0%;"></div></div></td><td>Running...</td><td><button class="btn btn-danger">Stop</button></td></tr>');
    //        $tr.appendTo('#tblTasks>tbody');

    //        taskHub.doLongProc(trId, $option.text());
    //    }
    //});

    //$('#tblTasks').on('click', 'button', function (e) {
    //    e.preventDefault();
    //    taskHub.cancelProc($(this).parent().parent().attr('id'));
    //});

    $.connection.hub.start().done(function () {
        dnnuclear.connectionId = $.connection.hub.id;
        $("#btnAnalyze").removeClass("disabled");
    });

//    //$.connection.hub.start();
}(dnnuclear));
