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
        SparksXService.ListGenericReports($rootScope.user.userId).success(function (data) {
            $scope.list = data.result;
        });
    };

    $scope.Get = function () {
        SparksXService.GetGenericReport($stateParams.id).success(function (data) {
            $scope.object = data;
            console.log(data);
            if ($gridContainer == null) {
                $gridContainer = $("#gridContainer").dxDataGrid({
                    dataSource: [],
                    export: {
                        enabled: true,
                        fileName: data.genericReportName
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
                                saveAs(new Blob([buffer], { type: 'application/octet-stream' }), data.genericReportName + '.xlsx');
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
                                saveAs(new Blob([buffer], { type: 'application/octet-stream' }), data.genericReportName + '.xlsx');
                            });
                        });
                        e.cancel = true;
                    },
                }).dxPivotGrid('instance');
            }
        });
    };

    $scope.Add = function (obj) {
        obj.createdBy = $rootScope.user.userId;

        SparksXService.AddGenericReport(obj).success(function (data) {
            $state.go('genericreports/get', { id: data });
        });
    };

    $scope.Edit = function (obj) {
        if (obj.RecordStatusId === 1) {
            obj.ModifiedBy = $rootScope.user.userId;
        }
        else {
            obj.DeletedBy = $rootScope.user.userId;
        }
        SparksXService.EditGenericReport(obj).success(function (data) {
            $state.go('genericreports/get', { id: data });
        });
    };

    $scope.BindAddFields = function () {
        $scope.object = {
            genericReportParameterList: []
        };

        SparksXService.GetUsers().success(function (data) {
            $scope.users = data;
        });

        SparksXService.GetParameterTypes().success(function (data) {
            $scope.parameterTypes = data;
        });
    };

    $scope.GetResultSet = function (obj) {
        SparksXService.EditGenericReport(obj).success(function (data) {
            $state.go('genericreports/List', { id: data });
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

    $scope.GetResultSet = function (isPivot) {

        var args = {
            genericReportId: parseInt($stateParams.id),
            genericReportParameterList: $scope.object.genericReportParameterList,
            userId: $rootScope.user.userId
        };
        $.ajax({
            url: $rootScope.settings.serverPath + '/api/GenericReport/GetResultSet',
            type: "post",
            data: JSON.stringify(args),
            contentType: "application/json",
            dataType: "json",
            beforeSend: function () {
                $gridContainer.beginCustomLoading();
                $scope.IsOnAir = true;
            },
            success: function (data) {
                var response = data.result;
                console.log(response);
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

                                if (obj.dataType == "Decimal") {
                                    value = parseFloat(obj.Value);
                                } else if (obj.DataType == "Int32" || obj.DataType == "Int16") {
                                    value = parseInt(obj.Value);
                                } else if (obj.DataType == "DateTime") {
                                    value = new Date(obj.Value);
                                }

                                object[obj.Caption] = value;
                            });

                            objectList.push(object);
                        });

                        //var fields = [];
                        //if (response.length) {
                        //    var hasDate = false;

                        //    $.each(response[0], function (key, obj) {
                        //        //console.log(key, obj);

                        //        if (obj.DataType == "DateTime") {
                        //            fields.push({
                        //                caption: obj.Caption,
                        //                dataField: obj.Caption.replace(" ", "_"),
                        //                dataType: "date",
                        //                //groupName: "Date",
                        //            });

                        //            //if (!hasDate) {
                        //            //    fields.push({
                        //            //        groupName: "Date", groupInterval: "year", groupIndex: 0
                        //            //    });

                        //            //    fields.push({
                        //            //        groupName: "Date", groupInterval: "month", groupIndex: 1
                        //            //    });
                        //            //}

                        //            hasDate = true;
                        //        }

                        //        if (obj.DataType == "Decimal") {
                        //            fields.push({
                        //                caption: obj.Caption,
                        //                dataField: obj.Caption.replace(" ", "_"),
                        //                dataType: "number",
                        //                summaryType: "sum",
                        //                format: {
                        //                    type: "fixedPoint",
                        //                    precision: 2
                        //                },
                        //            });
                        //        }

                        //        if (obj.DataType == "Int32" || obj.DataType == "Int16") {
                        //            fields.push({
                        //                caption: obj.Caption,
                        //                dataField: obj.Caption.replace(" ", "_"),
                        //                dataType: "number",
                        //                summaryType: "sum",
                        //                format: {
                        //                    type: "fixedPoint",
                        //                    precision: 0
                        //                },
                        //            });
                        //        }
                        //    });

                        //    $pivotContainer.option('dataSource.fields', fields);
                        //}

                        //console.log(fields, objectList);

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
        $scope.object.genericReportParameterList.push({});
    };

    $scope.DeleteParameter = function (i) {
        $scope.object.genericReportParameterList.splice(i, 1);
    };
}]);