angular.module('SparksXApp').controller('GenericReportController', ['$rootScope', '$state', '$stateParams', '$scope', 'SparksXService', '$http', function ($rootScope, $state, $stateParams, $scope, SparksXService, $http) {

    DevExpress.localization.locale("tr-TR");

    var $gridContainer = null;
    var $pivotContainer = null;

    $scope.$on('$viewContentLoaded', function () {
        App.initAjax();
        $rootScope.settings.layout.pageContentWhite = true;
        $rootScope.settings.layout.pageBodySolid = false;
    });


    $scope.List = function () {
        SparksXService.ListGenericReports($rootScope.user.UserId).success(function (data) {
            $scope.list = data.Items;
        });
    };

    $scope.Get = function () {
        SparksXService.GetGenericReport($stateParams.id).success(function (data) {
            $scope.object = data;

            if ($gridContainer == null) {
                $gridContainer = $("#gridContainer").dxDataGrid({
                    dataSource: [],
                    export: {
                        enabled: true,
                        fileName: data.GenericReportName
                    },
                    onExporting: function (e) {
                        var workbook = new ExcelJS.Workbook();
                        var worksheet = workbook.addWorksheet('Rapor');

                        worksheet.views = [
                            { state: 'frozen', xSplit: null, ySplit: 0 }
                        ];

                        DevExpress.excelExporter.exportDataGrid({
                            worksheet: worksheet,
                            component: e.component,
                            customizeCell: function (options) {
                                var { gridCell, excelCell } = options;

                                if (gridCell.rowType === 'header') {
                                    excelCell.font = { name: 'Arial', size: 11, bold: true };
                                }

                                if (gridCell.rowType === 'data') {
                                    excelCell.font = { name: 'Arial', size: 10 };
                                }

                                excelCell.border = {
                                    top: { style: 'thin', color: { argb: 'FF000000' } },
                                    left: { style: 'thin', color: { argb: 'FF000000' } },
                                    bottom: { style: 'thin', color: { argb: 'FF000000' } },
                                    right: { style: 'thin', color: { argb: 'FF000000' } }
                                };
                            }
                        }).then(function () {
                            workbook.xlsx.writeBuffer().then(function (buffer) {
                                saveAs(new Blob([buffer], { type: 'application/octet-stream' }), data.GenericReportName + '.xlsx');
                            });
                        });
                        e.cancel = true;
                    },
                    paging: {
                        pageSize: 10
                    },
                    pager: {
                        showPageSizeSelector: true,
                        allowedPageSizes: [10, 25, 50, 100]
                    },
                    remoteOperations: false,
                    filterRow: {
                        visible: true,
                        applyFilter: "auto"
                    },
                    searchPanel: {
                        visible: false,
                    },
                    groupPanel: { visible: true },
                    grouping: {
                        autoExpandAll: false
                    },
                    allowColumnReordering: true,
                    allowColumnResizing: true,
                    columnAutoWidth: true,
                    rowAlternationEnabled: true,
                    showBorders: true,
                }).dxDataGrid('instance');
            }

            if ($pivotContainer == null) {
                $pivotContainer = $('#pivotContainer').dxPivotGrid({
                    allowSortingBySummary: true,
                    allowFiltering: true,
                    showBorders: true,
                    showColumnGrandTotals: true,
                    showRowGrandTotals: true,
                    showRowTotals: true,
                    showColumnTotals: true,
                    wordWrapEnabled: false,
                    fieldChooser: {
                        enabled: true,
                        height: 400
                    },
                    dataSource: {
                        fields: [],
                        store: []
                    },
                    export: {
                        enabled: true,
                        fileName: data.GenericReportName
                    },
                    onExporting: function (e) {
                        var workbook = new ExcelJS.Workbook();
                        var worksheet = workbook.addWorksheet('Rapor');

                        worksheet.views = [
                            { state: 'frozen', xSplit: null, ySplit: 0 }
                        ];

                        DevExpress.excelExporter.exportPivotGrid({
                            worksheet: worksheet,
                            component: e.component,
                            customizeCell: function (options) {
                                var { gridCell, excelCell } = options;

                                excelCell.font = { name: 'Arial', size: 10 };
                                excelCell.border = {
                                    top: { style: 'thin', color: { argb: 'FF000000' } },
                                    left: { style: 'thin', color: { argb: 'FF000000' } },
                                    bottom: { style: 'thin', color: { argb: 'FF000000' } },
                                    right: { style: 'thin', color: { argb: 'FF000000' } }
                                };
                            }
                        }).then(function () {
                            workbook.xlsx.writeBuffer().then(function (buffer) {
                                saveAs(new Blob([buffer], { type: 'application/octet-stream' }), data.GenericReportName + '.xlsx');
                            });
                        });
                        e.cancel = true;
                    },
                }).dxPivotGrid('instance');
            }
        });
    };

    $scope.Add = function (obj) {
        obj.CreatedBy = $rootScope.user.UserId;

        SparksXService.AddGenericReport(obj).success(function (data) {
            $state.go('genericreports/get', { id: data });
        });
    };

    $scope.Edit = function (obj) {
        if (obj.RecordStatusId === 1) {
            obj.ModifiedBy = $rootScope.user.UserId;
        }
        else {
            obj.DeletedBy = $rootScope.user.UserId;
        }
        SparksXService.EditGenericReport(obj).success(function (data) {
            $state.go('genericreports/get', { id: data });
        });
    };

    $scope.BindAddFields = function () {
        $scope.object = {
            GenericReportParameterList: []
        };

        SparksXService.GetUsers().success(function (data) {
            $scope.users = data;
        });

        SparksXService.GetParameterTypes().success(function (data) {
            $scope.parameterTypes = data;
        });
    };

    $scope.BindEditFields = function () {
        SparksXService.GetParameterTypes().success(function (data) {
            $scope.parameterTypes = data;
        });

        SparksXService.GetUsers().success(function (data) {
            $scope.users = data;
        });

        SparksXService.GetRecordStatuses().success(function (data) {
            $scope.recordStatuses = data;
        });

        SparksXService.GetGenericReport($stateParams.id).success(function (data) {
            $scope.object = data;
        });
    };

    //$scope.CreateExcel = function (parameters, reportName) {

    //    var object = {
    //        GenericReportId: $stateParams.id,
    //        GenericReportName: reportName,
    //        GenericReportParameterList: parameters
    //    };

    //    SparksXService.CreateExcel(object).success(function (data) {
    //        var fullPath = $rootScope.settings.serverPath + '/files/' + data.GenericReportName;

    //        window.location.href = fullPath;
    //    }).error(function (obj) {
    //        console.log("Bir hata oluştu!");
    //    });
    //}

    $scope.GetResultSet = function (isPivot) {
        $.ajax({
            url: $rootScope.settings.serverPath + '/api/GenericReport/GetResultSet',
            type: "post",
            data: JSON.stringify({
                GenericReportId: $stateParams.id,
                GenericReportParameterList: $scope.object.GenericReportParameterList,
                UserId: $rootScope.user.UserId
            }),
            contentType: "application/json",
            dataType: "json",
            beforeSend: function () {
                $gridContainer.beginCustomLoading();
                $scope.IsOnAir = true;
            },
            success: function (data) {
                var response = data.Result;

                if (response != null && response != undefined && response.length > 0) {
                    if (!isPivot) {
                        // Grid ise
                        $scope.IsPivot = false;
                        var columns = [];

                        $.each(response[0], function (key, element) {
                            var column = {
                                dataField: key + ".Value",
                                caption: element.Caption,
                                dataType: element.DataType.toLowerCase(),
                            };

                            var numberFormats = ["decimal", "double", "int16", "int32", "int64", "byte", "sbyte", "single"];

                            if (numberFormats.includes(column.dataType)) {
                                if (column.dataType == "decimal" || column.dataType == "double") {
                                    column.format = {
                                        type: "fixedPoint",
                                        precision: 2
                                    };
                                }

                                column.dataType = "number";
                            }

                            columns.push(column);
                        });

                        $gridContainer.option('columns', columns);
                        $gridContainer.option('dataSource', response);

                        $scope.columns = columns;
                    } else {
                        // Pivot ise
                        $scope.IsPivot = true;

                        var objectList = [];

                        $.each(response, function (keyRow, objRow) {
                            var object = {};

                            $.each(objRow, function (key, obj) {
                                //console.log(key, obj);
                                var value = obj.Value;

                                if (obj.DataType == "Decimal") {
                                    value = parseFloat(obj.Value);
                                } else if (obj.DataType == "Int32" || obj.DataType == "Int16") {
                                    value = parseInt(obj.value);
                                } else if (obj.DataType == "DateTime") {
                                    value = new Date(obj.Value);
                                }

                                object[obj.Caption] = value;
                            });

                            objectList.push(object);
                        });

                        $pivotContainer.option('dataSource.store', objectList);
                    }
                }
                else if (data.Message != null && data.Message != undefined) { // warning dönünce
                    setTimeout(function () { alert(data.Message); }, 100);
                }
            },
            //error: function (jqXHR, textStatus, errorThrown ) {
            //    console.log(jqXHR.responseJSON.Message);
            //},
            complete: function () {
                $gridContainer.endCustomLoading();
                $scope.IsOnAir = false;
                $scope.$apply();
            }
        });
    }


    // Sub Table Helpers (Add and Delete Buttons)
    $scope.AddParameter = function () {
        $scope.object.GenericReportParameterList.push({});
    };

    $scope.DeleteParameter = function (i) {
        $scope.object.GenericReportParameterList.splice(i, 1);
    };
}]);