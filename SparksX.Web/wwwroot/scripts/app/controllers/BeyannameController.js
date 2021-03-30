angular.module('SparksXApp').controller('BeyannameController', ['$rootScope', '$state', '$stateParams', '$scope', 'SparksXService', function ($rootScope, $state, $stateParams, $scope, SparksXService) {

    $scope.$on('$viewContentLoaded', function () {
        App.initAjax();
        $rootScope.settings.layout.pageContentWhite = true;
        $rootScope.settings.layout.pageBodySolid = false;
    });

    function HasValue(val) {
        return val != undefined && val != null && val != '' && val != 'null' && val != 'undefined' && val != ' ';
    }

    $scope.List = function (obj) {
        if (HasValue(obj.Dosya_No) || HasValue(obj.Tip) || HasValue(obj.VarisGumrukA_2) || (HasValue(obj.BeyannameTarihiBaslangic) && HasValue(obj.BeyannameTarihiBitis))) {
            $scope.object.ErrorMessage = false;
            $scope.object.DisableButton = true;
            obj.UserId = $stateParams.id;
            $scope.list = undefined;

            SparksXService.ListBeyanname(obj).success(function (data) {
                if (data != null && data != undefined && data.length > 0) {
                    $scope.list = data;
                    $scope.object.DisableButton = false;
                    $scope.object.NotFoundMessage = false;
                    $scope.$apply();
                } else {
                    $scope.object.NotFoundMessage = true;
                    $scope.object.DisableButton = false;
                    $scope.$apply();
                }
            }).error(function (data) {
                $scope.object.ErrorMessage = true;
                $scope.object.DisableButton = false;
                $scope.$apply();
            });
        }
    };

    $scope.BindEditFields = function () {
        SparksXService.GetBeyanname($stateParams.id).success(function (data) {
            $scope.object = data.Result;

        });
    };

    $scope.BindFields = function () {
        $rootScope.settings.layout.pageSidebarClosed = true;

        $scope.object = {
            Dosya_No: null,
            Tip: null,
            NotFoundMessage: false,
            ErrorMessage: false,
            DisableButton: false,
        };
    };

    $scope.Get = function () {
        SparksXService.GetBeyanname($stateParams.id).success(function (data) {
            $scope.object = data;
        });

    };

}]);