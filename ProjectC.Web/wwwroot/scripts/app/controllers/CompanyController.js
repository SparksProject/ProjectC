angular.module('SparksXApp').controller('CompanyController', ['$rootScope', '$state', '$stateParams', '$scope', 'SparksXService', function ($rootScope, $state, $stateParams, $scope, SparksXService) {

    $scope.$on('$viewContentLoaded', function () {
        App.initAjax();
        $rootScope.settings.layout.pageContentWhite = true;
        $rootScope.settings.layout.pageBodySolid = false;
    });

    $scope.Edit = function (obj) {
        if (obj.RecordStatusId === 1) {
            obj.ModifiedBy = $rootScope.user.UserId;
        }
        else {
            obj.DeletedBy = $rootScope.user.UserId;
        }
        SparksXService.EditCompany(obj).success(function (data) {
            $state.go('company/get', { id: data });
        });
    };

    $scope.Get = function () {
        SparksXService.GetCompany($stateParams.id).success(function (obj) {
            $scope.object = obj;
        });
    };

    $scope.BindEditFields = function () {
        $rootScope.settings.layout.pageSidebarClosed = false;
        SparksXService.GetCompany($stateParams.id).success(function (obj) {
            $scope.object = obj;

            SparksXService.GetRecordStatuses().success(function (data) {
                $scope.recordStatuses = data;
            });
        });
    };

}]);
