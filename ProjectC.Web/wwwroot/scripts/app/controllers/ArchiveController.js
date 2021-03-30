angular.module('SparksXApp').controller('ArchiveController', ['$rootScope', '$state', '$stateParams', '$scope', 'SparksXService', function ($rootScope, $state, $stateParams, $scope, SparksXService) {

    $scope.$on('$viewContentLoaded', function () {
        App.initAjax();
        $rootScope.settings.layout.pageContentWhite = true;
        $rootScope.settings.layout.pageBodySolid = false;
    });

    function HasValue(val) {
        return val != undefined && val != null && val != '' && val != 'null' && val != 'undefined' && val != ' ';
    }

    $scope.List = function (obj) {
        if (HasValue(obj.TescilNo) || HasValue(obj.DosyaNo) || HasValue(obj.FaturaNo) || (HasValue(obj.TescilTarihiBaslangic) && HasValue(obj.TescilTarihiBitis))) {
            $scope.object.ErrorMessage = false;
            $scope.object.DisableButton = true;
            obj.UserId = $stateParams.id;
            $scope.list = undefined;

            SparksXService.ListArchive(obj).success(function (data) {
                if (data != null && data != undefined && data.length > 0) {
                    $scope.list = data;
                    $scope.object.DisableButton = false;
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

    $scope.BindFields = function () {
        $rootScope.settings.layout.pageSidebarClosed = true;

        $scope.object = {
            TescilTarihiBaslangic: null,
            TescilTarihiBitis: null,
            TescilNo: null,
            DosyaNo: null,
            FaturaNo: null,
            NotFoundMessage: false,
            ErrorMessage: false,
            DisableButton: false,
        };
    };

    $scope.Get = function () {
        SparksXService.GetArchive($stateParams.id).success(function (data) {
            $scope.list = data;
        });

    };

}]);