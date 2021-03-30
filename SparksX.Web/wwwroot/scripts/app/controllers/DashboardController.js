
angular.module('SparksXApp').controller('DashboardController', function ($rootScope, $state, $stateParams, $scope, $sce, settings, SparksXService) {

    $scope.$on('$viewContentLoaded', function () {
        // initialize core components
        App.initAjax();

        // set default layout mode
        $rootScope.settings.layout.pageContentWhite = true;
        $rootScope.settings.layout.pageBodySolid = false;
        $rootScope.settings.layout.pageSidebarClosed = false;
    });
    $scope.CheckUrl = function () {
        if (window.location.href.indexOf('index.html#/') < 1) {
            window.location.href = '/index.html#/dashboard.html';
        }
    };
    $scope.getFrameUrl = function (url) {
        return $sce.trustAsResourceUrl("https://app.powerbi.com/view?r=" + url);
    }

    $scope.GoTo = function (id) {
        $scope.object = null;
        $scope.keyword = null;
        $state.go('students/detail', { id: id });
    };

    $scope.frameUrl = 'https://app.powerbi.com/view?r=eyJrIjoiMjdiMzdhMzgtYjEyOS00OGViLWE4ODMtN2ExZWM3YTZhYWYwIiwidCI6IjBlYzNkMDYwLTk3MjMtNDg0Ni04MWU0LTk4MjU4NTkzYTExNCIsImMiOjl9';
    

    $scope.GetDashboardDetails = function () {
        if ($rootScope.user == undefined || $rootScope.user.EmployeeId == undefined) {
            return false;
        } else {
            return false; // henüz GetDashboardDetails action'ımız yok
        }

        SparksXService.GetDashboardDetails().success(function (data) {
            $scope.dashboard = data.Result;
        });
    };


});
