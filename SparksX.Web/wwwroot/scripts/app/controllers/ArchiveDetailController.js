angular.module('SparksXApp').controller('ArchiveDetailController', ['$rootScope', '$state', '$stateParams', '$scope', 'SparksXService', function ($rootScope, $state, $stateParams, $scope, SparksXService) {

    $scope.$on('$viewContentLoaded', function () {
        App.initAjax();
        $rootScope.settings.layout.pageContentWhite = true;
        $rootScope.settings.layout.pageBodySolid = false;


    });

    $scope.List = function () {
        $rootScope.settings.layout.pageSidebarClosed = false;

        $scope.list = undefined;
        $scope.object = { WaitAction: true };

        SparksXService.ListArchiveDetail($stateParams.id).success(function (data) {
            if (data != null && data != undefined && data.length > 0) {
                $scope.list = data;
            }

            $scope.object.WaitAction = false;
        }).error(function () {
            $scope.object.WaitAction = false;
        });
    }
}]);