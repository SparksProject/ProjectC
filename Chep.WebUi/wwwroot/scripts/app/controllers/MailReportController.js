angular.module('SparksXApp').controller('MailReportController', ['$rootScope', '$state', '$stateParams', '$scope', 'SparksXService', function ($rootScope, $state, $stateParams, $scope, SparksXService) {

    $scope.$on('$viewContentLoaded', function () {
        App.initAjax();
        $rootScope.settings.layout.pageContentWhite = true;
        $rootScope.settings.layout.pageBodySolid = false;
    });

    $scope.List = function () {
        SparksXService.ListMailReports().success(function (data) {
            $scope.list = data;
        });
    };

    $scope.Get = function () {
        SparksXService.GetMailReport($stateParams.id).success(function (data) {
            $scope.object = data;
        });
    };

    $scope.AddMailDefinition = function (obj) {
        SparksXService.AddMailDefinition(obj).success(function (data) {
            SparksXService.ListMailDefinitions().success(function (obj) {
                $scope.mailDefinitions = obj;
                $scope.object = null;
            });
        });
    };

    $scope.BindAddMailDefinitionFields = function () {
        $rootScope.settings.layout.pageSidebarClosed = false;
        SparksXService.ListMailDefinitions().success(function (obj) {
            $scope.mailDefinitions = obj;
        });
    };

    $scope.Add = function (obj) {
        obj.CreatedBy = $rootScope.user.UserId;

        SparksXService.AddMailReport(obj).success(function (data) {
            $state.go('mailreports/get', { id: data });
        });
    };

    $scope.Edit = function (obj) {
        if (obj.RecordStatusId === 1) {
            obj.ModifiedBy = $rootScope.user.UserId;
        }
        else {
            obj.DeletedBy = $rootScope.user.UserId;
        }
        SparksXService.EditMailReport(obj).success(function (data) {
            $state.go('mailreports/get', { id: data });
        });
    };

    $scope.BindAddFields = function () {
        $scope.object = {};
        $scope.object.BodyTemplate = "Merhaba #FullName#,";
        SparksXService.GetPeriodTypes().success(function (data) {
            $scope.periodTypes = data;
        });

        SparksXService.GetMailDefinitions().success(function (data) {
            $scope.mailDefinitions = data;
        });
    };

    $scope.BindEditFields = function () {
        SparksXService.GetMailReport($stateParams.id).success(function (data) {
            $scope.object = data;

            SparksXService.GetPeriodTypes().success(function (data) {
                $scope.periodTypes = data;
            });

            SparksXService.GetRecordStatuses().success(function (data) {
                $scope.recordStatuses = data;
            });

            SparksXService.GetMailDefinitions().success(function (data) {
                $scope.mailDefinitions = data;
            });
        });
    };

    $scope.GetMailResultSet = function () {
        $rootScope.settings.layout.pageSidebarClosed = true;

        var id = parseInt($stateParams.id);

        if (isNaN(id)) {
            $state.go('mailreports/list');
        }

        SparksXService.GetMailResultSet(id, $rootScope.user.UserId).success(function (data) {
            var response = data.Result;

            if (response != null && response != undefined && response.length > 0) {

                var headers = [];

                $.each(response[0], function (key, element) {
                    headers.push(element.Caption);
                });

                $.each(response, function (key, element) { // satırlar
                    $.each(element, function (i, prop) { // sütunlar
                        if (prop.DataType == 'DateTime' && prop.Value.indexOf('T00:00:00') > -1) {  // DateOnly tipinde ise
                            response[key][i].DataType = 'Date';
                        }
                    });
                });

                $scope.object = {
                    headers: headers,
                    result: response,
                };

                //$scope.$apply();
            }
            else if (data.Message != null && data.Message != undefined) { // warning dönünce
                setTimeout(function () { alert(data.Message); }, 100);
            }
        }).error(function (data) {
            console.log(data);
        });
    };

}]);
