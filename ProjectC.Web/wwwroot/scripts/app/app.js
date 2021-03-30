/// <reference path="../../content/assets/global/plugins/bootstrap-datepicker/js/bootstrap-datepicker.min.js" />
var SparksXApp = angular.module("SparksXApp", [
    "ui.router",
    "ui.bootstrap",
    "oc.lazyLoad",
    "ngSanitize",
    "ngMask",
]);

/* Configure ocLazyLoader(refer: https://github.com/ocombe/ocLazyLoad) */
SparksXApp.config(['$ocLazyLoadProvider', function ($ocLazyLoadProvider) {
    $ocLazyLoadProvider.config({
        // global configs go here
    });
}]);

//AngularJS v1.3.x workaround for old style controller declarition in HTML
SparksXApp.config(['$controllerProvider', function ($controllerProvider) {
    // this option might be handy for migrating old apps, but please don't use it
    // in new ones!
    $controllerProvider.allowGlobals();
}]);

/* Setup global settings */
SparksXApp.factory('settings', ['$http', '$rootScope', function ($http, $rootScope) {

    var localServerPath = 'http://localhost:6151';

    // supported languages
    var settings = {
        layout: {
            pageSidebarClosed: false, // sidebar menu state
            pageContentWhite: true, // set page content layout
            pageBodySolid: false, // solid body color state
            pageAutoScrollOnLoad: 500 // auto scroll to top on page load
        },
        assetsPath: '/content/assets',
        globalPath: '/content/assets/global',
        layoutPath: '/content/assets/layouts/layout2',
        serverPath: localServerPath,
    };

    $rootScope.settings = settings;
    $rootScope.user = JSON.parse(localStorage.getItem("user"));

    if ($rootScope.user !== null) {
        $http.defaults.headers.common.Authorization = 'Bearer ' + $rootScope.user.Token;
    }

    return settings;
}]);

/* Setup App Main Controller */
SparksXApp.controller('AppController', ['$scope', '$rootScope', function ($scope, $rootScope) {
    $scope.$on('$viewContentLoaded', function () {
        //App.initComponents(); // init core components
        //Layout.init(); //  Init entire layout(header, footer, sidebar, etc) on page load if the partials included in server side instead of loading with ng-include directive
    });
}]);

SparksXApp.run(function ($rootScope, $state) {
    $rootScope.$on("$stateChangeStart", function (event, toState, toParams, fromState, fromParams) {
        if (toState.data.isAuthenticated === false) {
            $state.go("dashboard");
            App.stopPageLoading();
            event.preventDefault();
        }
    });
});

/* Setup Layout Part - Header */
SparksXApp.controller('HeaderController', ['$scope', function ($scope) {
    $scope.$on('$includeContentLoaded', function () {
        Layout.initHeader(); // init header
    });
}]);

/* Setup Layout Part - Sidebar */
SparksXApp.controller('SidebarController', ['$state', '$scope', function ($state, $scope) {
    $scope.$on('$includeContentLoaded', function () {
        Layout.initSidebar($state); // init sidebar
    });
}]);

/* Setup Layout Part - Quick Sidebar */
SparksXApp.controller('QuickSidebarController', ['$scope', function ($scope) {
    $scope.$on('$includeContentLoaded', function () {
        setTimeout(function () {
            QuickSidebar.init(); // init quick sidebar
        }, 2000);
    });
}]);

/* Setup Layout Part - Theme Panel */
SparksXApp.controller('ThemePanelController', ['$scope', function ($scope) {
    $scope.$on('$includeContentLoaded', function () {
        Demo.init(); // init theme panel
    });
}]);

/* Setup Layout Part - Footer */
SparksXApp.controller('FooterController', ['$scope', function ($scope) {
    $scope.$on('$includeContentLoaded', function () {
        Layout.initFooter(); // init footer
    });
}]);

SparksXApp.run(function ($rootScope, SparksXService) {

    $rootScope.$on("$stateChangeStart", function (event, toState, toParams, fromState, fromParams) {

        SparksXService.IsAuthenticated().success(function (data) {

        }).error(function (data, status) {
            console.log(status);

            if (status === 401) {
                window.location.href = '/login.html';
                event.preventDefault();
            }
            else if (status === 404) {
            }
            else if (status === 500) {
            }
            else if (status === 200) {
                window.location.href = '';
            }
        });
    });
});

SparksXApp.provider('permissions', function () {
    var $this = this;

    this.$get = function () {
        return $this;
    }

    var user = JSON.parse(localStorage.getItem("user"));
    if (user != null) {
        var permissions = user.UserPermissions;
        this.userDetail = permissions;
    }
});

/* Init global settings and run the app */
SparksXApp.run(["$rootScope", "settings", "$state", function ($rootScope, settings, $state) {
    $rootScope.$state = $state; // state to be accessed from view
    $rootScope.$settings = settings; // settings to be accessed from view
}]);

/* Setup Routing For All Pages */
SparksXApp.config(['$stateProvider', '$urlRouterProvider', 'permissionsProvider', function ($stateProvider, $urlRouterProvider, permissionsProvider) {
    // Redirect any unmatched url
    // $urlRouterProvider.otherwise("dashboard");

    $stateProvider

        //Dashboard
        .state('dashboard', {
            url: "/dashboard.html",
            templateUrl: "/views/dashboard.html",
            data: { area: "Genel Görünüm", pageTitle: 'Genel Görünüm' },
            controller: "DashboardController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load({
                        name: 'SparksXApp',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/content/assets/global/plugins/morris/morris.css',
                            '/content/assets/global/plugins/morris/morris.min.js',
                            '/content/assets/global/plugins/morris/raphael-min.js',
                            '/content/assets/global/plugins/jquery.sparkline.min.js',
                            '/content/assets/pages/scripts/dashboard.min.js',

                            '/scripts/app/controllers/DashboardController.js'
                        ]
                    });
                }]
            }
        })

        //Users====================================================================

        //users/list
        .state('users/list', {
            url: "/users/list",
            templateUrl: "/views/users/list.html",
            data: { area: "SparksX", controller: "Kullanıcılar", action: "Tüm Kayıtlar", pageTitle: 'Kullanıcılar | Tüm Kayıtlar', isAuthenticated: permissionsProvider.userDetail != undefined ? permissionsProvider.userDetail.UserList : false },
            controller: "AccountController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'SparksXApp',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/content/assets/global/plugins/datatables/plugins/bootstrap/datatables.bootstrap.js',
                            '/scripts/app/controllers/AccountController.js'
                        ]
                    }]);
                }]
            }
        })

        //users/get
        .state('users/get', {
            url: "/users/get/:id",
            templateUrl: "/views/users/get.html",
            data: { area: "SparksX", controller: "Kullanıcılar", action: "Kayıt Detayı", pageTitle: 'Kullanıcılar | Kayıt Detayı', isAuthenticated: permissionsProvider.userDetail != undefined ? permissionsProvider.userDetail.UserGet : false },
            controller: "AccountController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'SparksXApp',
                        insertBefore: '#ng_load_plugins_before',
                        files: [

                            '/scripts/app/controllers/AccountController.js'
                        ]
                    }]);
                }]
            }
        })

        //users/add
        .state('users/add', {
            url: "/users/add",
            templateUrl: "/views/users/add.html",
            data: { area: "SparksX", controller: "Kullanıcılar", action: "Yeni Kayıt", pageTitle: 'Kullanıcılar | Yeni Kayıt', isAuthenticated: permissionsProvider.userDetail != undefined ? permissionsProvider.userDetail.UserAdd : false },
            controller: "AccountController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'ui.select',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/content/assets/global/plugins/angularjs/plugins/ui-select/select.min.css',
                            '/content/assets/global/plugins/angularjs/plugins/ui-select/select.min.js'
                        ]
                    }, {
                        name: 'SparksXApp',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/scripts/app/controllers/AccountController.js',
                        ]
                    }]);
                }]
            }
        })

        //users/edit
        .state('users/edit', {
            url: "/users/edit/:id",
            templateUrl: "/views/users/edit.html",
            data: { area: "SparksX", controller: "Kullanıcılar", action: "Kayıt Düzenleme", pageTitle: 'Kullanıcılar | Kayıt Düzenleme', isAuthenticated: permissionsProvider.userDetail != undefined ? permissionsProvider.userDetail.UserEdit : false },
            controller: "AccountController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'ui.select',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/content/assets/global/plugins/angularjs/plugins/ui-select/select.min.css',
                            '/content/assets/global/plugins/angularjs/plugins/ui-select/select.min.js'
                        ]
                    }, {
                        name: 'SparksXApp',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/scripts/app/controllers/AccountController.js',
                        ]
                    }]);
                }]
            }
        })

        //users/changepassword
        .state('users/changepassword', {
            url: "/users/changepassword/:id",
            templateUrl: "/views/users/changepassword.html",
            data: { area: "SparksX", controller: "Kullanıcılar", action: "Şifre Değiştirme", pageTitle: 'Kullanıcılar | Şifre Değiştirme', isAuthenticated: permissionsProvider.userDetail != undefined ? true : false },
            controller: "AccountController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'ui.select',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/content/assets/global/plugins/angularjs/plugins/ui-select/select.min.css',
                            '/content/assets/global/plugins/angularjs/plugins/ui-select/select.min.js'
                        ]
                    }, {
                        name: 'SparksXApp',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/scripts/app/controllers/AccountController.js',
                        ]
                    }]);
                }]
            }
        })
        //Company====================================================================

        //company/edit
        .state('company/edit', {
            url: "/company/edit/:id",
            templateUrl: "/views/company/edit.html",
            data: { area: "SparksX", controller: "Firma Bilgileri", action: "Kayıt Detayı", pageTitle: 'Firma Bilgileri | Kayıt Düzenleme', isAuthenticated: permissionsProvider.userDetail != undefined ? permissionsProvider.userDetail.CompanyEdit : false },
            controller: "CompanyController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'ui.select',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/content/assets/global/plugins/angularjs/plugins/ui-select/select.min.css',
                            '/content/assets/global/plugins/angularjs/plugins/ui-select/select.min.js'
                        ]
                    }, {
                        name: 'SparksXApp',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/scripts/app/controllers/CompanyController.js',
                        ]
                    }]);
                }]
            }
        })

        //company/get
        .state('company/get', {
            url: "/company/get/:id",
            templateUrl: "/views/company/get.html",
            data: { area: "SparksX", controller: "Firma Bilgileri", action: "Kayıt Detayı", pageTitle: 'Firma Bilgileri | Kayıt Detayı' },
            controller: "CompanyController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'SparksXApp',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/scripts/app/controllers/CompanyController.js',
                        ]
                    }]);
                }]
            }
        })

        //vendors/editfiles
        .state('vendors/editfiles', {
            url: "/vendors/editfiles/:id",
            templateUrl: "/views/vendors/editfiles.html",
            data: { area: "SparksX", controller: "Bayiler", action: "Başvuru Evrakları", pageTitle: 'Bayiler | Başvuru Evrakları', isAuthenticated: permissionsProvider.userDetail != undefined ? (permissionsProvider.userDetail.RoleId === "1" || permissionsProvider.userDetail.RoleId === "4") : false },
            controller: "VendorController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'ngFileUpload',
                        insertBefore: '#ng_load_plugins_before',
                        files: [

                            '/scripts/app/custom/angular-file-upload/ng-file-upload-shim.min.js',
                            '/scripts/app/custom/angular-file-upload/ng-file-upload.min.js'
                        ]
                    },
                    {
                        name: 'SparksXApp',
                        files: [
                            '/content/assets/global/plugins/jquery-inputmask/jquery.inputmask.bundle.min.js',
                            '/content/assets/global/plugins/bootstrap-fileinput/bootstrap-fileinput.css',
                            '/content/assets/global/plugins/bootstrap-fileinput/bootstrap-fileinput.js',
                            '/scripts/app/custom/lightbox/css/lightbox.min.css',
                            '/scripts/app/custom/lightbox/js/lightbox.min.js',

                            '/scripts/app/controllers/VendorController.js'
                        ]
                    }]);
                }]
            }
        })

        //maildefinition====================================================================

        //maildefinition/add
        .state('mailreport/addmaildefinition', {
            url: "/mailreport/addmaildefinition",
            templateUrl: "/views/mailreport/addmaildefinition.html",
            data: { area: "SparksX", controller: "E-posta Tanımları", action: "Yeni Kayıt", pageTitle: 'E-posta Tanımları | Yeni Kayıt', isAuthenticated: permissionsProvider.userDetail != undefined ? permissionsProvider.userDetail.MailDefinitionAdd : false },
            controller: "MailReportController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'SparksXApp',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/scripts/app/controllers/MailReportController.js',
                        ]
                    }]);
                }]
            }
        })

        //customers====================================================================

        //customers/list
        .state('customers/list', {
            url: "/customers/list",
            templateUrl: "/views/customer/list.html",
            data: { area: "SparksX", controller: "Müşteriler", action: "Tüm Kayıtlar", pageTitle: 'Müşteriler | Tüm Kayıtlar', isAuthenticated: permissionsProvider.userDetail != undefined ? permissionsProvider.userDetail.CustomerList : false },
            controller: "CustomerController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'SparksXApp',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/content/assets/global/plugins/datatables/plugins/bootstrap/datatables.bootstrap.js',
                            '/scripts/app/controllers/CustomerController.js'
                        ]
                    }]);
                }]
            }
        })

        //customers/get
        .state('customers/get', {
            url: "/customers/get/:id",
            templateUrl: "/views/customer/get.html",
            data: { area: "SparksX", controller: "Müşteriler", action: "Kayıt Detayı", pageTitle: 'Müşteriler | Kayıt Detayı', isAuthenticated: permissionsProvider.userDetail != undefined ? permissionsProvider.userDetail.CustomerGet : false },
            controller: "CustomerController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'SparksXApp',
                        insertBefore: '#ng_load_plugins_before',
                        files: [

                            '/scripts/app/controllers/CustomerController.js'
                        ]
                    }]);
                }]
            }
        })

        //customers/add
        .state('customers/add', {
            url: "/customers/add",
            templateUrl: "/views/customer/add.html",
            data: { area: "SparksX", controller: "Müşteriler", action: "Yeni Kayıt", pageTitle: 'Müşteriler | Yeni Kayıt', isAuthenticated: permissionsProvider.userDetail != undefined ? permissionsProvider.userDetail.CustomerAdd : false },
            controller: "CustomerController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'SparksXApp',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/scripts/app/controllers/CustomerController.js',
                        ]
                    }]);
                }]
            }
        })

        //customers/edit
        .state('customers/edit', {
            url: "/customers/edit/:id",
            templateUrl: "/views/customer/edit.html",
            data: { area: "SparksX", controller: "Müşteriler", action: "Kayıt Düzenleme", pageTitle: 'Müşteriler | Kayıt Düzenleme', isAuthenticated: permissionsProvider.userDetail != undefined ? permissionsProvider.userDetail.CustomerEdit : false },
            controller: "CustomerController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'ui.select',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/content/assets/global/plugins/angularjs/plugins/ui-select/select.min.css',
                            '/content/assets/global/plugins/angularjs/plugins/ui-select/select.min.js'
                        ]
                    }, {
                        name: 'SparksXApp',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/scripts/app/controllers/CustomerController.js',
                        ]
                    }]);
                }]
            }
        })

        //products====================================================================

        //products/list
        .state('products/list', {
            url: "/products/list",
            templateUrl: "/views/product/list.html",
            data: { area: "SparksX", controller: "Ürünler", action: "Tüm Kayıtlar", pageTitle: 'Ürünler | Tüm Kayıtlar', isAuthenticated: permissionsProvider.userDetail != undefined ? permissionsProvider.userDetail.ProductList : false },
            controller: "ProductController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'ngFileUpload',
                        insertBefore: '#ng_load_plugins_before',
                        files: [

                            '/scripts/app/custom/angular-file-upload/ng-file-upload-shim.min.js',
                            '/scripts/app/custom/angular-file-upload/ng-file-upload.min.js'
                        ]
                    }, {
                        name: 'ui.select',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/content/assets/global/plugins/angularjs/plugins/ui-select/select.min.css',
                            '/content/assets/global/plugins/angularjs/plugins/ui-select/select.min.js'
                        ]
                    }, {
                        name: 'SparksXApp',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/content/assets/global/plugins/datatables/plugins/bootstrap/datatables.bootstrap.js',
                            '/scripts/app/controllers/ProductController.js'
                        ]
                    }]);
                }]
            }
        })

        //products/get
        .state('products/get', {
            url: "/products/get/:id",
            templateUrl: "/views/product/get.html",
            data: { area: "SparksX", controller: "Ürünler", action: "Kayıt Detayı", pageTitle: 'Ürünler | Kayıt Detayı', isAuthenticated: permissionsProvider.userDetail != undefined ? permissionsProvider.userDetail.ProductGet : false },
            controller: "ProductController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'ngFileUpload',
                        insertBefore: '#ng_load_plugins_before',
                        files: [

                            '/scripts/app/custom/angular-file-upload/ng-file-upload-shim.min.js',
                            '/scripts/app/custom/angular-file-upload/ng-file-upload.min.js'
                        ]
                    }, {
                        name: 'SparksXApp',
                        insertBefore: '#ng_load_plugins_before',
                        files: [

                            '/scripts/app/controllers/ProductController.js'
                        ]
                    }]);
                }]
            }
        })

        //products/add
        .state('products/add', {
            url: "/products/add",
            templateUrl: "/views/product/add.html",
            data: { area: "SparksX", controller: "Ürünler", action: "Yeni Kayıt", pageTitle: 'Ürünler | Yeni Kayıt', isAuthenticated: permissionsProvider.userDetail != undefined ? permissionsProvider.userDetail.ProductAdd : false },
            controller: "ProductController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'ngFileUpload',
                        insertBefore: '#ng_load_plugins_before',
                        files: [

                            '/scripts/app/custom/angular-file-upload/ng-file-upload-shim.min.js',
                            '/scripts/app/custom/angular-file-upload/ng-file-upload.min.js',
                        ]
                    }, {
                        name: 'ui.select',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/content/assets/global/plugins/angularjs/plugins/ui-select/select.min.css',
                            '/content/assets/global/plugins/angularjs/plugins/ui-select/select.min.js'
                        ]
                    }, {
                        name: 'SparksXApp',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/scripts/app/controllers/ProductController.js',
                        ]
                    }]);
                }]
            }
        })

        //products/import
        .state('products/import', {
            url: "/products/import",
            templateUrl: "/views/product/import.html",
            data: { area: "SparksX", controller: "Ürünler", action: "Ürün Yükleme", pageTitle: 'Ürünler | Ürün Yükleme', isAuthenticated: permissionsProvider.userDetail != undefined ? permissionsProvider.userDetail.ProductAdd : false },
            controller: "ProductController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'ngFileUpload',
                        insertBefore: '#ng_load_plugins_before',
                        files: [

                            '/scripts/app/custom/angular-file-upload/ng-file-upload-shim.min.js',
                            '/scripts/app/custom/angular-file-upload/ng-file-upload.min.js',

                            '/content/assets/global/plugins/bootstrap-fileinput/bootstrap-fileinput.css',
                            '/content/assets/global/plugins/bootstrap-fileinput/bootstrap-fileinput.js',
                        ]
                    }, {
                        name: 'ui.select',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/content/assets/global/plugins/angularjs/plugins/ui-select/select.min.css',
                            '/content/assets/global/plugins/angularjs/plugins/ui-select/select.min.js'
                        ]
                    }, {
                        name: 'SparksXApp',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/scripts/app/controllers/ProductController.js',
                        ]
                    }]);
                }]
            }
        })

        //products/edit
        .state('products/edit', {
            url: "/products/edit/:id",
            templateUrl: "/views/product/edit.html",
            data: { area: "SparksX", controller: "Ürünler", action: "Kayıt Düzenleme", pageTitle: 'Ürünler | Kayıt Düzenleme', isAuthenticated: permissionsProvider.userDetail != undefined ? permissionsProvider.userDetail.ProductEdit : false },
            controller: "ProductController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'ngFileUpload',
                        insertBefore: '#ng_load_plugins_before',
                        files: [

                            '/scripts/app/custom/angular-file-upload/ng-file-upload-shim.min.js',
                            '/scripts/app/custom/angular-file-upload/ng-file-upload.min.js'
                        ]
                    }, {
                        name: 'ui.select',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/content/assets/global/plugins/angularjs/plugins/ui-select/select.min.css',
                            '/content/assets/global/plugins/angularjs/plugins/ui-select/select.min.js'
                        ]
                    }, {
                        name: 'SparksXApp',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/scripts/app/controllers/ProductController.js',
                        ]
                    }]);
                }]
            }
        })

        // mailreport ====================================================================

        //mailreports/list
        .state('mailreports/list', {
            url: "/mailreports/list",
            templateUrl: "/views/mailreport/list.html",
            data: { area: "SparksX", controller: "Mail Raporları", action: "Tüm Kayıtlar", pageTitle: 'Mail Raporları | Tüm Kayıtlar', isAuthenticated: permissionsProvider.userDetail != undefined ? permissionsProvider.userDetail.MailReportList : false },
            controller: "MailReportController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'SparksXApp',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/content/assets/global/plugins/datatables/plugins/bootstrap/datatables.bootstrap.js',
                            '/scripts/app/controllers/MailReportController.js'
                        ]
                    }]);
                }]
            }
        })

        //mailreports/get
        .state('mailreports/get', {
            url: "/mailreports/get/:id",
            templateUrl: "/views/mailreport/get.html",
            data: { area: "SparksX", controller: "Mail Raporları", action: "Kayıt Detayı", pageTitle: 'Mail Raporları | Kayıt Detayı', isAuthenticated: permissionsProvider.userDetail != undefined ? permissionsProvider.userDetail.MailReportGet : false },
            controller: "MailReportController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'SparksXApp',
                        insertBefore: '#ng_load_plugins_before',
                        files: [

                            '/scripts/app/controllers/MailReportController.js'
                        ]
                    }]);
                }]
            }
        })

        //mailreports/execute
        .state('mailreports/execute', {
            url: "/mailreports/execute/:id",
            templateUrl: "/views/mailreport/execute.html",
            data: { area: "SparksX", controller: "Mail Raporları", action: "Sorguyu Çalıştır", pageTitle: 'Mail Raporları | Sorguyu Çalıştır', isAuthenticated: permissionsProvider.userDetail != undefined ? permissionsProvider.userDetail.MailReportExecute : false },
            controller: "MailReportController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'SparksXApp',
                        insertBefore: '#ng_load_plugins_before',
                        files: [

                            '/scripts/app/controllers/MailReportController.js'
                        ]
                    }]);
                }]
            }
        })

        //mailreports/add
        .state('mailreports/add', {
            url: "/mailreports/add",
            templateUrl: "/views/mailreport/add.html",
            data: { area: "SparksX", controller: "Mail Raporları", action: "Yeni Kayıt", pageTitle: 'Mail Raporları | Yeni Kayıt', isAuthenticated: permissionsProvider.userDetail != undefined ? permissionsProvider.userDetail.MailReportAdd : false },
            controller: "MailReportController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'ui.select',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/content/assets/global/plugins/angularjs/plugins/ui-select/select.min.css',
                            '/content/assets/global/plugins/angularjs/plugins/ui-select/select.min.js'
                        ]
                    }, {
                        name: 'SparksXApp',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/scripts/app/controllers/MailReportController.js',
                        ]
                    }]);
                }]
            }
        })

        //mailreports/edit
        .state('mailreports/edit', {
            url: "/mailreports/edit/:id",
            templateUrl: "/views/mailreport/edit.html",
            data: { area: "SparksX", controller: "Mail Raporları", action: "Kayıt Düzenleme", pageTitle: 'Mail Raporları | Kayıt Düzenleme', isAuthenticated: permissionsProvider.userDetail != undefined ? permissionsProvider.userDetail.MailReportEdit : false },
            controller: "MailReportController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'ui.select',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/content/assets/global/plugins/angularjs/plugins/ui-select/select.min.css',
                            '/content/assets/global/plugins/angularjs/plugins/ui-select/select.min.js'
                        ]
                    }, {
                        name: 'SparksXApp',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/scripts/app/controllers/MailReportController.js',
                        ]
                    }]);
                }]
            }
        })

        //============================================= GENERIC REPORTS

        //genericreports/genericreportexecute
        .state('genericreports/genericreportexecute', {
            url: "/genericreports/genericreportexecute/:id",
            templateUrl: "/views/genericreport/genericreportexecute.html",
            data: { area: "SparksX", controller: "Kullanıcı Raporları", action: "Rapor", pageTitle: 'Rapor | Tüm Kayıtlar', isAuthenticated: permissionsProvider.userDetail != undefined ? permissionsProvider.userDetail.GenericReportExecute : false },
            controller: "GenericReportController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'SparksXApp',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/content/assets/global/plugins/datatables/plugins/bootstrap/datatables.bootstrap.js',
                            '/scripts/app/controllers/GenericReportController.js'
                        ]
                    }]);
                }]
            }
        })

        //genericreports/list
        .state('genericreports/list', {
            url: "/genericreports/list",
            templateUrl: "/views/genericreport/list.html",
            data: { area: "SparksX", controller: "Kullanıcı Raporları", action: "Tüm Kayıtlar", pageTitle: 'Kullanıcı Raporları | Tüm Kayıtlar', isAuthenticated: permissionsProvider.userDetail != undefined ? permissionsProvider.userDetail.GenericReportList : false },
            controller: "GenericReportController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'SparksXApp',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/content/assets/global/plugins/datatables/plugins/bootstrap/datatables.bootstrap.js',
                            '/scripts/app/controllers/GenericReportController.js'
                        ]
                    }]);
                }]
            }
        })

        //genericreports/get
        .state('genericreports/get', {
            url: "/genericreports/get/:id",
            templateUrl: "/views/genericreport/get.html",
            data: { area: "SparksX", controller: "Kullanıcı Raporları", action: "Kayıt Detayı", pageTitle: 'Kullanıcı Raporları | Kayıt Detayı', isAuthenticated: permissionsProvider.userDetail != undefined ? permissionsProvider.userDetail.GenericReportGet : false },
            controller: "GenericReportController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'SparksXApp',
                        insertBefore: '#ng_load_plugins_before',
                        files: [

                            '/scripts/app/controllers/GenericReportController.js'
                        ]
                    }]);
                }]
            }
        })

        //genericreports/add
        .state('genericreports/add', {
            url: "/genericreports/add",
            templateUrl: "/views/genericreport/add.html",
            data: { area: "SparksX", controller: "Kullanıcı Raporları", action: "Yeni Kayıt", pageTitle: 'Kullanıcı Raporları | Yeni Kayıt', isAuthenticated: permissionsProvider.userDetail != undefined ? permissionsProvider.userDetail.GenericReportAdd : false },
            controller: "GenericReportController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'ui.select',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/content/assets/global/plugins/angularjs/plugins/ui-select/select.min.css',
                            '/content/assets/global/plugins/angularjs/plugins/ui-select/select.min.js'
                        ]
                    }, {
                        name: 'SparksXApp',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/scripts/app/controllers/GenericReportController.js',
                        ]
                    }]);
                }]
            }
        })

        //genericreports/edit
        .state('genericreports/edit', {
            url: "/genericreports/edit/:id",
            templateUrl: "/views/genericreport/edit.html",
            data: { area: "SparksX", controller: "Kullanıcı Raporları", action: "Kayıt Düzenleme", pageTitle: 'Kullanıcı Raporları | Kayıt Düzenleme', isAuthenticated: permissionsProvider.userDetail != undefined ? permissionsProvider.userDetail.GenericReportEdit : false },
            controller: "GenericReportController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'ui.select',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/content/assets/global/plugins/angularjs/plugins/ui-select/select.min.css',
                            '/content/assets/global/plugins/angularjs/plugins/ui-select/select.min.js'
                        ]
                    }, {
                        name: 'SparksXApp',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/scripts/app/controllers/GenericReportController.js',
                        ]
                    }]);
                }]
            }
        })
        //WorkOrder====================================================================

        //workorder/list
        .state('workorder/list', {
            url: "/workorder/list/",
            templateUrl: "/views/workorder/list.html",
            data: { area: "SparksX", controller: "İş Emirleri", action: "Tüm Kayıtlar", pageTitle: 'İş Emirleri | Tüm Kayıtlar', isAuthenticated: permissionsProvider.userDetail != undefined ? permissionsProvider.userDetail.WorkOrderMasterList : false },
            controller: "WorkOrderController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'SparksXApp',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/content/assets/global/plugins/datatables/plugins/bootstrap/datatables.bootstrap.js',
                            '/scripts/app/controllers/WorkOrderController.js'
                        ]
                    }]);
                }]
            }
        })
        //Invoice====================================================================

        //Invoice/List
        .state('invoice/list', {
            url: "/invoice/list/:id",
            templateUrl: "/views/invoice/list.html",
            data: { area: "SparksX", controller: "Faturalar", action: "Tüm Kayıtlar", pageTitle: 'Faturalar | Tüm Kayıtlar', isAuthenticated: permissionsProvider.userDetail != undefined ? permissionsProvider.userDetail.WorkOrderMasterList : false },
            controller: "InvoiceController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'SparksXApp',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/content/assets/global/plugins/datatables/plugins/bootstrap/datatables.bootstrap.js',
                            '/scripts/app/controllers/InvoiceController.js'
                        ]
                    }]);
                }]
            }
        })

        .state('invoice/listall', {
            url: "/invoice/listall/:id",
            templateUrl: "/views/invoice/listall.html",
            data: { area: "SparksX", controller: "Faturalar", action: "Tüm Kalemler", pageTitle: 'Faturalar | Tüm Kayıtlar', isAuthenticated: permissionsProvider.userDetail != undefined ? permissionsProvider.userDetail.WorkOrderMasterList : false },
            controller: "InvoiceController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'SparksXApp',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/content/assets/global/plugins/datatables/plugins/bootstrap/datatables.bootstrap.js',
                            '/scripts/app/controllers/InvoiceController.js'
                        ]
                    }]);
                }]
            }
        })


        //Invoice/DEtails
        .state('invoicedetail/list', {
            url: "/invoicedetail/list/:id",
            templateUrl: "/views/invoicedetail/list.html",
            data: { area: "SparksX", controller: "Fatura Detay", action: "Tüm Kayıtlar", pageTitle: 'Faturalar | Tüm Kayıtlar', isAuthenticated: permissionsProvider.userDetail != undefined ? permissionsProvider.userDetail.WorkOrderMasterList : false },
            controller: "InvoiceDetailController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'SparksXApp',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/content/assets/global/plugins/datatables/plugins/bootstrap/datatables.bootstrap.js',
                            '/scripts/app/controllers/InvoiceDetailController.js'
                        ]
                    }]);
                }]
            }
        })
        .state('archive/list', {
            url: "/archive/list/:id",
            templateUrl: "/views/archive/list.html",
            data: { area: "SparksX", controller: " Arşiv ", action: "Tüm Kayıtlar", pageTitle: 'Arşiv | Tüm Kayıtlar', isAuthenticated: permissionsProvider.userDetail != undefined ? permissionsProvider.userDetail.EvrimArchiveList : false },
            controller: "ArchiveController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'SparksXApp',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/content/assets/global/plugins/datatables/plugins/bootstrap/datatables.bootstrap.js',
                            '/scripts/app/controllers/ArchiveController.js'
                        ]
                    }]);
                }]
            }
        })


        .state('archivedetail/list', {
            url: "/archivedetail/list/:id",
            templateUrl: "/views/archivedetail/list.html",
            data: { area: "SparksX", controller: "Arşiv Detay", action: "Arşiv Detayı", pageTitle: 'Arşiv Detay', isAuthenticated: permissionsProvider.userDetail != undefined ? permissionsProvider.userDetail.EvrimArchiveList : false },
            controller: "ArchiveDetailController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'SparksXApp',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/content/assets/global/plugins/datatables/plugins/bootstrap/datatables.bootstrap.js',
                            '/scripts/app/controllers/ArchiveDetailController.js'
                        ]
                    }]);
                }]
            }
        })

        //===================================Teminat

        //teminat/list
        .state('teminats/list', {
            url: "/teminats/list",
            templateUrl: "/views/teminat/list.html",
            data: { area: "SparksX", controller: "Teminatlar", action: "Tüm Kayıtlar", pageTitle: 'Teminatlar | Tüm Kayıtlar', isAuthenticated: permissionsProvider.userDetail != undefined ? permissionsProvider.userDetail.CustomerList : false },
            controller: "TeminatController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'SparksXApp',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/content/assets/global/plugins/datatables/plugins/bootstrap/datatables.bootstrap.js',
                            '/scripts/app/controllers/TeminatController.js'
                        ]
                    }]);
                }]
            }
        })

        //teminat/get
        .state('teminat/get', {
            url: "/teminat/get/:id",
            templateUrl: "/views/teminat/get.html",
            data: { area: "SparksX", controller: "Teminatlar", action: "Kayıt Detayı", pageTitle: 'Teminatlar | Kayıt Detayı', isAuthenticated: permissionsProvider.userDetail != undefined ? permissionsProvider.userDetail.CustomerGet : false },
            controller: "TeminatController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'SparksXApp',
                        insertBefore: '#ng_load_plugins_before',
                        files: [

                            '/scripts/app/controllers/TeminatController.js'
                        ]
                    }]);
                }]
            }
        })

        //teminat/add
        .state('teminat/add', {
            url: "/teminat/add",
            templateUrl: "/views/teminat/add.html",
            data: { area: "SparksX", controller: "Teminatlar", action: "Yeni Kayıt", pageTitle: 'Teminatlar | Yeni Kayıt', isAuthenticated: permissionsProvider.userDetail != undefined ? permissionsProvider.userDetail.CustomerAdd : false },
            controller: "TeminatController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'SparksXApp',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/scripts/app/controllers/TeminatController.js',
                        ]
                    }]);
                }]
            }
        })

        //customers/edit
        .state('teminat/edit', {
            url: "/teminat/edit/:id",
            templateUrl: "/views/teminat/edit.html",
            data: { area: "SparksX", controller: "Teminatlar", action: "Kayıt Düzenleme", pageTitle: 'Teminatlar | Kayıt Düzenleme', isAuthenticated: permissionsProvider.userDetail != undefined ? permissionsProvider.userDetail.CustomerEdit : false },
            controller: "TeminatController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'ui.select',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/content/assets/global/plugins/angularjs/plugins/ui-select/select.min.css',
                            '/content/assets/global/plugins/angularjs/plugins/ui-select/select.min.js'
                        ]
                    }, {
                        name: 'SparksXApp',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/scripts/app/controllers/TeminatController.js',
                        ]
                    }]);
                }]
            }
        })


        //===========================SparksArchive

        //products====================================================================

        //products/list
        .state('sparksarchives/list', {
            url: "/sparksarchives/list",
            templateUrl: "/views/sparksarchive/list.html",
            data: { area: "SparksX", controller: "Sparks Arşiv", action: "Tüm Kayıtlar", pageTitle: 'Arşiv | Tüm Kayıtlar', isAuthenticated: permissionsProvider.userDetail != undefined ? permissionsProvider.userDetail.SparksArchiveList : false },
            controller: "SparksArchiveController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'ngFileUpload',
                        insertBefore: '#ng_load_plugins_before',
                        files: [

                            '/scripts/app/custom/angular-file-upload/ng-file-upload-shim.min.js',
                            '/scripts/app/custom/angular-file-upload/ng-file-upload.min.js'
                        ]
                    }, {
                        name: 'ui.select',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/content/assets/global/plugins/angularjs/plugins/ui-select/select.min.css',
                            '/content/assets/global/plugins/angularjs/plugins/ui-select/select.min.js'
                        ]
                    }, {
                        name: 'SparksXApp',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/content/assets/global/plugins/datatables/plugins/bootstrap/datatables.bootstrap.js',
                            '/scripts/app/controllers/SparksArchiveController.js'
                        ]
                    }]);
                }]
            }
        })

        //sparks/get
        .state('sparksarchive/get', {
            url: "/sparksarchive/get/:id",
            templateUrl: "/views/sparksarchive/get.html",
            data: { area: "SparksX", controller: "Arşiv", action: "Arşiv Detayı", pageTitle: 'Arşiv | Arşiv Detayı', isAuthenticated: permissionsProvider.userDetail != undefined ? permissionsProvider.userDetail.SparksArchiveList : false },
            controller: "SparksArchiveController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'ngFileUpload',
                        insertBefore: '#ng_load_plugins_before',
                        files: [

                            '/scripts/app/custom/angular-file-upload/ng-file-upload-shim.min.js',
                            '/scripts/app/custom/angular-file-upload/ng-file-upload.min.js'
                        ]
                    }, {
                        name: 'SparksXApp',
                        insertBefore: '#ng_load_plugins_before',
                        files: [

                            '/scripts/app/controllers/SparksArchiveController.js'
                        ]
                    }]);
                }]
            }
        })


        .state('sparksarchive/delete', {
            url: "/products/delete/:id",
            templateUrl: "/views/sparksarchives/list.html",
            data: { area: "SparksX", controller: "Arşiv", action: "Arşiv Sil", pageTitle: 'Arşiv | Arşiv Sil', isAuthenticated: permissionsProvider.userDetail != undefined ? permissionsProvider.userDetail.ProductGet : false },
            controller: "SparksArchiveController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'ngFileUpload',
                        insertBefore: '#ng_load_plugins_before',
                        files: [

                            '/scripts/app/custom/angular-file-upload/ng-file-upload-shim.min.js',
                            '/scripts/app/custom/angular-file-upload/ng-file-upload.min.js'
                        ]
                    }, {
                        name: 'SparksXApp',
                        insertBefore: '#ng_load_plugins_before',
                        files: [

                            '/scripts/app/controllers/SparksArchiveController.js'
                        ]
                    }]);
                }]
            }
        })

        ////products/add
        //.state('products/add', {
        //    url: "/products/add",
        //    templateUrl: "/views/product/add.html",
        //    data: { area: "SparksX", controller: "Ürünler", action: "Yeni Kayıt", pageTitle: 'Ürünler | Yeni Kayıt', isAuthenticated: permissionsProvider.userDetail != undefined ? permissionsProvider.userDetail.ProductAdd : false },
        //    controller: "ProductController",
        //    resolve: {
        //        deps: ['$ocLazyLoad', function ($ocLazyLoad) {
        //            return $ocLazyLoad.load([{
        //                name: 'ngFileUpload',
        //                insertBefore: '#ng_load_plugins_before',
        //                files: [

        //                    '/scripts/app/custom/angular-file-upload/ng-file-upload-shim.min.js',
        //                    '/scripts/app/custom/angular-file-upload/ng-file-upload.min.js',
        //                ]
        //            }, {
        //                name: 'ui.select',
        //                insertBefore: '#ng_load_plugins_before',
        //                files: [
        //                    '/content/assets/global/plugins/angularjs/plugins/ui-select/select.min.css',
        //                    '/content/assets/global/plugins/angularjs/plugins/ui-select/select.min.js'
        //                ]
        //            }, {
        //                name: 'SparksXApp',
        //                insertBefore: '#ng_load_plugins_before',
        //                files: [
        //                    '/scripts/app/controllers/ProductController.js',
        //                ]
        //            }]);
        //        }]
        //    }
        //})

        //products/import
        .state('sparksarchive/import', {
            url: "/sparksarchive/import",
            templateUrl: "/views/sparksarchive/import.html",
            data: { area: "SparksX", controller: "Arşiv", action: "Arşiv Ekle", pageTitle: 'Arşivler | Arşiv Ekle', isAuthenticated: permissionsProvider.userDetail != undefined ? permissionsProvider.userDetail.SparksArchiveImport : false },
            controller: "SparksArchiveController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'ngFileUpload',
                        insertBefore: '#ng_load_plugins_before',
                        files: [

                            '/scripts/app/custom/angular-file-upload/ng-file-upload-shim.min.js',
                            '/scripts/app/custom/angular-file-upload/ng-file-upload.min.js',

                            '/content/assets/global/plugins/bootstrap-fileinput/bootstrap-fileinput.css',
                            '/content/assets/global/plugins/bootstrap-fileinput/bootstrap-fileinput.js',
                        ]
                    }, {
                        name: 'ui.select',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/content/assets/global/plugins/angularjs/plugins/ui-select/select.min.css',
                            '/content/assets/global/plugins/angularjs/plugins/ui-select/select.min.js'
                        ]
                    }, {
                        name: 'SparksXApp',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/scripts/app/controllers/SparksArchiveController.js',
                        ]
                    }]);
                }]
            }
        })

        ////products/edit
        //.state('products/edit', {
        //    url: "/products/edit/:id",
        //    templateUrl: "/views/product/edit.html",
        //    data: { area: "SparksX", controller: "Ürünler", action: "Kayıt Düzenleme", pageTitle: 'Ürünler | Kayıt Düzenleme', isAuthenticated: permissionsProvider.userDetail != undefined ? permissionsProvider.userDetail.ProductEdit : false },
        //    controller: "ProductController",
        //    resolve: {
        //        deps: ['$ocLazyLoad', function ($ocLazyLoad) {
        //            return $ocLazyLoad.load([{
        //                name: 'ngFileUpload',
        //                insertBefore: '#ng_load_plugins_before',
        //                files: [

        //                    '/scripts/app/custom/angular-file-upload/ng-file-upload-shim.min.js',
        //                    '/scripts/app/custom/angular-file-upload/ng-file-upload.min.js'
        //                ]
        //            }, {
        //                name: 'ui.select',
        //                insertBefore: '#ng_load_plugins_before',
        //                files: [
        //                    '/content/assets/global/plugins/angularjs/plugins/ui-select/select.min.css',
        //                    '/content/assets/global/plugins/angularjs/plugins/ui-select/select.min.js'
        //                ]
        //            }, {
        //                name: 'SparksXApp',
        //                insertBefore: '#ng_load_plugins_before',
        //                files: [
        //                    '/scripts/app/controllers/ProductController.js',
        //                ]
        //            }]);
        //        }]
        //    }
        //})
        //===========================SparksArchive

        //products====================================================================

        //products/list
        .state('beyanname/list', {
            url: "/beyanname/list/:id",
            templateUrl: "/views/beyanname/list.html",
            data: { area: "SparksX", controller: "Beyanname", action: "Tüm Kayıtlar", pageTitle: 'Beyanname | Tüm Kayıtlar', isAuthenticated: permissionsProvider.userDetail != undefined ? permissionsProvider.userDetail.SparksArchiveList : false },
            controller: "BeyannameController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'ngFileUpload',
                        insertBefore: '#ng_load_plugins_before',
                        files: [

                            '/scripts/app/custom/angular-file-upload/ng-file-upload-shim.min.js',
                            '/scripts/app/custom/angular-file-upload/ng-file-upload.min.js'
                        ]
                    }, {
                        name: 'ui.select',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/content/assets/global/plugins/angularjs/plugins/ui-select/select.min.css',
                            '/content/assets/global/plugins/angularjs/plugins/ui-select/select.min.js'
                        ]
                    }, {
                        name: 'SparksXApp',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/content/assets/global/plugins/datatables/plugins/bootstrap/datatables.bootstrap.js',
                            '/scripts/app/controllers/BeyannameController.js'
                        ]
                    }]);
                }]
            }
        })
        .state('beyanname/get', {
            url: "/beyanname/get/:id",
            templateUrl: "/views/beyanname/get.html",
            data: { area: "SparksX", controller: "Beyanname", action: "Kayıt Detayı", pageTitle: 'Beyanname | Kayıt Detayı', isAuthenticated: permissionsProvider.userDetail != undefined ? permissionsProvider.userDetail.CustomerGet : false },
            controller: "BeyannameController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'SparksXApp',
                        insertBefore: '#ng_load_plugins_before',
                        files: [

                            '/scripts/app/controllers/BeyannameController.js'
                        ]
                    }]);
                }]
            }
        })


        //stokgiris====================================================================

        //stokgiris/list
        .state('stokgiris/list', {
            url: "/stokgiris/list",
            templateUrl: "/views/stokgiris/list.html",
            data: { area: "SparksX", controller: "Stok Giriş Kartı", action: "Tüm Kayıtlar", pageTitle: 'Stok Giriş Kartı | Tüm Kayıtlar', isAuthenticated: permissionsProvider.userDetail != undefined ? permissionsProvider.userDetail.StokGirisList : false },
            controller: "StokGirisController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'ngFileUpload',
                        insertBefore: '#ng_load_plugins_before',
                        files: [

                            '/scripts/app/custom/angular-file-upload/ng-file-upload-shim.min.js',
                            '/scripts/app/custom/angular-file-upload/ng-file-upload.min.js'
                        ]
                    }, {
                        name: 'ui.select',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/content/assets/global/plugins/angularjs/plugins/ui-select/select.min.css',
                            '/content/assets/global/plugins/angularjs/plugins/ui-select/select.min.js'
                        ]
                    }, {
                        name: 'SparksXApp',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/content/assets/global/plugins/datatables/plugins/bootstrap/datatables.bootstrap.js',
                            '/scripts/app/controllers/StokGirisController.js'
                        ]
                    }]);
                }]
            }
        })

        //stokgiris/get
        .state('stokgiris/get', {
            url: "/stokgiris/get/:id",
            templateUrl: "/views/stokgiris/get.html",
            data: { area: "SparksX", controller: "Stok Giriş Kartı", action: "Kayıt Detayı", pageTitle: 'Stok Giriş Kartı | Kayıt Detayı', isAuthenticated: permissionsProvider.userDetail != undefined ? permissionsProvider.userDetail.StokGirisGet : false },
            controller: "StokGirisController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'ngFileUpload',
                        insertBefore: '#ng_load_plugins_before',
                        files: [

                            '/scripts/app/custom/angular-file-upload/ng-file-upload-shim.min.js',
                            '/scripts/app/custom/angular-file-upload/ng-file-upload.min.js'
                        ]
                    }, {
                        name: 'SparksXApp',
                        insertBefore: '#ng_load_plugins_before',
                        files: [

                            '/scripts/app/controllers/StokGirisController.js'
                        ]
                    }]);
                }]
            }
        })

        //stokgiris/add
        .state('stokgiris/add', {
            url: "/stokgiris/add",
            templateUrl: "/views/stokgiris/add.html",
            data: { area: "SparksX", controller: "Stok Giriş Kartı", action: "Yeni Kayıt", pageTitle: 'Stok Giriş Kartı | Yeni Kayıt', isAuthenticated: permissionsProvider.userDetail != undefined ? permissionsProvider.userDetail.StokGirisAdd : false },
            controller: "StokGirisController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'ngFileUpload',
                        insertBefore: '#ng_load_plugins_before',
                        files: [

                            '/scripts/app/custom/angular-file-upload/ng-file-upload-shim.min.js',
                            '/scripts/app/custom/angular-file-upload/ng-file-upload.min.js',
                        ]
                    }, {
                        name: 'ui.select',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/content/assets/global/plugins/angularjs/plugins/ui-select/select.min.css',
                            '/content/assets/global/plugins/angularjs/plugins/ui-select/select.min.js'
                        ]
                    }, {
                        name: 'SparksXApp',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/scripts/app/controllers/StokGirisController.js',
                        ]
                    }]);
                }]
            }
        })

        //stokgiris/import
        .state('stokgiris/import', {
            url: "/stokgiris/import",
            templateUrl: "/views/stokgiris/import.html",
            data: { area: "SparksX", controller: "Stok Giriş Kartı", action: "Ürün Yükleme", pageTitle: 'Stok Giriş Kartı | Ürün Yükleme', isAuthenticated: permissionsProvider.userDetail != undefined ? permissionsProvider.userDetail.StokGirisAdd : false },
            controller: "StokGirisController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'ngFileUpload',
                        insertBefore: '#ng_load_plugins_before',
                        files: [

                            '/scripts/app/custom/angular-file-upload/ng-file-upload-shim.min.js',
                            '/scripts/app/custom/angular-file-upload/ng-file-upload.min.js',

                            '/content/assets/global/plugins/bootstrap-fileinput/bootstrap-fileinput.css',
                            '/content/assets/global/plugins/bootstrap-fileinput/bootstrap-fileinput.js',
                        ]
                    }, {
                        name: 'ui.select',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/content/assets/global/plugins/angularjs/plugins/ui-select/select.min.css',
                            '/content/assets/global/plugins/angularjs/plugins/ui-select/select.min.js'
                        ]
                    }, {
                        name: 'SparksXApp',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/scripts/app/controllers/StokGirisController.js',
                        ]
                    }]);
                }]
            }
        })

        //stokgiris/edit
        .state('stokgiris/edit', {
            url: "/stokgiris/edit/:id",
            templateUrl: "/views/stokgiris/edit.html",
            data: { area: "SparksX", controller: "Stok Giriş Kartı", action: "Kayıt Düzenleme", pageTitle: 'Stok Giriş Kartı | Kayıt Düzenleme', isAuthenticated: permissionsProvider.userDetail != undefined ? permissionsProvider.userDetail.StokGirisEdit : false },
            controller: "StokGirisController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'ngFileUpload',
                        insertBefore: '#ng_load_plugins_before',
                        files: [

                            '/scripts/app/custom/angular-file-upload/ng-file-upload-shim.min.js',
                            '/scripts/app/custom/angular-file-upload/ng-file-upload.min.js'
                        ]
                    }, {
                        name: 'ui.select',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/content/assets/global/plugins/angularjs/plugins/ui-select/select.min.css',
                            '/content/assets/global/plugins/angularjs/plugins/ui-select/select.min.js'
                        ]
                    }, {
                        name: 'SparksXApp',
                        insertBefore: '#ng_load_plugins_before',
                        files: [
                            '/scripts/app/controllers/StokGirisController.js',
                        ]
                    }]);
                }]
            }
        })





}]);