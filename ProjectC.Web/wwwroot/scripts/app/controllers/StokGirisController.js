angular.module('SparksXApp').controller('StokGirisController', ['$rootScope', '$state', '$stateParams', '$scope', 'settings', 'SparksXService', '$timeout', 'Upload', function ($rootScope, $state, $stateParams, $scope, settings, SparksXService, $timeout, Upload) {

    DevExpress.localization.locale("tr-TR");

    var $gridContainer = null;
    var $gridDetail = null;
    var $modalDetail = null;
    var DeletedChepStokGirisDetayIdList = [];

    $scope.$on('$viewContentLoaded', function () {
        App.initAjax();
        $rootScope.settings.layout.pageContentWhite = true;
        $rootScope.settings.layout.pageBodySolid = false;
        $rootScope.settings.layout.pageSidebarClosed = false;

    });

    $scope.Modal = function () {
        if ($modalDetail == null) {
            $modalDetail = $('#modalDetail').on({
                "hidden.bs.modal": function () {
                    $scope.object = {};
                    $scope.$apply();

                    $gridDetail.option("dataSource", []);
                    $gridDetail.cancelEditData();

                    DeletedChepStokGirisDetayIdList = [];
                }
            }).modal({
                show: false,
                keyboard: false
            });

            if ($gridDetail == null) {
                $gridDetail = $("#gridDetail").dxDataGrid({
                    keyExpr: "StokGirisDetayId",
                    dataSource: [],
                    columns: [
                        {
                            dataField: "TPSSiraNo", caption: "TPS Sıra No", dataType: "number",
                            format: { type: "fixedPoint", precision: 0 },
                        },
                        { dataField: "TPSBeyan", caption: "TPS Beyan", },
                        { dataField: "EsyaCinsi", caption: "Eşya Cinsi", },
                        { dataField: "EsyaGTIP", caption: "Eşya GTİP", },
                        { dataField: "FaturaNo", caption: "Fatura No", },
                        { dataField: "FaturaTarih", caption: "Fatura Tarihi", dataType: "date", formatType: "shortDate" },
                        {
                            dataField: "FaturaTutar", caption: "Fatura Tutar", dataType: "number",
                            format: { type: "fixedPoint", precision: 2 },
                        },
                        { dataField: "FaturaDovizKod", caption: "Fatura Döviz Kod", },
                        {
                            dataField: "Miktar", caption: "Miktar", dataType: "number",
                            format: { type: "fixedPoint", precision: 0 },
                        },
                        { dataField: "OlcuBirimi", caption: "Ölçü Birimi", },
                        { dataField: "Rejim", caption: "Rejim", },
                        { dataField: "CikisRejimi", caption: "Çıkış Rejimi", },
                        { dataField: "GidecegiUlke", caption: "Gideceği Ülke", },
                        { dataField: "MenseUlke", caption: "Menşei Ülke", },
                        { dataField: "SozlesmeUlke", caption: "Sözleşme Ülke", },
                        { dataField: "Marka", caption: "Marka", },
                        { dataField: "Model", caption: "Model", },
                        { dataField: "UrunKod", caption: "Ürün Kodu", },
                        { dataField: "PO", caption: "PO", },
                    ],
                    onContextMenuPreparing: function (e) {
                        if (e.target == "content") {
                            if (!e.items) e.items = [];

                            if ($rootScope.user.UserPermissions.StokGirisAdd) {
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
                            DeletedChepStokGirisDetayIdList.push(e.key);
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
    }

    // CRUD
    $scope.Grid = function () {

        if ($gridContainer == null) {
            $gridContainer = $("#gridContainer").dxDataGrid({
                keyExpr: "StokGirisId",
                dataSource: [],
                columns: [
                    { dataField: "ReferansNo", caption: "Referans No", },
                    { dataField: "TPSNo", caption: "TPS No", },
                    { dataField: "TPSDurum", caption: "TPS Durum", },
                    { dataField: "BasvuruTarihi", caption: "Basvuru Tarihi", dataType: "date", formatType: "shortDate" },
                    { dataField: "SureSonuTarihi", caption: "Süre Sonu Tarihi", dataType: "date", formatType: "shortDate" },
                    { dataField: "GumrukKod", caption: "Gumruk Kodu", },
                    { dataField: "BeyannameNo", caption: "Beyanname No", },
                    { dataField: "BeyannameTarihi", caption: "Beyanname Tarihi", dataType: "date", formatType: "shortDate" },
                    { dataField: "BelgeAd", caption: "Belge Ad", },
                    { dataField: "BelgeSart", caption: "Belge Şart", },
                    { dataField: "TPSAciklama", caption: "TPS Açıklama", },
                    { dataField: "IthalatciFirma", caption: "İthalatçı Firma", },
                    { dataField: "IhracatciFirma", caption: "İhracatçı Firma", },
                    { dataField: "KapAdet", caption: "Kap Adet", },
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
                            saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'StokGiris.xlsx');
                        });
                    });
                    e.cancel = true;
                },
                onContextMenuPreparing: function (e) {
                    if (e.target == "content") {
                        if (!e.items) e.items = [];

                        if ($rootScope.user.UserPermissions.StokGirisAdd) {
                            e.items.push({
                                text: "Ekle",
                                icon: "add",
                                onItemClick: function () {
                                    $scope.object.StokGirisId = 0;
                                    $modalDetail.modal('show');
                                }
                            });

                            e.items.push({
                                text: "Excel'den Al",
                                icon: "exportxlsx",
                                onItemClick: function () {
                                    $modalDetail.modal('show');
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
                    SparksXService.GetStokGiris(e.key).success(function (data) {
                        $scope.object = data;

                        $gridDetail.option("dataSource", data.ChepStokGirisDetayList);

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

        $gridContainer.beginCustomLoading();

        ListData();
    }

    function ListData() {
        SparksXService.ListStokGirises().success(function (data) {
            $gridContainer.option("dataSource", data);
            $gridContainer.endCustomLoading();
        }).error(function () {
            $gridContainer.option("dataSource", []);
            $gridContainer.endCustomLoading();
        });
    }


    $scope.List = function () {
        $rootScope.settings.layout.pageSidebarClosed = false;
        SparksXService.ListStokGirises().success(function (data) {
            $scope.list = data;
        });
    }

    $scope.Get = function () {
        SparksXService.GetStokGiris($stateParams.id).success(function (data) {
            $scope.object = data;
        });
    };

    $scope.BindFields = function () {
        $scope.object = {};

        //if ($rootScope.SelectedCustomerId == undefined) {
        //} else {
        //    $scope.object.CustomerId = $rootScope.SelectedCustomerId;
        //    $scope.hasCustomerSelected = true;
        //}

        //SparksXService.GetCustomers().success(function (data) {
        //    $scope.customers = data;

        //    SparksXService.ListStokGirises($scope.object.CustomerId).success(function (data) {
        //        $scope.list = data;
        //    });
        //});
    };

    $scope.Action = function (obj) {
        App.startPageLoading();

        var ds = $gridDetail.getDataSource();

        for (var i = 0; i < ds._items.length; i++) {
            if (!Number.isInteger(ds._items[i].StokGirisDetayId)) {
                ds._items[i].StokGirisDetayId = 0;
            }
        }
        obj.ChepStokGirisDetayList = ds._items;
        obj.DeletedChepStokGirisDetayIdList = DeletedChepStokGirisDetayIdList;

        $gridDetail
            .getController('validating')
            .validate()
            .then(function (validGrid) {
                if (validGrid) {

                    if (obj.StokGirisId == 0) {
                        // Insert
                        SparksXService.AddStokGiris(obj).success(function (data) {
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
                        SparksXService.EditStokGiris(obj).success(function (data) {
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

                }
            });
    };
}]);