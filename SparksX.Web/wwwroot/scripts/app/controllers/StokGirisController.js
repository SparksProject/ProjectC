angular.module('SparksXApp').controller('StokGirisController', ['$rootScope', '$state', '$stateParams', '$scope', 'settings', 'SparksXService', '$timeout', 'Upload', function ($rootScope, $state, $stateParams, $scope, settings, SparksXService, $timeout, Upload) {

    $scope.$on('$viewContentLoaded', function () {
        App.initAjax();
        $rootScope.settings.layout.pageContentWhite = true;
        $rootScope.settings.layout.pageBodySolid = false;
        $rootScope.settings.layout.pageSidebarClosed = false;

    });


    // CRUD

    $scope.List = function () {
        $rootScope.settings.layout.pageSidebarClosed = false;
        SparksXService.List().success(function (data) {
            $scope.list = data;
        });
    }

    $scope.Get = function () {
        SparksXService.GetStokGiris($stateParams.id).success(function (data) {
            $scope.object = data;
        });
    };

    $scope.BindAddFields = function () {
        $scope.object = {};

        if ($rootScope.SelectedCustomerId == undefined) {
        } else {
            $scope.object.CustomerId = $rootScope.SelectedCustomerId;
            $scope.hasCustomerSelected = true;
        }

        SparksXService.GetCustomers().success(function (data) {
            $scope.customers = data;

            SparksXService.ListStokGiriss($scope.object.CustomerId).success(function (data) {
                $scope.list = data;
            });
        });
    };

    $scope.BindEditFields = function () {

        SparksXService.GetStokGiris($stateParams.id).success(function (obj) {
            $scope.object = obj;

            SparksXService.GetCustomers().success(function (data) {
                $scope.customers = data;
            });

            SparksXService.GetRecordStatuses().success(function (data) {
                $scope.recordStatuses = data;
            });
        });
    };

    $scope.Add = function (obj) {
        obj.CreatedBy = $rootScope.user.UserId;
        obj.RecordStatusId = 1;

        SparksXService.AddStokGiris(obj).success(function (data) {
            $state.go('products/list');
        });
    };

    $scope.Edit = function (obj) {
        if (obj.RecordStatusId === 1) {
            obj.ModifiedBy = $rootScope.user.UserId;
        }
        else {
            obj.DeletedBy = $rootScope.user.UserId;
        }
        SparksXService.EditStokGiris(obj).success(function (data) {
            $state.go('products/list');
        });
    };
}]);