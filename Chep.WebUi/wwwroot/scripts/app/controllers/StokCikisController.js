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
        if ($modalDetail == null) {
            $btnArchive = $('#btnArchive');
            $btnJobOrder = $('#btnJobOrder');

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

                    $gridDetail.option("dataSource", []);
                    $gridDetail.cancelEditData();

                    DeletedChepStokCikisDetayIdList = [];

                    $modalDetail.find('form .form-group').removeClass('has-success').removeClass('has-error');
                    $modalDetail.find('form .form-control').removeClass('ng-valid').removeClass('ng-invalid');
                    $modalDetail.find('form .input-icon .fa').removeClass('fa-check').removeClass('fa-warning');

                    $btnArchive.addClass('hidden');
                    $btnJobOrder.addClass('hidden');
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
                            dataField: "stokGirisDetayId", caption: "Stok Girişi Beyanname No",
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
                                    dropDownOptions: { width: 600 },
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
                                            columns: [
                                                {
                                                    dataField: "tpsSiraNo", caption: "TPS Sıra No", dataType: "number",
                                                    format: { type: "fixedPoint", precision: 0 },
                                                },
                                                { dataField: "tpsCikisSiraNo", caption: "TPS Çıkış Sıra No", },
                                                { dataField: "tpsBeyan", caption: "TPS Beyan", },
                                                { dataField: "faturaNo", caption: "Fatura No", },
                                                { dataField: "faturaTarih", caption: "Fatura Tarihi", dataType: "date", formatType: "shortDate" },
                                                {
                                                    dataField: "faturaTutar", caption: "Fatura Tutar", dataType: "number",
                                                    format: { type: "fixedPoint", precision: 2 },
                                                },
                                                { dataField: "faturaDovizKod", caption: "Fatura Döviz Kod", },

                                            ],
                                            hoverStateEnabled: true,
                                            scrolling: { mode: "virtual" },
                                            height: 250,
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
                            dataField: "tpsCikisSiraNo", caption: "TPS Çıkış Sıra No", dataType: "number", width: 130,
                            format: { type: "fixedPoint", precision: 0 }
                        },
                        {
                            dataField: "miktar", caption: "Miktar", dataType: "number",
                            format: { type: "fixedPoint", precision: 0 },
                        },
                        {
                            dataField: "kg", caption: "Kg", dataType: "number",
                            format: { type: "fixedPoint", precision: 2 },
                        },
                        {
                            dataField: "invoiceAmount", caption: "Fatura Tutar", dataType: "number",
                            format: { type: "fixedPoint", precision: 0 },
                        },
                        {
                            dataField: "invoiceDetailId", caption: "Fatura Detay Id", dataType: "text", width: 130,
                            format: { type: "fixedPoint", precision: 0 }, allowEditing: false,
                        },
                    ],
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
                        { dataField: "girisBeyannameNo", caption: "Beyanname No", width: 150, },
                        { dataField: "tpsNo", caption: "TPS No", width: 150, },
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
                    { dataField: "tpsNo", caption: "TPS No", },
                    { dataField: "islemTarihi", caption: "İşlem Tarihi", dataType: "date", formatType: "shortDate" },
                    { dataField: "tpsTarih", caption: "TPS Tarihi", dataType: "date", formatType: "shortDate" },
                    { dataField: "beyannameNo", caption: "Beyanname No", },
                    { dataField: "beyannameTarihi", caption: "Beyanname Tarihi", dataType: "date", formatType: "shortDate" },
                    { dataField: "ihracatciFirmaName", caption: "İhracatci Firma", },
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

                            e.items.push({
                                text: "Excel'den Al",
                                icon: "exportxlsx",
                                onItemClick: function () {
                                    $modalImport.modal('show');
                                }
                            });
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
        var tpsNo = $scope.filter.tpsNo ? $scope.filter.tpsNo : "";

        SparksXService.ListStokCikises(referansNo, beyannameNo, tpsNo).success(function (data) {
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
        SparksXService.GetCurrencyTypes().success(function (data) {
            $scope.currencytypes = data;
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

    $scope.GetStokDusumListe = function (itemNo, cikisAdet) {
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

            SparksXService.GetStokDusumListe(itemNo, cikisAdet).success(function (data) {
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
        } else {
            angular.forEach(input, function (formElement, fieldName) {
                if ($(formElement).hasClass("ng-invalid")) {
                    $(formElement).closest('.form-group').addClass('has-error').removeClass("has-success");
                    $(formElement).parent('.input-icon').children('i').removeClass('fa-check').addClass("fa-warning");
                }
            }, this);
        }
    };

    $scope.InsertStokCikisFromStokDusumListe = function (id, obj) {
        if (obj.itemNo == undefined || obj.dropCount == undefined) {
            alert('Ürün Kodu ve Çıkış Adet zorunludur!');
            return false;
        }

        if (id > 0) {
            SparksXService.InsertStokCikisFromStokDusumListe(id, obj.itemNo, obj.dropCount).success(function (data) {
                if (data.result) {
                    swal({
                        icon: "success",
                        title: "İşlem başarlı!",
                    }, function () {
                        $modalDrop.modal('hide');
                    });
                }
            });
        } else {
            var dsDrop = $gridDrop.getDataSource(),
                dsDetail = $gridDetail.getDataSource(),
                itemsDetail = dsDetail._items;

            $.each(dsDrop._items, function (index, elem) {
                var obj = $.extend({}, elem, {
                    stokCikisDetayId: $.newguid(),
                    miktar: elem.dusulenMiktar,
                });

                itemsDetail.push(obj);
            });

            $modalDrop.modal('hide');
            $gridDetail.option("dataSource", itemsDetail);
        }
    }

    $.newguid = function () {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g,
            function (c) {
                var r = Math.random() * 16 | 0, v = c == 'x' ? r : r & 0x3 | 0x8;
                return v.toString(16);
            });
    };

}]);