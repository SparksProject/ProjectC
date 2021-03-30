angular.module('SparksXApp').controller('CustomerController', ['$rootScope', '$state', '$stateParams', '$scope', 'SparksXService', function ($rootScope, $state, $stateParams, $scope, SparksXService) {

    $scope.$on('$viewContentLoaded', function () {
        App.initAjax();
        $rootScope.settings.layout.pageContentWhite = true;
        $rootScope.settings.layout.pageBodySolid = false;
    });


    $scope.List = function () {
        SparksXService.ListCustomers().success(function (data) {
            $scope.list = data;
        });
    };

    $scope.Get = function () {
        SparksXService.GetCustomer($stateParams.id).success(function (data) {
            $scope.object = data;
        });
    };

    $scope.Add = function (obj) {
        obj.CreatedBy = $rootScope.user.UserId;

        SparksXService.AddCustomer(obj).success(function (data) {
            $state.go('customer/get', { id: data });
        });
    };

    $scope.Edit = function (obj) {
        if (obj.RecordStatusId === 1) {
            obj.ModifiedBy = $rootScope.user.UserId;
        }
        else {
            obj.DeletedBy = $rootScope.user.UserId;
        }

        SparksXService.EditCustomer(obj).success(function (data) {
            $state.go('customers/get', { id: data });
        });
    };

    $scope.BindAddFields = function () {
        $scope.object = {};
        $scope.object.BodyTemplate = "Merhaba #FullName#, <br/>Raporunuz ektedir. <br/>İyi çalışmalar";
        SparksXService.GetPeriodTypes().success(function (data) {
            $scope.periodTypes = data;
        });

        SparksXService.GetMailDefinitions().success(function (data) {
            $scope.mailDefinitions = data;
        });
    };

    $scope.BindEditFields = function () {
        SparksXService.GetCustomer($stateParams.id).success(function (data) {
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

}]);