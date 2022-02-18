angular.module('SparksXApp').controller('StokCikisController', ['$rootScope', '$state', '$stateParams', '$scope', 'settings', 'SparksXService', '$timeout', 'Upload', '$http', function ($rootScope, $state, $stateParams, $scope, settings, SparksXService, $timeout, Upload, $http) {

    DevExpress.localization.locale("tr-TR");

    var $gridContainer = null;
    var $gridDetail = null;
    var $gridDrop = null;
    var $modalDetail = null;
    var $modalImport = null;
    var $modalDrop = null;
    var DeletedChepStokCikisDetayIdList = [];
    var $btnArchive = null;
    var $btnJobOrder = null;
    var $btnDropSubmit = null;
    var $btnKiloDagit = null;

    var storeStokGiris = new DevExpress.data.CustomStore({
        key: "stokGirisDetayId",
        method: "Get",
        loadMode: "raw", // omit in the DataGrid, TreeList, PivotGrid, and Scheduler
        load: function () {
            var deferred = $.Deferred();

            SparksXService.ListStokGirisDetails().success(function (data) {
                deferred.resolve(data);
            }).error(function () {
                deferred.reject("Data Loading Error");
            });

            return deferred.promise();
        }
    });

    $scope.$on('$viewContentLoaded', function () {
        App.initAjax();
        $rootScope.settings.layout.pageContentWhite = true;
        $rootScope.settings.layout.pageBodySolid = false;
        $rootScope.settings.layout.pageSidebarClosed = false;

    });

    $scope.ModalDetail = function () {
        var amountValue = 0.00;
        var vatAmountValue = 0.00;

        if ($modalDetail == null) {
            $btnArchive = $('#btnArchive');
            $btnJobOrder = $('#btnJobOrder');
            $btnKiloDagit = $('#btnKiloDagit');

            $modalDetail = $('#modalDetail').on({
                "hide.bs.modal": function (e) {
                    if ($('.modal:visible').length > 1) {
                        bodyModalPadding = $('body').css('paddingRight');
                    } else {
                        bodyModalPadding = 0;
                    }
                },
                "shown.bs.modal": function () {

                },
                "hidden.bs.modal": function () {
                    $scope.object = {};
                    $scope.$apply();

                    amountValue = 0.00;
                    vatAmountValue = 0.00;

                    $gridDetail.option("dataSource", []);
                    $gridDetail.cancelEditData();

                    DeletedChepStokCikisDetayIdList = [];

                    $modalDetail.find('form .form-group').removeClass('has-success').removeClass('has-error');
                    $modalDetail.find('form .form-control').removeClass('ng-valid').removeClass('ng-invalid');
                    $modalDetail.find('form .input-icon .fa').removeClass('fa-check').removeClass('fa-warning');

                    $btnArchive.addClass('hidden');
                    $btnJobOrder.addClass('hidden');
                    $btnKiloDagit.addClass('hidden');
                }
            }).modal({
                show: false,
                keyboard: false
            });

            if ($gridDetail == null) {
                $gridDetail = $("#gridDetail").dxDataGrid({
                    keyExpr: "stokCikisDetayId",
                    dataSource: [],
                    columns: [
                        {
                            dataField: "siraNo", caption: "Sıra No", dataType: "number", sortOrder: "asc",
                            validationRules: [{ type: "required" }],
                        },
                        {
                            dataField: "stokGirisDetayId", caption: "Stok Girişi Beyanname No", width: 250,
                            lookup: {
                                dataSource: storeStokGiris, // Edit aşamasında kolonda SelectBox oluşturulur ve tanımlanan kaynaktan ajax get veri alır.
                                displayExpr: function (data) {
                                    return data.beyannameNo + " - " + data.tpsSiraNo;
                                }, // Dönen veride basılacak metin
                                valueExpr: "stokGirisDetayId", // Dönen veride basılacak value
                            },
                            editCellTemplate: function (cellElement, cellInfo) {
                                // Hücre içine DropDownBox kurulumu
                                return $("<div>").dxDropDownBox({
                                    dropDownOptions: { width: 1050 },
                                    dataSource: storeStokGiris,
                                    value: cellInfo.value,
                                    valueExpr: "stokGirisDetayId",
                                    displayExpr: function (data) {
                                        return data.beyannameNo + " - " + data.tpsSiraNo;
                                    }, // Dönen veride basılacak metin
                                    contentTemplate: function (eDDBTemplate) {

                                        // DropDownBox içine DataGrid kurulumu
                                        return $("<div>").dxDataGrid({
                                            dataSource: storeStokGiris,
                                            remoteOperations: true,
                                            filterRow: {
                                                visible: true,
                                            },
                                            columns: [
                                                {
                                                    dataField: "tpsNo", caption: "TPS No", width: 200
                                                }, {
                                                    dataField: "beyannameNo", caption: "Beyanname No", dataType: "string", width: 150
                                                }, {
                                                    dataField: "tpsSiraNo", caption: "TPS Sıra No", dataType: "number", width: 90
                                                }, {
                                                    dataField: "tpsCikisSiraNo", caption: "TPS Çıkış Sıra No", dataType: "number", width: 90
                                                }, {
                                                    dataField: "urunKod", caption: "Ürün Kodu", dataType: "string", width: 90
                                                }, {
                                                    dataField: "esyaCinsi", caption: "Eşya Cinsi", dataType: "string", width: 90
                                                }, {
                                                    dataField: "miktar", caption: "Giriş Miktar", dataType: "number", width: 90
                                                }, {
                                                    dataField: "cikisMiktar", caption: "Çıkış Miktar", dataType: "number", width: 90
                                                }, {
                                                    dataField: "kalanMiktar", caption: "Kalan Miktar", dataType: "number", width: 90
                                                },

                                            ],

                                            hoverStateEnabled: true,
                                            scrolling: { mode: "virtual" },
                                            height: 300,
                                            selection: { mode: "single" },
                                            selectedRowKeys: [cellInfo.value],
                                            focusedRowEnabled: true,
                                            focusedRowKey: cellInfo.value,
                                            onSelectionChanged: function (selectionChangedArgs) {
                                                eDDBTemplate.component.option("value", selectionChangedArgs.selectedRowKeys[0]);
                                                cellInfo.setValue(selectionChangedArgs.selectedRowKeys[0]);

                                                // Seçili satır sayısında min 1 adet arar. Yoksa grid kapatmaz.
                                                if (selectionChangedArgs.selectedRowKeys.length > 0) {
                                                    // satır seçildi ise veriyi edit mode tabloya yazar
                                                    $.each(selectionChangedArgs.selectedRowsData[0], function (rowColumn, rowValue) {
                                                        if (rowValue != undefined) {
                                                            cellInfo.row.cells.forEach(function (cell) {
                                                                if (cell.column.dataField == rowColumn && cell.columnIndex != cellInfo.columnIndex) {
                                                                    if (cell.column.dataField != "miktar") {
                                                                        cellInfo.component.cellValue(cell.rowIndex, cell.columnIndex, rowValue);
                                                                    }
                                                                    if (cell.column.dataField == "urunKod") {
                                                                        var data = {
                                                                            id: rowValue
                                                                        };
                                                                        SparksXService.GetByUrunKod(data.id).success(function (data) {
                                                                            cellInfo.component.cellValue(cell.rowIndex, 4, data.birimTutar);
                                                                            cellInfo.component.cellValue(cell.rowIndex, 6, data.netWeight);
                                                                            cellInfo.component.cellValue(cell.rowIndex, 7, data.grossWeight);
                                                                        }).error(function (er) {
                                                                            swal({
                                                                                icon: "error",
                                                                                title: "Hata!",
                                                                                text: er,
                                                                            });
                                                                        });
                                                                    }
                                                                }
                                                            });
                                                        }
                                                    });

                                                    eDDBTemplate.component.close(); // satır seçildiği için dropdownbox kapatır
                                                }
                                            }
                                        });

                                    },
                                });

                            }
                        },
                        {
                            dataField: "urunKod", caption: "Ürün Kodu", dataType: "string", width: 100,
                        },
                        {
                            dataField: "tpsCikisSiraNo", caption: "TPS Çıkış Sıra No", dataType: "number", width: 130,
                            format: { type: "fixedPoint", precision: 0 }
                        },
                        {
                            dataField: "miktar", caption: "Miktar", dataType: "number",
                            format: { type: "fixedPoint", precision: 2 },
                        },
                        {
                            dataField: "birimTutar", caption: "Birim Tutar", dataType: "number",
                            format: { type: "fixedPoint", precision: 2 },
                        },
                        {
                            dataField: "invoiceAmount", caption: "Fatura Tutar", dataType: "number",
                            format: { type: "fixedPoint", precision: 2 },
                        },
                        {
                            dataField: "brutKg", caption: "Brüt Kg", dataType: "number",
                            format: { type: "fixedPoint", precision: 2 },
                        },
                        {
                            dataField: "netKg", caption: "Net Kg", dataType: "number",
                            format: { type: "fixedPoint", precision: 2 },
                        },

                        {
                            dataField: "invoiceDetailId", caption: "Fatura Detay Id", dataType: "text", width: 130,
                            format: { type: "fixedPoint", precision: 0 }, allowEditing: false,
                        },
                    ],
                    summary: {
                        totalItems: [{
                            column: "siraNo",
                            summaryType: "count"
                        }, {
                            column: "miktar",
                            summaryType: "sum",
                            valueFormat: { type: "fixedPoint", precision: 2 }
                        }, {
                            column: "birimTutar",
                            summaryType: "sum",
                            valueFormat: { type: "fixedPoint", precision: 2 }
                        }, {
                            column: "invoiceAmount",
                            summaryType: "sum",
                            valueFormat: { type: "fixedPoint", precision: 2 }

                        }, {
                            column: "brutKg",
                            summaryType: "sum",
                            valueFormat: { type: "fixedPoint", precision: 2 }
                        }, {
                            column: "netKg",
                            summaryType: "sum",
                            valueFormat: { type: "fixedPoint", precision: 2 }
                        }]


                    },
                    paging: false,
                    export: {
                        enabled: true,
                    },
                    onInitNewRow: function (e) {
                        var ds = e.component.getDataSource();
                        if (ds._items.length) {
                            e.data.siraNo = $(ds._items).last()[0].siraNo + 1;
                        } else {
                            e.data.siraNo = 1;
                        }

                    },
                    onExporting: function (e) {
                        var workbook = new ExcelJS.Workbook();
                        var worksheet = workbook.addWorksheet('Sayfa 1');

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
                                saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'StokÇıkışKalemler.xlsx');
                            });
                        });
                        e.cancel = true;
                    },
                    onContextMenuPreparing: function (e) {
                        if (e.target == "content") {
                            if (!e.items) e.items = [];

                            if ($rootScope.user.userPermissions.stokCikisAdd) {
                                e.items.push({
                                    text: "Ekle",
                                    icon: "add",
                                    onItemClick: function () {
                                        e.component.addRow();
                                    }
                                });
                            }

                            if (e.rowIndex >= 0) {
                                // Tabloda boş satıra sağ tıklandığında sil çıkmaması için düzeltme
                                e.items.push({
                                    text: "Sil",
                                    icon: "trash",
                                    onItemClick: function (eDel) {
                                        if (e.row.isEditing) {
                                            e.component.cancelEditData();
                                        } else {
                                            e.component.deleteRow(e.rowIndex);
                                        }
                                    }
                                });
                            }
                        }
                    },
                    onEditorPreparing: function (e) {//her değişiklikte her satıra bakar 
                        if (e.dataField == "miktar") {
                            e.editorOptions.onValueChanged = function (args) {
                                amountValue = args.value;
                                e.setValue(args.value);
                                e.component.cellValue(e.row.rowIndex, "invoiceAmount", amountValue * vatAmountValue);

                            }
                        }
                        if (e.dataField == "birimTutar") {
                            e.editorOptions.onValueChanged = function (args) {
                                vatAmountValue = args.value;
                                e.setValue(args.value);
                                e.component.cellValue(e.row.rowIndex, "invoiceAmount", amountValue * vatAmountValue);
                            }
                        }
                    },
                    onRowRemoving: function (e) {
                        var deferred = $.Deferred();

                        // Satır silme aşamasında onay sorusu sorulur. İptal edilirse silme işlemi iptal edilir.
                        swal({
                            icon: "warning",
                            title: "Dikkat!",
                            text: "Silmek istediğinize emin mizini?",
                            showCancelButton: true,
                        }, function (result) {
                            if (result) {
                                var rowIndex = e.component.getRowIndexByKey(e.key);
                                e.component.deleteRow(rowIndex);
                                deferred.resolve(false);
                            }
                            else {
                                deferred.resolve(true);
                            }
                        });

                        e.cancel = deferred.promise();
                    },
                    onRowRemoved: function (e) {
                        // Satır silindikten sonra silinen satırı eğer yeni eklenmemiş ise array push eder
                        if (Number.isInteger(e.key)) {
                            DeletedChepStokCikisDetayIdList.push(e.key);
                        }
                    },
                    onRowDblClick: function (e) {
                        // Çift tıklanan satırı düzenleme ayarı
                        e.component.editRow(e.rowIndex);
                    },
                    editing: {
                        mode: "row",
                        allowUpdating: true,
                        allowDeleting: true,
                        allowAdding: true,
                        confirmDelete: false,
                    },
                    filterRow: {
                        visible: false,
                    },
                    groupPanel: {
                        visible: false,
                    },
                    showBorders: true,
                    showRowLines: true,
                    sorting: {
                        mode: "none"
                    },
                }).dxDataGrid('instance');
            }
        }

        if ($modalDrop == null) {
            $btnDropSubmit = $('#btnDropSubmit');

            $modalDrop = $('#modalDrop').on({
                "hide.bs.modal": function (e) {
                    if ($('.modal:visible').length > 1) {
                        bodyModalPadding = $('body').css('paddingRight');
                    } else {
                        bodyModalPadding = 0;
                    }
                },
                "hidden.bs.modal": function (e) {
                    $btnDropSubmit.prop('disabled', true);
                    $scope.object.Drop = {};

                    $gridDrop.option("dataSource", []);

                    $modalDrop.find('form .form-group').removeClass('has-success').removeClass('has-error');
                    $modalDrop.find('form .form-control').removeClass('ng-valid').removeClass('ng-invalid');
                    $modalDrop.find('form .input-icon .fa').removeClass('fa-check').removeClass('fa-warning');

                    $scope.$apply();
                }
            }).modal({
                show: false,
                keyboard: false,
                backdrop: false
            });

            if ($gridDrop == null) {
                $gridDrop = $("#gridDrop").dxDataGrid({
                    keyExpr: "stokGirisDetayId",
                    dataSource: [],
                    columns: [
                        { dataField: "girisBeyannameNo", caption: "Beyanname No", width: 140, },
                        { dataField: "tpsNo", caption: "TPS No", width: 190, },
                        { dataField: "urunKod", caption: "Ürün Kodu", },
                        {
                            dataField: "girisMiktar", caption: "Giriş Adet", dataType: "number",
                            format: { type: "fixedPoint", precision: 0 },
                        },
                        {
                            dataField: "kalanMiktar", caption: "Kalan Adet", dataType: "number",
                            format: { type: "fixedPoint", precision: 0 },
                        },
                        {
                            dataField: "dusulenMiktar", caption: "Düşülen Adet", dataType: "number",
                            format: { type: "fixedPoint", precision: 0 },
                        },
                        {
                            dataField: "bakiyeMiktar", caption: "Bakiye Adet", dataType: "number",
                            format: { type: "fixedPoint", precision: 0 },
                        },
                    ],
                    summary: {
                        totalItems: [
                            {
                                column: "dusulenMiktar",
                                summaryType: "sum",
                            },
                            {
                                column: "bakiyeMiktar",
                                summaryType: "sum",
                            }
                        ]
                    },
                    onCellPrepared: function (e) {
                        if (e.rowType == "data" && e.data.DusulenMiktar > 0) {
                            $(e.cellElement).css('backgroundColor', '#e8f0fe');
                        }
                    },
                    filterRow: {
                        visible: false,
                    },
                    groupPanel: {
                        visible: false,
                    },
                    paging: false,
                    showBorders: true,
                    showRowLines: true,
                    sorting: {
                        mode: "none"
                    },
                }).dxDataGrid('instance');
            }
        }
    }

    // CRUD
    $scope.Grid = function () {
        $scope.filter = {};

        if ($gridContainer == null) {
            $gridContainer = $("#gridContainer").dxDataGrid({
                keyExpr: "stokCikisId",
                dataSource: [],
                columns: [
                    { dataField: "referansNo", caption: "Referans No", },
                    { dataField: "tPSNo", caption: "TPS No", },
                    { dataField: "islemTarihi", caption: "İşlem Tarihi", dataType: "date", formatType: "shortDate" },
                    { dataField: "tpsTarih", caption: "TPS Tarihi", dataType: "date", formatType: "shortDate" },
                    { dataField: "beyannameNo", caption: "Beyanname No", },
                    { dataField: "beyannameTarihi", caption: "Beyanname Tarihi", dataType: "date", formatType: "shortDate" },
                    { dataField: "ihracatciFirmaName", caption: "İhracatci Firma", },
                    { dataField: "aliciFirmaName", caption: "Alıcı Firma", },
                ],
                export: {
                    enabled: true,
                },
                onExporting: function (e) {
                    var workbook = new ExcelJS.Workbook();
                    var worksheet = workbook.addWorksheet('Sayfa 1');

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
                            saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'StokCikis.xlsx');
                        });
                    });
                    e.cancel = true;
                },
                onContextMenuPreparing: function (e) {
                    if (e.target == "content") {
                        if (!e.items) e.items = [];

                        if ($rootScope.user.userPermissions.stokCikisAdd) {
                            e.items.push({
                                text: "Ekle",
                                icon: "add",
                                onItemClick: function () {
                                    var today = new Date();
                                    $scope.object.stokCikisId = 0;
                                    //Bugünki Tarihi Alması için
                                    SparksXService.GetNextReferenceNumber('Cikis').success(function (data) {
                                        $scope.object.referansNo = data;
                                    });
                                    $scope.object.islemTarihi = today.toISOString();
                                    $modalDetail.modal('show');
                                }
                            });

                            //e.items.push({
                            //    text: "Excel'den Al",
                            //    icon: "exportxlsx",
                            //    onItemClick: function () {
                            //        $modalImport.modal('show');
                            //    }
                            //});

                            if (e.rowIndex >= 0) {
                                e.items.push({
                                    text: "Sil",
                                    icon: "trash",
                                    onItemClick: function (eDel) {
                                        swal({
                                            icon: "warning",
                                            title: "Dikkat!",
                                            text: "Silmek istediğinize emin mizini?",
                                            showCancelButton: true,
                                        }, function (result) {
                                            if (result) {
                                                // Silme işlemine devam edilecek
                                                var data = {
                                                    id: e.row.key
                                                };
                                                SparksXService.DeleteStokCikis(data.id).success(function (data) {
                                                    ListData();
                                                }).error(function (er) {
                                                    swal({
                                                        icon: "error",
                                                        title: "Hata!",
                                                        text: er,
                                                    });
                                                });
                                            }
                                        });
                                    }
                                });
                            }
                        }

                        //if (e.rowIndex >= 0) {
                        //    e.items.push({
                        //        text: "Sil",
                        //        icon: "trash",
                        //        onItemClick: function (eDel) {
                        //            swal({
                        //                icon: "warning",
                        //                title: "Dikkat!",
                        //                text: "Silmek istediğinize emin mizini?",
                        //                showCancelButton: true,
                        //            }, function (result) {
                        //                if (result) {
                        //                   //Sil işlemi
                        //                }
                        //            });
                        //        }
                        //    });
                        //}
                    }
                },
                onRowDblClick: function (e) {
                    SparksXService.GetStokCikis(e.key).success(function (data) {
                        $scope.object = data;

                        $gridDetail.option("dataSource", data.chepStokCikisDetayList);

                        $btnArchive.removeClass('hidden');
                        $btnJobOrder.removeClass('hidden');
                        $btnKiloDagit.removeClass('hidden');

                        $modalDetail.modal('show');
                    });
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
                allowColumnReordering: false,
                allowColumnResizing: true,
                columnAutoWidth: true,
                rowAlternationEnabled: true,
                showBorders: true,
                focusedRowEnabled: true,
            }).dxDataGrid('instance');
        }
    }


    $scope.ModalImport = function () {
        if ($modalImport == null) {
            $modalImport = $('#modalImport').on({
                "hide.bs.modal": function (e) {
                    if ($('.modal:visible').length > 1) {
                        bodyModalPadding = $('body').css('paddingRight');
                    } else {
                        bodyModalPadding = 0;
                    }
                },
                "hidden.bs.modal": function () {

                }
            }).modal({
                show: false,
                keyboard: false
            });
        }
    }

    function ListData() {
        var referansNo = $scope.filter.referansNo ? $scope.filter.referansNo : "";
        var beyannameNo = $scope.filter.beyannameNo ? $scope.filter.beyannameNo : "";
        var siparisNo = $scope.filter.siparisNo ? $scope.filter.siparisNo : "";

        SparksXService.ListStokCikises(referansNo, beyannameNo, siparisNo).success(function (data) {
            $gridContainer.option("dataSource", data);
            $gridContainer.endCustomLoading();
        }).error(function () {
            $gridContainer.option("dataSource", []);
            $gridContainer.endCustomLoading();
        });
    }


    $scope.List = function () {
        $rootScope.settings.layout.pageSidebarClosed = false;
        SparksXService.ListStokCikises().success(function (data) {
            $scope.list = data;
        });
    }

    $scope.Get = function () {
        SparksXService.GetStokCikis($stateParams.id).success(function (data) {
            $scope.object = data;
        });
    };

    $scope.Filter = function (obj) {
        ListData();
    };

    $scope.BindFields = function () {
        $scope.object = {};

        SparksXService.GetCustomers().success(function (data) {
            $scope.customers = data;
        });

        SparksXService.GetCustoms().success(function (data) {
            $scope.customs = data;
        });

        SparksXService.GetCurrencyTypes().success(function (data) {
            $scope.currencytypes = data;
        });

        SparksXService.GetPaymentMethods().success(function (data) {
            $scope.paymentmethods = data;
        });

        SparksXService.GetDeliveryTerms().success(function (data) {
            $scope.deliveryterms = data;
        });
    };

    $scope.Action = function (obj) {
        if (obj.islemTarihi == "") {
            obj.islemTarihi = null;
        }

        if (obj.beyannameTarihi == "") {
            obj.beyannameTarihi = null;
        }

        if (obj.tpsTarih == "") {
            obj.tpsTarih = null;
        }

        if (obj.invoiceDate == "") {
            obj.invoiceDate = null;
        }

        obj.referansNo = parseInt(obj.referansNo);

        if (obj.gtbReferenceNo != null) {
            obj.gtbReferenceNo = obj.gtbReferenceNo.toString();
        }

        App.startPageLoading();

        var ds = $gridDetail.getDataSource();

        for (var i = 0; i < ds._items.length; i++) {
            if (!Number.isInteger(ds._items[i].stokCikisDetayId)) {
                ds._items[i].stokCikisDetayId = 0;
            }
        }

        obj.chepStokCikisDetayList = ds._items;
        obj.deletedChepStokCikisDetayIdList = DeletedChepStokCikisDetayIdList;

        $gridDetail
            .getController('validating')
            .validate()
            .then(function (validGrid) {
                if (validGrid) {
                    //if (obj.chepStokCikisDetayList.length > 0) {
                    if (obj.stokCikisId == 0) {
                        // Insert
                        SparksXService.AddStokCikis(obj).success(function (data) {
                            swal({
                                icon: "success",
                                title: "Başarılı!",
                                text: "Ekleme işlemi başarılı.",
                            }, function (result) {
                                if (result) {
                                    $modalDetail.modal('hide');
                                }
                            });

                            ListData();

                            App.stopPageLoading();
                        }).error(function () {
                            swal({
                                icon: "error",
                                title: "Hata!",
                                text: "Ekleme işlemi yapılamadı. Lütfen tekrar deneyin.",
                            });

                            App.stopPageLoading();
                        });
                    } else {
                        // Update
                        SparksXService.EditStokCikis(obj).success(function (data) {
                            swal({
                                icon: "success",
                                title: "Başarılı!",
                                text: "Güncelleme işlemi başarılı.",
                            }, function (result) {
                                if (result) {
                                    $modalDetail.modal('hide');
                                }
                            });

                            ListData();
                            App.stopPageLoading();
                        }).error(function () {
                            swal({
                                icon: "error",
                                title: "Hata!",
                                text: "Güncelleme işlemi yapılamadı. Lütfen tekrar deneyin.",
                            });
                            App.stopPageLoading();
                        });
                    }
                    //}
                    //else {
                    //    swal({
                    //        icon: "warning",
                    //        title: "Detay Alanı Giriniz!",
                    //        text: "Detay alanına satır eklemeden kayıt yapılamaz!",
                    //    });
                    //}


                }
            });
    };

    $scope.GetStokDusumListe = function (itemNo, cikisAdet, ithalatciFirma) {
        var $form = $('#form-drop'),
            form = $form.controller('form'),
            input = $form.find('input[ng-required], select[ng-required], textarea[ng-required], div[ng-invalid-required]');;

        form.$submitted = false;
        $btnDropSubmit.prop('disabled', true);

        if (form.$valid) {
            angular.forEach(input, function (formElement, fieldName) {
                if ($(formElement).hasClass("ng-valid")) {
                    $(formElement).closest('.form-group').addClass('has-success').removeClass("has-error");
                    $(formElement).parent('.input-icon').children('i').removeClass('fa-warning').addClass("fa-check");
                }
            }, this);

            $gridDrop.beginCustomLoading();
            var data = $scope.object.Drop; // CHEP Depo Düşüm master verileri
            var stokCikisDetail = $gridDetail.getDataSource(); // Stok Çıkış modal satırları
            var deneme2 = $gridDrop.getDataSource(); // CHEP Depo Düşüm modal satırları

            var liste = [];
            for (var i = 0; i < stokCikisDetail._items.length; i++) {
                var id = parseInt(stokCikisDetail._items[i].stokCikisDetayId);
                if (isNaN(id) || stokCikisDetail._items[i].stokCikisDetayId.length > 10 || stokCikisDetail._items[i].stokCikisDetayId == 0) { // Guid ise veya 0 ise
                    stokCikisDetail._items[i].stokCikisDetayId = 0;
                    liste.push(stokCikisDetail._items[i]);
                }
            }
            if (liste.length > 0) {
                var obj = {
                    ChepStokCikisDetayList: liste,
                    itemNo: data.itemNo,
                    dropCount: parseInt(data.dropCount)
                }
                SparksXService.GetStokDusumListeAdd(obj).success(function (data) {
                    if (data.result != null) {
                        $gridDrop.option("dataSource", data.result);

                        $btnDropSubmit.prop('disabled', false);
                    }
                    if (data.message != null) {
                        swal({
                            icon: "error",
                            title: data.message,
                        });
                    }
                    $gridDrop.endCustomLoading();

                }).error(function (err) {
                    $gridDrop.endCustomLoading();
                });
            }
            else {
                SparksXService.GetStokDusumListe(itemNo, cikisAdet, ithalatciFirma).success(function (data) {
                    if (data.result != null) {
                        $gridDrop.option("dataSource", data.result);

                        $btnDropSubmit.prop('disabled', false);
                    }
                    if (data.message != null) {
                        swal({
                            icon: "error",
                            title: data.message,
                        });
                    }

                    $gridDrop.endCustomLoading();
                }).error(function (err) {
                    $gridDrop.endCustomLoading();
                });
            }
        } else {
            angular.forEach(input, function (formElement, fieldName) {
                if ($(formElement).hasClass("ng-invalid")) {
                    $(formElement).closest('.form-group').addClass('has-error').removeClass("has-success");
                    $(formElement).parent('.input-icon').children('i').removeClass('fa-check').addClass("fa-warning");
                }
            }, this);
        }
    };

    $scope.SetWorkOrderService = function (id) {
        SparksXService.GetStokCikis(id)
            .success(function (data) {

                if (data.invoiceId == null) {
                    swal({
                        icon: "warning",
                        title: "Fatura Id alanı boş. Tüm verileri kaydettiğinizden emin olunuz.",
                    });
                    return false;
                }
                if (data.invoiceNo == null) {
                    swal({
                        icon: "warning",
                        title: "Fatura No alanı boş. Tüm verileri kaydettiğinizden emin olunuz.",
                    });
                    return false;
                }
                if (data.invoiceDate == null) {
                    swal({
                        icon: "warning",
                        title: "Fatura Tarihi alanı boş. Tüm verileri kaydettiğinizden emin olunuz.",
                    });
                    return false;
                }
                if (data.ihracatciFirma == null) {
                    swal({
                        icon: "warning",
                        title: "İhracatcı Firma alanı boş. Tüm verileri kaydettiğinizden emin olunuz.",
                    });
                    return false;
                }
                if (data.invoiceCurrency == null) {
                    swal({
                        icon: "warning",
                        title: "Fatura Döviz Türü alanı boş. Tüm verileri kaydettiğinizden emin olunuz.",
                    });
                    return false;
                }
                if (data.invoiceAmount == null || data.invoiceAmount <= 0) {
                    swal({
                        icon: "warning",
                        title: "Fatura Tutar alanı 0'dan büyük olmalı. Tüm verileri kaydettiğinizden emin olunuz.",
                    });
                    return false;
                }
                if (data.odemeSekli == null) {
                    swal({
                        icon: "warning",
                        title: "Ödeme Şekli alanı boş. Tüm verileri kaydettiğinizden emin olunuz.",
                    });
                    return false;
                }
                if (data.aliciFirma == null) {
                    swal({
                        icon: "warning",
                        title: "Alıcı Firma alanı boş. Tüm verileri kaydettiğinizden emin olunuz.",
                    });
                    return false;
                }
                if (data.teslimSekli == null) {
                    swal({
                        icon: "warning",
                        title: "Teslim Şekli alanı boş. Tüm verileri kaydettiğinizden emin olunuz.",
                    });
                    return false;
                }
                if (data.kapCinsi == null) {
                    swal({
                        icon: "warning",
                        title: "Kap Cinsi alanı boş. Tüm verileri kaydettiğinizden emin olunuz.",
                    });
                    return false;
                }
                if (data.kapMiktari == null || data.kapMiktari <= 0) {
                    swal({
                        icon: "warning",
                        title: "Kap Adedi alanı 0'dan büyük olmalı. Tüm verileri kaydettiğinizden emin olunuz.",
                    });
                    return false;
                }

                for (var i = 0; i < data.chepStokCikisDetayList.length; i++) {
                    if (data.chepStokCikisDetayList[i].miktar == null || data.chepStokCikisDetayList[i].miktar <= 0) {
                        swal({
                            icon: "warning",
                            title: "Detayların birinde Miktar alanı  0'dan büyük olmalı. Tüm verileri kaydettiğinizden emin olunuz.",
                        });
                        return false;
                    }
                    if (data.chepStokCikisDetayList[i].invoiceAmount == null || data.chepStokCikisDetayList[i].invoiceAmount <= 0) {
                        swal({
                            icon: "warning",
                            title: "Detayların birinde Fatura Tutar alanı  0'dan büyük olmalı. Tüm verileri kaydettiğinizden emin olunuz.",
                        });
                        return false;
                    }
                    if (data.chepStokCikisDetayList[i].netKg == null || data.chepStokCikisDetayList[i].netKg <= 0) {
                        swal({
                            icon: "warning",
                            title: "Detayların birinde Net KG alanı  0'dan büyük olmalı. Tüm verileri kaydettiğinizden emin olunuz.",
                        });
                        return false;
                    }
                    if (data.chepStokCikisDetayList[i].invoiceDetailId == null) {
                        swal({
                            icon: "warning",
                            title: "Detayların birinde Fatura Detay Id alanı boş. Tüm verileri kaydettiğinizden emin olunuz.",
                        });
                        return false;
                    }
                }

                SparksXService.SetWorkOrderService(id)
                    .success(function (isEmriDurum) {
                        var obj = {
                            stokCikisId: data.stokCikisId,
                            isEmriDurum: isEmriDurum,
                        };
                        SparksXService.IsEmriDurumEditStokCikis(obj).success(function (updateData) {
                            $scope.object = updateData;

                        }).error(function () {
                            swal({
                                icon: "error",
                                title: "Hata!",
                                text: "İş emri gönderildi ancak durum güncelleme işlemi yapılamadı.",
                            });
                        });
                        swal({
                            icon: "successs",
                            title: isEmriDurum,
                        });
                    }).error(function (er) {
                        swal({
                            icon: "error",
                            title: er.message,
                        });
                    });

            }).error(function (error) {
                swal({
                    icon: "error",
                    title: error.message,
                });
            });



    };

    $scope.InsertStokCikisFromStokDusumListe = function (id, obj) {
        if (obj.itemNo == undefined || obj.dropCount == undefined) {
            alert('Ürün Kodu ve Çıkış Adet zorunludur!');
            return false;
        }
        var dsDrop = $gridDrop.getDataSource(),
            dsDetail = $gridDetail.getDataSource(),
            itemsDetail = dsDetail._items;

        $.each(dsDrop._items, function (index, elem) {
            var obj = $.extend({}, elem, {
                stokCikisDetayId: $.newguid(),
                miktar: elem.dusulenMiktar,
                invoiceAmount: elem.dusulenMiktar * elem.birimTutar,
            });
            if (elem.dusulenMiktar > 0) {
                itemsDetail.push(obj);
            }
        });

        $.each(dsDetail._items, function (index, elem) {
            elem.siraNo = index + 1;
        });

        $modalDrop.modal('hide');
        $gridDetail.option("dataSource", itemsDetail);
    }

    $.newguid = function () {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g,
            function (c) {
                var r = Math.random() * 16 | 0, v = c == 'x' ? r : r & 0x3 | 0x8;
                return v.toString(16);
            });
    };


    $scope.KiloDagit = function (obj) {
        SparksXService.GetStokCikis(obj).success(function (data) {
            $scope.object = data;

            var farkNet, farkBrut;

            //var kilo = 1000;
            var toplamMiktar = 0;
            var toplamNetKg = 0;
            //var netKilo = parseFloat(prompt("Net Kiloyu Yazınız", 1));
            var brutKilo = parseFloat(prompt("Brüt Kiloyu Yazınız", 1));
            //var roundNet = parseFloat(0);
            var roundBrut = parseFloat(0);

            for (var i in data.chepStokCikisDetayList) {
                var kalem = data.chepStokCikisDetayList[i];
                if (kalem.miktar == null) {
                    swal({
                        icon: "warning",
                        title: kalem.siraNo + " nolu kalemde miktar bilgisi girilmedi",
                    });
                    return;
                }

                toplamMiktar += parseFloat(kalem.miktar);
                toplamNetKg += parseFloat(kalem.netKg);
            }
            var detayKayitSayisi = data.chepStokCikisDetayList.length;
            debugger;
            for (var i in data.chepStokCikisDetayList) {
                var kalem = data.chepStokCikisDetayList[i];
                if (isNaN(toplamMiktar)) {
                    swal({
                        icon: "warning",
                        title: "miktar bilgisi girilmedi",
                    });
                    return;
                }
                if (brutKilo > 0) {
                    if (kalem.brutKg == null) {
                        swal({
                            icon: "warning",
                            title: kalem.siraNo + " nolu kalemde Brüt Kg bilgisi girilmedi",
                        });
                        return;
                    }
                    //Toplam Net KG - Yazılan Brüt Kilo / Detaydaki Kayıt Sayısı = birimmiktar + NEtKG her satır için
                    var netBrut = parseFloat(toplamNetKg) - (brutKilo).toFixed(2);
                    kalem.miktar = parseFloat(netBrut / (detayKayitSayisi).toFixed(2));
                    var birimMiktar = kalem.miktar;
                    kalem.brutKg = parseFloat((birimMiktar + kalem.netKg).toFixed(2));

                    //roundBrut += parseFloat(kalem.brutKg);
                    //console.log(i + ".kalemin brütKg değeri: " + brutKG); //TODO:
                }

                //if (netKilo > 0) {
                //    if (kalem.netKg == null) {
                //    if (kalem.netKg == null) {
                //        alert(kalem.siraNo + " nolu kalemde Net Ağırlık (Kg) bilgisi girilmedi");
                //        return;
                //    }
                //    kalem.netKg = parseFloat(((parseFloat(netKilo) * parseFloat(kalem.miktar)) / parseFloat(toplamMiktar)).toFixed(2));
                //    roundNet += parseFloat(kalem.netKg);
                //    //console.log(i + ". kalemin netKg değeri: " + netKg); //TODO:
                //}
            }

            SparksXService.EditStokCikis(data).success(function () {
                swal({
                    icon: "success",
                    title: "Başarılı!",
                    text: "Kilo dağıtıldı!",
                });
                SparksXService.GetStokCikis(obj).success(function (updateData) {

                    $gridDetail.option("dataSource", updateData.chepStokCikisDetayList);
                });
            }).error(function () {
                swal({
                    icon: "error",
                    title: "Hata!",
                    text: "Güncelleme işlemi yapılamadı. Lütfen tekrar deneyin.",
                });
            });

            //farkNet = parseFloat((parseFloat(netKilo) - roundNet)).toFixed(2);
            farkBrut = parseFloat((parseFloat(brutKilo - roundBrut))).toFixed(2);

            //console.log("farkNet: ", farkNet);

            //if (farkBrut != 0 || farkNet != 0) {
            //    swal({
            //        icon: "success",
            //        title: "İşlem Bitti!",
            //        text: /*'Net Kg : ' + farkNet.toString() + */' Brut Kg : ' + farkBrut.toString() + ' fark ile Dağıtılmıştır. Kalemlere Farkı El ile dağıtınız. (Eksi Değerli Farkı Uygun Kalemden Eksiltiniz.)',
            //    });
            //}
        });

    };


}]);