SparksXApp.directive('ngSpinnerBar', ['$rootScope', '$state', '$http',
    function ($rootScope, $state, $http) {
        return {
            link: function (scope, element, attrs) {
                // by defult hide the spinner bar
                element.addClass('hide'); // hide spinner bar by default
                $('#block-element').removeClass('page-block');

                $rootScope.isLoading = function () {
                    return $http.pendingRequests.length > 0;
                };

                $rootScope.$watch(function () {
                    if ($http.pendingRequests.length !== 0) {
                        element.removeClass('hide');
                        $('#block-element').addClass('page-block');
                        $('body').removeClass('page-on-load');

                        setTimeout(function () {
                        }, $rootScope.settings.layout.pageAutoScrollOnLoad);
                    } else {
                        element.addClass('hide');
                        $('#block-element').removeClass('page-block');
                    }
                });

                // display the spinner bar whenever the route changes(the content part started loading)
                $rootScope.$on('$stateChangeStart', function () {
                    element.removeClass('hide'); // show spinner bar
                });

                // hide the spinner bar on rounte change success(after the content loaded)
                $rootScope.$on('$stateChangeSuccess', function (event) {
                    element.addClass('hide'); // hide spinner bar
                    $('body').removeClass('page-on-load'); // remove page loading indicator
                    Layout.setAngularJsSidebarMenuActiveLink('match', null, event.currentScope.$state); // activate selected link in the sidebar menu

                    // auto scorll to page top
                    setTimeout(function () {
                        App.scrollTop(); // scroll to the top on content load
                    }, $rootScope.settings.layout.pageAutoScrollOnLoad);
                });

                // handle errors
                $rootScope.$on('$stateNotFound', function () {
                    element.addClass('hide'); // hide spinner bar
                });

                // handle errors
                $rootScope.$on('$stateChangeError', function () {
                    element.addClass('hide'); // hide spinner bar
                });
            }
        };

    }
]);

SparksXApp.directive('a', function () {
    return {
        restrict: 'E',
        link: function (scope, elem, attrs) {
            if (attrs.ngClick || attrs.href === '' || attrs.href === '#') {
                elem.on('click', function (e) {
                    e.preventDefault(); // prevent link click for above criteria
                });
            }
        }
    };
});

SparksXApp.directive('dropdownMenuHover', function () {
    return {
        link: function (scope, elem) {
            elem.dropdownHover();
        }
    };
});

SparksXApp.directive('phone', function () {
    return {
        link: function (scope, element, attrs) {
            $(element).inputmask("mask", {
                "mask": "(999) 999-9999"
            });
        }
    };
});

SparksXApp.directive('iban', function () {
    return {
        link: function (scope, element, attrs) {
            $(element).inputmask("mask", {
                "mask": "TR99-9999-9999-9999-9999-9999-99"
            });
        }
    };
});

SparksXApp.directive('datetime', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, ngModel) {
            if (jQuery().datepicker) {
                $(element).datepicker({
                    rtl: App.isRTL(),
                    orientation: "left",
                    autoclose: true,
                    format: 'dd.mm.yyyy',
                    forceParse: false,
                    weekStart: 1
                });
            }

            ngModel.$parsers.push(function (data) {
                if (data !== undefined && data !== "" && data !== null && data.substring(0, 4) !== "0001") {
                    var date = data.split('.');
                    var result = date[2] + '-' + date[1] + '-' + date[0];

                    return result;
                }
            });

            ngModel.$formatters.push(function (data) {

                if (data !== undefined && data !== "" && data !== null && data.substring(0, 4) !== "0001") {
                    var date = data.substring(0, 10).split('-');
                    var result = date[2] + '.' + date[1] + '.' + date[0];

                    return result;
                }
            });
        }
    };
});

SparksXApp.directive('datetime2', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, ngModelController) {
            $(element).inputmask("d.m.y");

            ngModelController.$parsers.push(function (data) {
                if (data !== undefined || data !== null) {
                    var date = data.split('.');
                    var result = date[2] + '-' + date[1] + '-' + date[0];
                    return result;
                }
            });

            ngModelController.$formatters.push(function (data) {
                if (data !== undefined || data != null) {
                    var date = data.substring(0, 10).split('-');
                    var result = date[2] + '.' + date[1] + '.' + date[0];
                    return result;
                }
            });
        }
    }
});

SparksXApp.directive('datetime3', function () {
    return {
        restrict: 'A',
        require: '?ngModel',
        link: function (scope, element, attrs, ngModelController) {
            // Private variables
            var datepickerFormat = 'dd.mm.yyyy',
                momentFormat = 'dd.mm.yyyy',
                datepicker,
                elPicker;

            datepicker = element.datepicker({
                autoclose: true,
                keyboardNavigation: false,
                todayHighlight: true,
                format: datepickerFormat
            });
            elPicker = datepicker.data('datepicker').picker;

            // Adjust offset on show
            datepicker.on('show', function (evt) {
                elPicker.css('left', parseInt(elPicker.css('left')) + +attrs.offsetX);
                elPicker.css('top', parseInt(elPicker.css('top')) + +attrs.offsetY);
            });

            // Only watch and format if ng-model is present https://docs.angularjs.org/api/ng/type/ngModel.NgModelController
            if (ngModelController) {
                // So we can maintain time
                var lastModelValueMoment;

                ngModelController.$formatters.push(function (modelValue) {

                    //
                    // Date -> String
                    //

                    // Get view value (String) from model value (Date)
                    var viewValue,
                        m = moment(modelValue);
                    if (modelValue && m.isValid()) {
                        // Valid date obj in model
                        lastModelValueMoment = m.clone(); // Save date (so we can restore time later)
                        viewValue = m.format(momentFormat);
                    } else {
                        // Invalid date obj in model
                        lastModelValueMoment = undefined;
                        viewValue = undefined;
                    }

                    // Update picker
                    element.datepicker('update', viewValue);

                    // Update view
                    return viewValue;
                });

                ngModelController.$parsers.push(function (viewValue) {
                    //
                    // String -> Date
                    //

                    // Get model value (Date) from view value (String)
                    var modelValue,
                        m = moment(viewValue, momentFormat, true);
                    if (viewValue && m.isValid()) {
                        // Valid date string in view
                        if (lastModelValueMoment) { // Restore time
                            m.hour(lastModelValueMoment.hour());
                            m.minute(lastModelValueMoment.minute());
                            m.second(lastModelValueMoment.second());
                            m.millisecond(lastModelValueMoment.millisecond());
                        }
                        modelValue = m.toDate();
                    } else {
                        // Invalid date string in view
                        modelValue = undefined;
                    }

                    // Update model
                    return modelValue;
                });

                datepicker.on('changeDate', function (evt) {
                    // Only update if it's NOT an <input> (if it's an <input> the datepicker plugin trys to cast the val to a Date)
                    if (evt.target.tagName !== 'INPUT') {
                        ngModelController.$setViewValue(moment(evt.date).format(momentFormat)); // $seViewValue basically calls the $parser above so we need to pass a string date value in
                        ngModelController.$render();
                    }
                });
            }

        }
    };
});

SparksXApp.directive('number', function () {

    return {
        restrict: 'A',
        require: '?ngModel',
        link: function (scope, element, attrs, modelCtrl) {
            modelCtrl.$parsers.push(function (inputValue) {
                if (inputValue === undefined) return '';
                var transformedInput = inputValue.replace(/[^0-9]/g, '');
                if (transformedInput !== inputValue) {
                    modelCtrl.$setViewValue(transformedInput);
                    modelCtrl.$render();
                }
                return transformedInput;
            });
        }
    };
});

SparksXApp.directive('validSubmit', ['$parse', function ($parse) {
    return {
        compile: function compile(tElement, tAttrs, transclude) {
            return {
                post: function postLink(scope, element, iAttrs, controller) {
                    var form = element.controller('form');
                    form.$submitted = false;
                    var fn = $parse(iAttrs.validSubmit);

                    var input = element.find('input[ng-required], select[ng-required], textarea[ng-required], div[ng-invalid-required]');

                    $('#btn-show-modal').on('click', function (event) {
                        scope.$apply(function () {
                            element.addClass('ng-submitted');
                            form.$submitted = true;
                            if (form.$valid) {
                                $('#btn-show-modal').attr("data-target", "#static");
                            }
                            else {

                                $('#btn-show-modal').removeAttr("data-target");
                                $('.alert.alert-danger').css("display", "block");

                                scope.$watch(form, function () {

                                    angular.forEach(form, function (value, key) {
                                        if (key[0] === '$') return;

                                        var field = $('[name=' + key + ']');
                                        if (value.$invalid) {

                                            field.closest('.form-group').addClass('has-error').removeClass("has-success");
                                            field.parent('.input-icon').children('i').removeClass('fa-check').addClass("fa-warning");
                                        }
                                        else {
                                            field.closest('.form-group').addClass('has-success').removeClass("has-error");
                                            field.parent('.input-icon').children('i').removeClass('fa-warning').addClass("fa-check");
                                        }
                                    });

                                }, true);

                                setTimeout(function () {
                                    App.scrollTop();
                                }, 500);
                            }
                        });
                    });

                    $('#btn-save').on('click', function (event) {
                        scope.$apply(function () {
                            element.addClass('ng-submitted');
                            form.$submitted = true;
                            if (form.$valid) {
                                $('.modal-backdrop.fade.in').hide();
                                fn(scope, { $event: event });
                            }
                            else {
                                $('#btn-show-modal').removeAttr("data-target");
                                $('.alert.alert-danger').css("display", "block");
                                angular.forEach(input, function (formElement, fieldName) {
                                    if ($(formElement).hasClass("ng-invalid")) {
                                        $(formElement).closest('.form-group').addClass('has-error').removeClass("has-success");
                                        $(formElement).parent('.input-icon').children('i').removeClass('fa-check').addClass("fa-warning");
                                    }
                                }, this);

                                element.find('div[ng-required]').each(function () {
                                    if ($(this).hasClass("ng-invalid")) {
                                        $(this).closest('.form-group').addClass('has-error').removeClass("has-success");
                                        $(this).parent('.input-icon').children('i').removeClass('fa-check').addClass("fa-warning");
                                    }
                                })

                                setTimeout(function () {
                                    App.scrollTop();
                                }, 500);
                            }
                        });
                    });

                    scope.$watch(form, function () {

                        angular.forEach(form, function (value, key) {
                            if (key[0] === '$') return;
                            var field = $('[name=' + key + ']');

                            field.on('blur change keyup paste click', function () {

                                if (form.$valid) {
                                    $('.alert.alert-danger').css("display", "none");
                                }

                                if (field.hasClass("ng-valid")) {
                                    field.closest('.form-group').removeClass('has-error').addClass("has-success");
                                    field.parent('.input-icon').children('i').removeClass("fa-warning").addClass("fa-check");
                                }
                                else {
                                    field.closest('.form-group').addClass('has-error').removeClass("has-success");
                                    field.parent('.input-icon').children('i').removeClass('fa-check').addClass("fa-warning");
                                }
                            });

                        }, this);

                    }, true);
                }
            }
        }
    }
}]);

SparksXApp.directive("checkbox", function () {
    return {
        restrict: 'A',
        replace: false,
        scope: {
            ngModel: '='
        },
        transclude: true,
        link: function (scope, element, attrs, ctrl) {
            $(element).val(false);
            scope.$watch('ngModel', function (newValue) {
                $(element).val(newValue);
            });
        }
    }
});

SparksXApp.directive('uppercase', function () {

    return {
        require: 'ngModel',
        link: function (scope, element, attrs, modelCtrl) {
            var capitalize = function (inputValue) {
                if (inputValue === undefined) inputValue = '';
                var capitalized = inputValue.toUpperCase();
                if (capitalized !== inputValue) {
                    // see where the cursor is before the update so that we can set it back
                    var selection = element[0].selectionStart;
                    modelCtrl.$setViewValue(capitalized);
                    modelCtrl.$render();
                    // set back the cursor after rendering
                    element[0].selectionStart = selection;
                    element[0].selectionEnd = selection;
                }
                return capitalized;
            }
            modelCtrl.$parsers.push(capitalize);
            capitalize(scope[attrs.ngModel]); // capitalize initial value
        }
    };
});

//DATATABLE DIRECTIVES========================>

function GetDirectiveTemplate(scope, element, attrs, aoColumns, order, pageLength, $compile) {
    var options = {
        "language": {
            "sProcessing": "İşleniyor...",
            "sLengthMenu": "Sayfada _MENU_ Kayıt Göster",
            "sZeroRecords": "Eşleşen Kayıt Bulunmadı",
            "sInfo": "  _TOTAL_ Kayıttan _START_ - _END_ Arası Kayıtlar",
            "sInfoEmpty": "Kayıt Yok",
            "sInfoFiltered": "( _MAX_ Kayıt İçerisinden Bulunan)",
            "sInfoPostFix": "",
            "sSearch": "Bul:",
            "sUrl": "",
            "oPaginate": {
                "sFirst": "İlk",
                "sPrevious": "Önceki",
                "sNext": "Sonraki",
                "sLast": "Son"
            }
        },

        buttons: [
            { extend: 'print', className: 'btn default', text: 'YAZDIR' },
            { extend: 'pdf', className: 'btn default' },
            { extend: 'excel', className: 'btn default' }
        ],

        "order": order,

        "lengthMenu": [
            [5, 10, 20, 50, -1],
            [5, 10, 20, 50, "Hepsi"] // change per page values here
        ],
        // set the initial value
        "pageLength": pageLength,

        "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",

        "aoColumns": aoColumns,

        createdRow: function (row, data, dataIndex) {
            if ($compile != undefined) {
                $compile(angular.element(row).contents())(scope);
            }
        }
    };

    var explicitColumns = [];

    element.find('th').each(function (index, elem) {
        explicitColumns.push($(elem).text());
    });

    if (attrs.fnRowCallback) {
        options["fnRowCallback"] = scope.$eval(attrs.fnRowCallback);
    }

    scope.$watch(attrs.aaData, function (value) {
        var val = value || null;
        if (val) {
            dataTable.fnClearTable();
            dataTable.fnAddData(scope.$eval(attrs.aaData));
        }
    });

    var dataTable = element.dataTable(options);
}

SparksXApp.directive('dtUser', function () {
    var order = [
        [0, 'asc']
    ];

    var aoColumns = [
        {
            "mDataProp": "createdDate",
            "mRender": function (data, type, full) {
                if (type === 'sort') {
                    return data;
                } else {
                    return full.createdDateDisplay;
                }
            }
        },
        { "mDataProp": "userName" },
        { "mDataProp": "userTypeName" },
        { "mDataProp": "recordStatusName" },
        {
            "mDataProp": "",
            "mRender": function (data, type, full) {
                return '<a href="#/users/get/' + full.userId + '">Detay</a>';
            }
        }
    ];

    var pageLength = 10; // [5, 10, 20, 50, -1]  --> [5, 10, 20, 50, "Hepsi"]

    return function (scope, element, attrs) {
        return GetDirectiveTemplate(scope, element, attrs, aoColumns, order, pageLength)
    };
});

SparksXApp.directive('dtCustomer', function ($compile) {
    return function (scope, element, attrs) {
        var _this = scope;
        var options = {
            "language": {
                "sProcessing": "İşleniyor...",
                "sLengthMenu": "Sayfada _MENU_ Kayıt Göster",
                "sZeroRecords": "Eşleşen Kayıt Bulunmadı",
                "sInfo": "  _TOTAL_ Kayıttan _START_ - _END_ Arası Kayıtlar",
                "sInfoEmpty": "Kayıt Yok",
                "sInfoFiltered": "( _MAX_ Kayıt İçerisinden Bulunan)",
                "sInfoPostFix": "",
                "sSearch": "Bul:",
                "sUrl": "",
                "oPaginate": {
                    "sFirst": "İlk",
                    "sPrevious": "Önceki",
                    "sNext": "Sonraki",
                    "sLast": "Son"
                }
            },

            buttons: [
                { extend: 'print', className: 'btn default', text: 'YAZDIR' },
                { extend: 'pdf', className: 'btn default' },
                { extend: 'excel', className: 'btn default' }
            ],

            "order": [
                [0, 'desc']
            ],

            "lengthMenu": [
                [5, 10, 20, 50, -1],
                [5, 10, 20, 50, "Hepsi"] // change per page values here
            ],

            // set the initial value
            "pageLength": -1,

            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",

            "aoColumns": [
                {
                    "mDataProp": "createdDateDisplay",
                    "mRender": function (data, type, full) {
                        if (type == 'sort') {
                            return full.createdDate;
                        } else {
                            return full.createdDateDisplay;
                        }
                    }
                },
                { "mDataProp": "name" },
                { "mDataProp": "recordStatusName" },
                {
                    "mDataProp": "",
                    "mRender": function (data, type, full) {
                        return '<a href="#/customers/get/' + full.customerId + '">Detay</a>';
                    }
                }
            ],
            createdRow: function (row, data, dataIndex) {
                $compile(angular.element(row).contents())(_this);
            }
        };

        var explicitColumns = [];

        element.find('th').each(function (index, elem) {
            explicitColumns.push($(elem).text());
        });

        if (attrs.fnRowCallback) {
            options["fnRowCallback"] = scope.$eval(attrs.fnRowCallback);
        }

        scope.$watch(attrs.aaData, function (value) {
            var val = value || null;
            if (val) {
                dataTable.fnClearTable();
                dataTable.fnAddData(scope.$eval(attrs.aaData));
            }
        });

        var dataTable = element.dataTable(options);
    };
});

SparksXApp.directive('dtProduct', function ($compile) {
    return function (scope, element, attrs) {
        var _this = scope;
        var options = {
            "language": {
                "sProcessing": "İşleniyor...",
                "sLengthMenu": "Sayfada _MENU_ Kayıt Göster",
                "sZeroRecords": "Eşleşen Kayıt Bulunmadı",
                "sInfo": "  _TOTAL_ Kayıttan _START_ - _END_ Arası Kayıtlar",
                "sInfoEmpty": "Kayıt Yok",
                "sInfoFiltered": "( _MAX_ Kayıt İçerisinden Bulunan)",
                "sInfoPostFix": "",
                "sSearch": "Bul:",
                "sUrl": "",
                "oPaginate": {
                    "sFirst": "İlk",
                    "sPrevious": "Önceki",
                    "sNext": "Sonraki",
                    "sLast": "Son"
                }
            },

            buttons: [
                { extend: 'print', className: 'btn default', text: 'YAZDIR' },
                { extend: 'pdf', className: 'btn default' },
                { extend: 'excel', className: 'btn default' }

            ],

            "order": [
                [0, 'desc']
            ],

            "lengthMenu": [
                [5, 10, 20, 50, -1],
                [5, 10, 20, 50, "Hepsi"] // change per page values here
            ],

            // set the initial value
            "pageLength": -1,

            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",

            "aoColumns": [
                { "mDataProp": "customerName" },
                { "mDataProp": "productNo" },
                { "mDataProp": "productNameTr" },
                { "mDataProp": "productNameEng" },
                { "mDataProp": "productNameOrg" },
                { "mDataProp": "hsCode" },
                { "mDataProp": "uom" },
                { "mDataProp": "grossWeight" },
                { "mDataProp": "netWeight" },
                { "mDataProp": "sapCode" },
                { "mDataProp": "countryOfOrigin" },
                {
                    "mDataProp": "",
                    "mRender": function (data, type, full) {
                        return '<a href="#/products/edit/' + full.productId + '">Düzenle</a>';
                    }
                }
            ],
            createdRow: function (row, data, dataIndex) {
                $compile(angular.element(row).contents())(_this);
            }
        };

        var explicitColumns = [];

        element.find('th').each(function (index, elem) {
            explicitColumns.push($(elem).text());
        });

        if (attrs.fnRowCallback) {
            options["fnRowCallback"] = scope.$eval(attrs.fnRowCallback);
        }

        scope.$watch(attrs.aaData, function (value) {
            var val = value || null;
            if (val) {
                dataTable.fnClearTable();
                dataTable.fnAddData(scope.$eval(attrs.aaData));
            }
        });

        var dataTable = element.dataTable(options);
    };
});

SparksXApp.directive('dtMailreport', function ($compile) {
    return function (scope, element, attrs) {
        var _this = scope;
        var options = {
            "language": {
                "sProcessing": "İşleniyor...",
                "sLengthMenu": "Sayfada _MENU_ Kayıt Göster",
                "sZeroRecords": "Eşleşen Kayıt Bulunmadı",
                "sInfo": "  _TOTAL_ Kayıttan _START_ - _END_ Arası Kayıtlar",
                "sInfoEmpty": "Kayıt Yok",
                "sInfoFiltered": "( _MAX_ Kayıt İçerisinden Bulunan)",
                "sInfoPostFix": "",
                "sSearch": "Bul:",
                "sUrl": "",
                "oPaginate": {
                    "sFirst": "İlk",
                    "sPrevious": "Önceki",
                    "sNext": "Sonraki",
                    "sLast": "Son"
                }
            },

            buttons: [
                { extend: 'print', className: 'btn default', text: 'YAZDIR' },
                { extend: 'pdf', className: 'btn default' },
                { extend: 'excel', className: 'btn default' }
            ],

            "order": [
                [0, 'desc']
            ],

            "lengthMenu": [
                [5, 10, 20, 50, -1],
                [5, 10, 20, 50, "Hepsi"] // change per page values here
            ],

            // set the initial value
            "pageLength": -1,

            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",

            "aoColumns": [
                {
                    "mDataProp": "createdDateDisplay",
                    "mRender": function (data, type, full) {
                        if (type == 'sort') {
                            return full.createdDate;
                        } else {
                            return full.createdDateDisplay;
                        }
                    }
                },
                { "mDataProp": "mailReportName" },
                { "mDataProp": "periodTypeName" },
                { "mDataProp": "recordStatusName" },
                {
                    "mDataProp": "",
                    "mRender": function (data, type, full) {
                        return '<a href="#/mailreports/get/' + full.mailReportId + '">Detay</a>';
                    }
                }
            ],
            createdRow: function (row, data, dataIndex) {
                $compile(angular.element(row).contents())(_this);
            }
        };

        var explicitColumns = [];

        element.find('th').each(function (index, elem) {
            explicitColumns.push($(elem).text());
        });

        if (attrs.fnRowCallback) {
            options["fnRowCallback"] = scope.$eval(attrs.fnRowCallback);
        }

        scope.$watch(attrs.aaData, function (value) {
            var val = value || null;
            if (val) {
                dataTable.fnClearTable();
                dataTable.fnAddData(scope.$eval(attrs.aaData));
            }
        });

        var dataTable = element.dataTable(options);
    };
});

SparksXApp.directive('dtGenericreport', function ($rootScope) {
    var order = [
        [0, 'asc']
    ];

    var aoColumns = [
        { "mDataProp": "genericReportName" },
        {
            "mDataProp": "genericReportId",
            "mRender": function (data, type, full) {
                var detailLink = '<a href="#/genericreports/get/' + data + '">Detay</a>';
                var reportLink = '<a href="#/genericreports/genericreportexecute/' + data + '">Rapor Al</a>';
                var isDefaultReportDisplay = '(Sabit Rapor)';

                if ($rootScope.user.userPermissions.genericReportGet) {
                    if (full.isDefaultReport) {
                        return detailLink + ' / ' + reportLink + ' / ' + isDefaultReportDisplay;
                    } else {
                        return detailLink + ' / ' + reportLink;
                    }
                } else {
                    return reportLink;
                }
            }
        }
    ];

    var pageLength = 10;

    return function (scope, element, attrs) {
        return GetDirectiveTemplate(scope, element, attrs, aoColumns, order, pageLength)
    };
});

SparksXApp.directive('dtSparksarchive', function ($compile, $rootScope) {
    return function (scope, element, attrs) {
        var _this = scope;
        var options = {
            "language": {
                "sProcessing": "İşleniyor...",
                "sLengthMenu": "Sayfada _MENU_ Kayıt Göster",
                "sZeroRecords": "Eşleşen Kayıt Bulunmadı",
                "sInfo": "  _TOTAL_ Kayıttan _START_ - _END_ Arası Kayıtlar",
                "sInfoEmpty": "Kayıt Yok",
                "sInfoFiltered": "( _MAX_ Kayıt İçerisinden Bulunan)",
                "sInfoPostFix": "",
                "sSearch": "Bul:",
                "sUrl": "",
                "oPaginate": {
                    "sFirst": "İlk",
                    "sPrevious": "Önceki",
                    "sNext": "Sonraki",
                    "sLast": "Son"
                }
            },

            buttons: [
                { extend: 'print', className: 'btn default', text: 'YAZDIR' },
                { extend: 'pdf', className: 'btn default' },
                { extend: 'excel', className: 'btn default' }
            ],
            "columnDefs": [
                { className: "dt-left", "targets": [0, 1, 2, 3, 4, 5, 6] }
            ],


            "order": [
                [0, 'desc']
            ],

            "lengthMenu": [
                [5, 10, 20, 50, -1],
                [5, 10, 20, 50, "Hepsi"] // change per page values here
            ],

            // set the initial value
            "pageLength": 50,

            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",

            "aoColumns": [
                {
                    "mDataProp": "dosyaAdi",
                    "mRender": function (data, type, full) {
                        return '<a target="_blank" ng-href="' + $rootScope.settings.serverPath + '/files/sparksarchive/download/' + data + '" download ><i class="fa fa-download"> İndir</i></a>';

                        //return '<a target="_blank" ng-href="' + data + '" download ><i class="fa fa-download"> İndir</i></a>';
                    }
                },
                { "mDataProp": "dosyaNo" },
                { "mDataProp": "firma" },
                { "mDataProp": "alici" },
                { "mDataProp": "musRefNo" },
                { "mDataProp": "faturaNo" },
                { "mDataProp": "tescilNo" },
                {
                    "mDataProp": "tescilTarihi",
                    "mRender": function (data, type, full) {
                        var d = new Date(data);
                        month = '' + (d.getMonth() + 1),
                            day = '' + d.getDate(),
                            year = d.getFullYear();
                        if (month.length < 2)
                            month = '0' + month;
                        if (day.length < 2)
                            day = '0' + day;
                        return [day, month, year].join('.');
                        //var date = new Date(data);
                        //return date.toDate();                       
                    }
                }

            ],
            createdRow: function (row, data, dataIndex) {
                $compile(angular.element(row).contents())(_this);
            }
        };

        var explicitColumns = [];

        element.find('th').each(function (index, elem) {
            explicitColumns.push($(elem).text());
        });

        if (attrs.fnRowCallback) {
            options["fnRowCallback"] = scope.$eval(attrs.fnRowCallback);
        }

        scope.$watch(attrs.aaData, function (value) {
            var val = value || null;
            if (val) {
                dataTable.fnClearTable();
                dataTable.fnAddData(scope.$eval(attrs.aaData));
            }
        });

        var dataTable = element.dataTable(options);
    };
});

SparksXApp.directive('dtStokgiris', function ($compile) {
    return function (scope, element, attrs) {
        var _this = scope;
        var options = {
            "language": {
                "sProcessing": "İşleniyor...",
                "sLengthMenu": "Sayfada _MENU_ Kayıt Göster",
                "sZeroRecords": "Eşleşen Kayıt Bulunmadı",
                "sInfo": "  _TOTAL_ Kayıttan _START_ - _END_ Arası Kayıtlar",
                "sInfoEmpty": "Kayıt Yok",
                "sInfoFiltered": "( _MAX_ Kayıt İçerisinden Bulunan)",
                "sInfoPostFix": "",
                "sSearch": "Bul:",
                "sUrl": "",
                "oPaginate": {
                    "sFirst": "İlk",
                    "sPrevious": "Önceki",
                    "sNext": "Sonraki",
                    "sLast": "Son"
                }
            },

            buttons: [
                { extend: 'print', className: 'btn default', text: 'YAZDIR' },
                { extend: 'pdf', className: 'btn default' },
                { extend: 'excel', className: 'btn default' }

            ],

            "order": [
                [0, 'desc']
            ],

            "lengthMenu": [
                [5, 10, 20, 50, -1],
                [5, 10, 20, 50, "Hepsi"] // change per page values here
            ],

            // set the initial value
            "pageLength": -1,

            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",

            "aoColumns": [
                { "mDataProp": "customerName" },
                { "mDataProp": "productNo" },
                { "mDataProp": "productNameTr" },
                { "mDataProp": "productNameEng" },
                { "mDataProp": "productNameOrg" },
                { "mDataProp": "hsCode" },
                { "mDataProp": "uom" },
                { "mDataProp": "grossWeight" },
                { "mDataProp": "netWeight" },
                { "mDataProp": "sapCode" },
                { "mDataProp": "countryOfOrigin" },
                {
                    "mDataProp": "",
                    "mRender": function (data, type, full) {
                        return '<a href="#/stokgiris/edit/' + full.stokGirisId + '">Düzenle</a>';
                    }
                }
            ],
            createdRow: function (row, data, dataIndex) {
                $compile(angular.element(row).contents())(_this);
            }
        };

        var explicitColumns = [];

        element.find('th').each(function (index, elem) {
            explicitColumns.push($(elem).text());
        });

        if (attrs.fnRowCallback) {
            options["fnRowCallback"] = scope.$eval(attrs.fnRowCallback);
        }

        scope.$watch(attrs.aaData, function (value) {
            var val = value || null;
            if (val) {
                dataTable.fnClearTable();
                dataTable.fnAddData(scope.$eval(attrs.aaData));
            }
        });

        var dataTable = element.dataTable(options);
    };
});
