angular.module('SparksXApp').controller('SparksArchiveController', ['$rootScope', '$state', '$stateParams', '$scope', 'settings', 'SparksXService', '$timeout', 'Upload', function ($rootScope, $state, $stateParams, $scope, settings, SparksXService, $timeout, Upload) {

    $scope.$on('$viewContentLoaded', function () {
        App.initAjax();
        $rootScope.settings.layout.pageContentWhite = true;
        $rootScope.settings.layout.pageBodySolid = false;
        $rootScope.settings.layout.pageSidebarClosed = false;

    });


    function HasValue(val) {
        return val != undefined && val != null && val != '' && val != 'null' && val != 'undefined' && val != ' ';
    }

    // CRUD
    $scope.Get = function () {
        SparksXService.GetSparksArchive($stateParams.id).success(function (data) {
            $scope.object = data;
        });
    };

    $scope.List = function (obj) {
        if (HasValue(obj.TescilNo) || HasValue(obj.DosyaNo) || HasValue(obj.FaturaNo) || (HasValue(obj.TescilTarihiBaslangic) && HasValue(obj.TescilTarihiBitis))) {
            $scope.object.ErrorMessage = false;
            $scope.object.DisableButton = true;
            obj.UserId = $rootScope.user.UserId;
            $scope.list = undefined;

            SparksXService.ListSparksArchives(obj).success(function (data) {
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

    $scope.Delete = function (id) {
        SparksXService.DeleteSparksArchive($stateParams.id).success(function (data) {
            $scope.object = data;
        });
    };

    $scope.BindAddFields = function () {
        console.log($rootScope.user);

        $scope.object = {
            CustomerId: null
        };
        $scope.list = null;

        if ($rootScope.SelectedCustomerId == undefined) {
        } else {
            $scope.object.CustomerId = $rootScope.SelectedCustomerId;
            $scope.hasCustomerSelected = true;
        }

        SparksXService.GetCustomers().success(function (data) {
            $scope.customers = data;

            //SparksXService.ListProducts($scope.object.CustomerId).success(function (data) {
            //    $scope.list = data;
            //});
        });

        var fileTypes = [];

        fileTypes.push({ Id: 'T', Name: 'T-İthalat' });
        fileTypes.push({ Id: 'H', Name: 'H-İhracat' });
        fileTypes.push({ Id: 'P', Name: 'P-Antrepo' });
        fileTypes.push({ Id: '@', Name: '@-NCTS' });
        fileTypes.push({ Id: 'O', Name: 'O-Özet Beyan' });    
        
        $scope.fileTypes = fileTypes;
    };

    $scope.BindEditFields = function () {

        SparksXService.GetSparksArchive($stateParams.id).success(function (obj) {
            $scope.object = obj;

            SparksXService.GetCustomers().success(function (data) {
                $scope.customers = data;
            });

        });
    };

    $scope.Add = function (obj) {
        SparksXService.AddSparksArchive(obj).success(function (data) {
            $state.go('sparksarchives/list');
        });
    };

    $scope.Edit = function (obj) {

        SparksXService.EditSparksArchive(obj).success(function (data) {
            $state.go('sparksarchives/list');
        });
    };


    // Product Changed
    $scope.ProductChanged = function (customerId) {
        $rootScope.SelectedCustomerId = customerId;
        $scope.hasCustomerSelected = true;
        SparksXService.ListSparksArchives(customerId).success(function (data) {
            if (data != null && data.length > 0) {
                $scope.list = data;
            }
        });
    }


    $scope.AddAndUpload = function (obj) {
        SparksXService.AddSparksArchive(obj).success(function (id) {
            Upload.upload({
                url: $rootScope.settings.serverPath + '/api/SparksArchive/Upload/?id=' + id,
                data: {},
                file: $scope.File
            }).then(function (response) {
                $timeout(function () {
                    $state.go('sparksarchives/list');
                });
            });
        });
    }
}]);