angular.module('SparksXApp').controller('InvoiceDetailController', ['$rootScope', '$state', '$stateParams', '$scope', 'SparksXService', function ($rootScope, $state, $stateParams, $scope, SparksXService) {

    $scope.$on('$viewContentLoaded', function () {
        App.initAjax();
        $rootScope.settings.layout.pageContentWhite = true;
        $rootScope.settings.layout.pageBodySolid = false;

       
    });
    $scope.List = function () {
        $rootScope.settings.layout.pageSidebarClosed = false;
        SparksXService.ListInvoiceDetail($stateParams.id).success(function (data) {
            $scope.list = data;
        });
    }
    
}]);