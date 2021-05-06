SparksXApp.controller('AccountController', ['$rootScope', '$state', '$stateParams', '$scope', 'settings', 'SparksXService', function ($rootScope, $state, $stateParams, $scope, settings, SparksXService) {
    $scope.$on('$viewContentLoaded', function () {
        App.initAjax();
        $rootScope.settings.layout.pageContentWhite = true;
        $rootScope.settings.layout.pageBodySolid = false;
        $rootScope.settings.layout.pageSidebarClosed = false;

    });

    $scope.Authenticate = function (object) {
        Login(object);
    };

    $scope.Login = function (object) {
        Login(object);
    }

    $scope.Logout = function () {
        localStorage.clear();
        window.location.href = '/login.html';
    };

    $scope.CheckIfAlreadyLogin = function () {
        return false;

        SparksXService.IsAuthenticated().success(function (result) {
            window.location.href = '/index.html#/dashboard.html';
        });

    }

    $scope.List = function () {
        SparksXService.ListUsers().success(function (data) {
            $scope.list = data.result;
        });
    };

    $scope.Get = function () {
        SparksXService.GetUser($stateParams.id).success(function (data) {
            $scope.object = data.result;
        });
    };

    $scope.BindAddFields = function () {
        SparksXService.GetCustomers().success(function (data) {
            $scope.customers = data;
        });

        SparksXService.GetUserTypes().success(function (data) {
            $scope.userTypes = data;
        });
    };

    $scope.BindEditFields = function () {
        SparksXService.GetUserTypes().success(function (data) {
            $scope.userTypes = data;
        });

        SparksXService.GetRecordStatuses().success(function (data) {
            $scope.recordStatuses = data.Result;
        });

        SparksXService.GetUser($stateParams.id).success(function (obj) {
            $scope.object = obj.result;
        });
        SparksXService.GetCustomers().success(function (data) {
            $scope.customers = data;
        });
    };

    $scope.Add = function (obj) {
        obj.createdBy = $rootScope.user.userId;
        obj.recordStatusId = 1;

        SparksXService.AddUser(obj).success(function (data) {
            $state.go('users/get', { id: data.result });

        });
    };

    $scope.Edit = function (obj) {
        if (obj.recordStatusId === 1) {
            obj.modifiedBy = $rootScope.user.userId;
        }
        else {
            obj.deletedBy = $rootScope.user.userId;
        }
        SparksXService.Edit(obj).success(function (data) {
            $state.go('users/get', { id: data });
        });
    };


    function Login(object) {
        localStorage.clear();
        $rootScope.user = undefined;
        $scope.loginclass = "fa fa-spin fa-spinner";

        SparksXService.Authenticate(object).success(function (data) {
            if (data.result != null) {
                $scope.isAuthenticationSuccessful = true;
                localStorage.setItem("user", JSON.stringify(data.result));
                window.location.href = '/index.html#/dashboard.html';
            }
        }).error(function (err) {
            console.log(err);
            $scope.loginclass = undefined;
            $scope.isAuthenticationSuccessful = false;
            window.Location.href = '/login.html';
        });
    }
}]);