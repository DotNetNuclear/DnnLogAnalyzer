var dnnuclear = dnnuclear || {};

(function (dnnuclear) {
    /*
    *  Log Analyzer SignalR hub definition
    */
    dnnuclear.LogAnalyzerHub = $.connection.logAnalyzerHub;
    dnnuclear.LogAnalyzerHub.client = {
        progress: function (procId, i) {
            dnnuclear.LogAnalyzer.updateProgress(i);
        },
        procStart: function (procId, id) {
            dnnuclear.LogAnalyzer.updateProgress(0);
            dnnuclear.LogAnalyzer.progressBar.show();
            console.log(id);
        },
        procComplete: function (procId) {
            dnnuclear.LogAnalyzer.updateProgress(100);
            setTimeout(function () {
                //dnnuclear.LogAnalyzer.updateProgress(0);
                dnnuclear.LogAnalyzer.progressBar.hide();
            }, 2000);
        }
    };

    /*
    *  Log Analyzer options definition & defaults
    */
    dnnuclear.LogAnalyzerDefOptions = {
        moduleId: -1,
        koContainer: $(".dnnuclear-logAnalyzer:first"),
        progressBarSelector: "#logAnalyzerProgress",
        logFileList: []
    };

    /*
    *  Log Analyzer Controller
    */
    dnnuclear.LogAnalyzer = {
        hubInitialized: false,
        hub: dnnuclear.LogAnalyzerHub,
        connectionId: -1,
        progressBar: null,
        options: null,
        serviceFW: null,
        serviceRoot: null,

        init: function (options) {
            this.options = $.extend(dnnuclear.LogAnalyzerDefOptions, options);

            this.serviceFW = $.ServicesFramework(this.options.moduleId);
            this.serviceRoot = this.serviceFW.getServiceRoot("dotnetnuclear.loganalyzer");
            this.progressBar = $(this.options.progressBarSelector);

            this.hub.state.moduleid = this.options.moduleId;
            this.hub.state.userid = -1;
            dnnuclear.LaRequestViewModel.logFiles = this.options.logFileList;

            ko.applyBindings(dnnuclear.LaRequestViewModel, $(this.options.koContainer).get(0));

            $.connection.hub.start().done(function () {
                dnnuclear.LogAnalyzer.hub.connectionId = $.connection.hub.id;
                dnnuclear.LogAnalyzer.hubInitialized = true;
            });
        },
        updateProgress: function(val) {
            var progBar = this.progressBar.find(".progress-bar");
            if (val >= 0) {
                progBar.css('width', val + '%').attr('aria-valuenow', val).text(val + '%');
            }
        },
        analyzeLogs: function () {
            var req = {
                "taskId": new Date().getTime().toString(),
                "files": dnnuclear.LaRequestViewModel.selectedLogs()
            };
            jQuery.ajax({
                type: "POST",
                url: this.serviceRoot + "logsvc/analyze",
                beforeSend: this.serviceFW.setModuleHeaders,
                data: req,
                dataType: "json"
            }).done(function (response) {
                dnnuclear.LogAnalyzer.displayReport(response);
            }).fail(function (xhr, result, error) {
                console.log("error: " + error);
            });
        },
        displayReport: function (data) {
            alert('show report');
        }
    };

    /*
    *  Knockout view models
    */
    dnnuclear.LaRequestViewModel = {
        logFiles: [],
        selectedLogs: ko.observableArray(),
        formatLogName: function(logFile) {
            return logFile.Name + " (" + logFile.FileSize + ")";
        },
        startAnalyzer: function () {
            dnnuclear.LogAnalyzer.analyzeLogs();
        }
    };

    dnnuclear.LaResultsViewModel = {
        Results: ko.observableArray()
    };

}(dnnuclear));