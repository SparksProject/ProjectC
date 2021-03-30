angular.module('SparksXApp').controller('TeminatController', ['$rootScope', '$state', '$stateParams', '$scope', 'SparksXService', function ($rootScope, $state, $stateParams, $scope, SparksXService) {

    $scope.$on('$viewContentLoaded', function () {
        App.initAjax();
        $rootScope.settings.layout.pageContentWhite = true;
        $rootScope.settings.layout.pageBodySolid = false;
    });


    $scope.List = function () {
        SparksXService.ListTeminats().success(function (data) {
            $scope.list = data;
        });
    };

    $scope.Get = function () {
        SparksXService.GetTeminat($stateParams.id).success(function (data) {
            $scope.object = data.Result;
        });
    };

    $scope.Add = function (obj) {
        SparksXService.AddTeminat(obj).success(function (data) {
            $state.go('teminat/get', { id: data });
        });
    };

    $scope.Edit = function (obj) {
        

        SparksXService.EditTeminat(obj).success(function (data) {
            $state.go('teminat/get', { id: data });
        });
    };

    $scope.BindEditFields = function () {
        SparksXService.GetTeminat($stateParams.id).success(function (data) {
            $scope.object = data.Result;

        });
    };

}]);