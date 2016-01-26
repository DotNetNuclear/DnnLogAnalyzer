var dnnuclear = dnnuclear || {};

(function (dnnuclear) {
    /*
    *  Log Analyzer SignalR hub definition
    */
    dnnuclear.LogAnalyzerHub = $.connection.logAnalyzerHub;
    dnnuclear.LogAnalyzerHub.client = {
        progress: function (procId, i, err) {
            dnnuclear.LogAnalyzer.updateProgress(i, err);
        },
        procStart: function (procId, id) {
            dnnuclear.LogAnalyzer.updateProgress(0);
            dnnuclear.LogAnalyzer.progressBar.show();
            console.log(id);
        },
        procComplete: function (procId) {
            dnnuclear.LogAnalyzer.updateProgress(100);
            setTimeout(function () {
                dnnuclear.LogAnalyzer.progressBar.hide();
            }, 1000);
        }
    };

    /*
    *  Log Analyzer options definition & defaults
    */
    dnnuclear.LogAnalyzerDefOptions = {
        moduleId: -1,
        koContainer: $(".dnnuclear-logAnalyzer:first"),
        messageContainer: $(".dnnFormMessage:first"),
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
            dnnuclear.LogAnalyzerViewModel.logFiles = this.options.logFileList;

            ko.applyBindings(dnnuclear.LogAnalyzerViewModel, $(this.options.koContainer).get(0));

            $.connection.hub.start().done(function () {
                dnnuclear.LogAnalyzer.hub.connectionId = $.connection.hub.id;
                dnnuclear.LogAnalyzer.hubInitialized = true;
            });
        },
        updateProgress: function(val, errMsg) {
            var progBar = this.progressBar.find(".progress-bar");
            if (errMsg && errMsg.length > 0) {
                $(this.options.messageContainer).text(errMsg).show();
            }
            if (val >= 0) {
                progBar.css('width', val + '%').attr('aria-valuenow', val).text(val + '%');
            }
        },
        analyzeLogs: function () {
            var req = {
                "taskId": new Date().getTime().toString(),
                "files": dnnuclear.LogAnalyzerViewModel.selectedLogs()
            };
            jQuery.ajax({
                type: "POST",
                url: this.serviceRoot + "logsvc/analyze",
                beforeSend: this.serviceFW.setModuleHeaders,
                data: req,
                dataType: "json"
            }).done(function (response) {
                if (response && response.ReportedItems) {
                    dnnuclear.LogAnalyzer.displayReport(response.ReportedItems);
                }
            }).fail(function (xhr, result, error) {
                console.log("error: " + error);
            });
        },
        displayReport: function (data) {
            dnnuclear.LogAnalyzerViewModel.analyzedResults(data);
        }
    };

    /*
    *  Knockout view models
    */
    dnnuclear.LogAnalyzerViewModel = {
        logFiles: [],
        analyzedResults: ko.observableArray(),
        selectedLogs: ko.observableArray(),
        formatLogName: function(logFile) {
            return logFile.Name + " (" + logFile.FileSize + ")";
        },
        startAnalyzer: function () {
            dnnuclear.LogAnalyzer.analyzeLogs();
        },
        resetAnalyzer: function () {
            var self = dnnuclear.LogAnalyzerViewModel;
            self.selectedLogs.removeAll();
            self.analyzedResults.removeAll();
        },
        selectALog: function (item, event) {
            var self = dnnuclear.LogAnalyzerViewModel;
            if (self.selectedLogs.indexOf(item.Name) >= 0) {
                self.selectedLogs.remove(item.Name);
            } else {
                self.selectedLogs.push(item.Name);
            }
        },
        afterResultRender: function (elem) {
            $(elem).find('td.log-message span').readmore({
                speed: 125,
                collapsedHeight: 80
            });
        }
    };

}(dnnuclear));