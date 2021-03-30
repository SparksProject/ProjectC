angular.module('SparksXApp').controller('WorkOrderController', ['$rootScope', '$state', '$stateParams', '$scope', 'SparksXService', function ($rootScope, $state, $stateParams, $scope, SparksXService) {

    $scope.$on('$viewContentLoaded', function () {
        App.initAjax();
        $rootScope.settings.layout.pageContentWhite = true;
        $rootScope.settings.layout.pageBodySolid = false;
    });


    $scope.List = function () {
        $rootScope.settings.layout.pageSidebarClosed = true;
        SparksXService.ListWorkOrder().success(function (data) {
            $scope.list = data;
        });
    };

    $scope.Get = function () {
        SparksXService.GetWorkOrder($stateParams.id).success(function (data) {
            $scope.object = data;
        });
    };

    $scope.Add = function (obj) {
        obj.CreatedBy = $rootScope.user.UserId;

        SparksXService.AddWorkOrder(obj).success(function (data) {
            $state.go('workorder/get', { id: data });
        });
    };

    
   

}]);