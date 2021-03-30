angular.module('SparksXApp').controller('VendorController', ['$rootScope', '$state', '$stateParams', '$scope', '$timeout', 'Upload', 'SparksXService', function ($rootScope, $state, $stateParams, $scope, $timeout, Upload, SparksXService) {

    $scope.$on('$viewContentLoaded', function () {
        App.initAjax();
        $rootScope.settings.layout.pageContentWhite = true;
        $rootScope.settings.layout.pageBodySolid = false;        
    });

    $scope.List = function () {
        $rootScope.settings.layout.pageSidebarClosed = false;
        SparksXService.ListVendors().success(function (data) {
            $scope.list = data.Result;
        });
    };

    $scope.Add = function (obj) {
        obj.CreatedBy = $rootScope.user.EmployeeId
        obj.RecordStatusId = 1;

        SparksXService.AddVendor(obj).success(function (data) {
            $state.go('vendors/get', { id: data.Result });
        });
    };    

    $scope.Edit = function (obj) {
        if (obj.RecordStatusId === 1) {
            obj.ModifiedBy = $rootScope.user.EmployeeId;
        }
        else {
            obj.DeletedBy = $rootScope.user.EmployeeId;
        }
        SparksXService.UpdateVendor(obj).success(function (data) {
            $state.go('vendors/get', { id: data.Result });
        });
    };

    $scope.EditVendorFile = function (obj) {      
        SparksXService.EditVendorFile(obj).success(function (data) {
            $state.go('vendors/get', { id: data.Result });
        });
    };

    $scope.Get = function () {
        Get($stateParams.id);  
    };

    function Get(id) {
        SparksXService.GetVendor(id).success(function (obj) {
            $scope.object = obj.Result;
        });
    }

    $scope.BindAddFields = function () {
        $rootScope.settings.layout.pageSidebarClosed = false;
        SparksXService.GetCompanyTypes().success(function (result) {
            $scope.companyTypes = result.Result;
        });

        SparksXService.GetCities().success(function (result) {
            $scope.cities = result.Result;
        });
    };

    $scope.BindEditFields = function () {
        $rootScope.settings.layout.pageSidebarClosed = false;
        SparksXService.GetVendor($stateParams.id).success(function (obj) {
            $scope.object = obj.Result;

            SparksXService.GetCompanyTypes().success(function (result) {
                $scope.companyTypes = result.Result;
            });

            SparksXService.GetCities().success(function (result) {
                $scope.cities = result.Result;
            });

            SparksXService.GetTowns(obj.Result.CityId).success(function (result) {
                $scope.towns = result.Result;
            });

            SparksXService.GetRecordStatuses().success(function (data) {
                $scope.recordStatuses = data.Result;
            });
        });
    };

    $scope.GetTowns = function (id) {
        SparksXService.GetTowns(id).success(function (data) {
            $scope.object.TownId = undefined;
            $scope.towns = data.Result;
        });
    };

    $scope.UploadTaxFile = function (id) {
        if ($scope.TaxFile !== null) {
            $scope.TaxFile.upload = Upload.upload({
                url: $rootScope.settings.serverPath + '/Vendor/UploadTaxFile/' + id,
                data: {},
                file: $scope.TaxFile
            }).then(function (response) {
                $timeout(function () {
                    Get(id);
                });
            });
        }
    };

    $scope.UploadTradeRegFile = function (id) {
        if ($scope.TradeRegFile !== null) {
            $scope.TradeRegFile.upload = Upload.upload({
                url: $rootScope.settings.serverPath + '/Vendor/UploadTradeRegFile/' + id,
                data: {},
                file: $scope.TradeRegFile
            }).then(function (response) {
                $timeout(function () {
                    Get(id);
                });
            });
        }
    };

    $scope.UploadOperationFile = function (id) {
        if ($scope.OperationFile !== null) {
            $scope.OperationFile.upload = Upload.upload({
                url: $rootScope.settings.serverPath + '/Vendor/UploadOperationFile/' + id,
                data: {},
                file: $scope.OperationFile
            }).then(function (response) {
                $timeout(function () {
                    Get(id);
                });
            });
        }
    };

    $scope.UploadSignatureFile = function (id) {
        if ($scope.SignatureFile !== null) {
            $scope.SignatureFile.upload = Upload.upload({
                url: $rootScope.settings.serverPath + '/Vendor/UploadSignatureFile/' + id,
                data: {},
                file: $scope.SignatureFile
            }).then(function (response) {
                $timeout(function () {
                    Get(id);
                });
            });
        }
    };   

    $scope.BindListFields = function () {
        $rootScope.settings.layout.pageSidebarClosed = true;

        SparksXService.GetVendors().success(function (result) {
            $scope.vendors = result.Result;
        });

        SparksXService.ListVendorClaimList().success(function (data) {
            $scope.listclaims = data.Result;
        });
    };

    $scope.ListClaims = function (vendorId, begin, end) {
        $rootScope.settings.layout.pageSidebarClosed = true;
        SparksXService.ListVendorClaimList(vendorId, begin, end).success(function (data) {
            $scope.listclaims = data.Result;
        });
    };

}]);
