angular.module('SparksXApp').controller('InvoiceController', ['$rootScope', '$state', '$stateParams', '$scope', 'SparksXService', function ($rootScope, $state, $stateParams, $scope, SparksXService) {

    $scope.$on('$viewContentLoaded', function () {
        App.initAjax();
        $rootScope.settings.layout.pageContentWhite = true;
        $rootScope.settings.layout.pageBodySolid = false;

       
    });


    $scope.List = function () {
        $rootScope.settings.layout.pageSidebarClosed = false;
        SparksXService.ListInvoice($stateParams.id).success(function (data) {
            $scope.list = data;
        });
    }
    $scope.ListAll = function (){
        $rootScope.settings.layout.pageSidebarClosed = false;
        SparksXService.ListInvoiceAll($stateParams.id).success(function (data) {
            $scope.list = data;
        });

    }
    
   
    
}]);