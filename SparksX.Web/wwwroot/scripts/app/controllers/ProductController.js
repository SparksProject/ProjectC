angular.module('SparksXApp').controller('ProductController', ['$rootScope', '$state', '$stateParams', '$scope', 'settings', 'SparksXService', '$timeout', 'Upload', function ($rootScope, $state, $stateParams, $scope, settings, SparksXService, $timeout, Upload) {

    $scope.$on('$viewContentLoaded', function () {
        App.initAjax();
        $rootScope.settings.layout.pageContentWhite = true;
        $rootScope.settings.layout.pageBodySolid = false;
        $rootScope.settings.layout.pageSidebarClosed = false;

    });


    // CRUD
    $scope.Get = function () {
        SparksXService.GetProduct($stateParams.id).success(function (data) {
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

            SparksXService.ListProducts($scope.object.CustomerId).success(function (data) {
                $scope.list = data;
            });
        });
    };

    $scope.BindEditFields = function () {

        SparksXService.GetProduct($stateParams.id).success(function (obj) {
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

        SparksXService.AddProduct(obj).success(function (data) {
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
        SparksXService.EditProduct(obj).success(function (data) {
            $state.go('products/list');
        });
    };


    // Product Changed
    $scope.ProductChanged = function (customerId) {
        $rootScope.SelectedCustomerId = customerId;
        $scope.hasCustomerSelected = true;
        SparksXService.ListProducts(customerId).success(function (data) {
            $scope.list = data;
        });
    }


    // UploadFile
    $scope.UploadFile = function (id) {
        $scope.HasMessage = false;
        $scope.ResultMessage = undefined;

        if ($scope.ExcelFile == null || id == null) {
            return false;
        }

        var reader = new FileReader();
        reader.readAsBinaryString($scope.ExcelFile);
        reader.onload = function () {
            var file = btoa(reader.result);

            var obj = {
                File: file,
                CreatedBy: $rootScope.user.UserId,
                CustomerId: id
            };

            SparksXService.PostExcel(obj).success(function (data) {
                if (data.Message != null && data.Message != undefined && data.Message.length > 0) {
                    $scope.ResultMessage = data.Message;
                    $scope.HasMessage = true;

                    var content = '<button class="close" data-close="alert"></button>';
                    content += data.Message;

                    $('#divMessage').html(content).removeClass('display-none');
                } else {
                    $state.go('products/list');
                }
            }).error(function () {
                $scope.ResultMessage = "HATA";
                $scope.HasMessage = true;
            });
        };
        reader.onerror = function () {
            console.log("error");
            return false;
        };
    };
}]);