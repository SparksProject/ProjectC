angular.module('SparksXApp').controller('StokGirisController', ['$rootScope', '$state', '$stateParams', '$scope', 'settings', 'SparksXService', '$timeout', 'Upload', function ($rootScope, $state, $stateParams, $scope, settings, SparksXService, $timeout, Upload) {

    DevExpress.localization.locale("tr-TR");

    var $gridContainer = null;
    var $gridDetail = null;
    var $modalDetail = null;
    var $modalImport = null;
    var DeletedChepStokGirisDetayIdList = [];

    var storeCountries = new DevExpress.data.CustomStore({
        key: "ediCode",
        method: "Get",
        loadMode: "raw", // omit in the DataGrid, TreeList, PivotGrid, and Scheduler
        load: function () {
            var deferred = $.Deferred();

            SparksXService.GetCountries().success(function (data) {
                deferred.resolve(data);
            }).error(function () {
                deferred.reject("Data Loading Error");
            });

            return deferred.promise();
        }
    });

    var storeCurrencyTypes = new DevExpress.data.CustomStore({
        key: "ediCode",
        method: "Get",
        loadMode: "raw", // omit in the DataGrid, TreeList, PivotGrid, and Scheduler
        load: function () {
            var deferred = $.Deferred();

            SparksXService.GetCurrencyTypes().success(function (data) {
                deferred.resolve(data);
            }).error(function () {
                deferred.reject("Data Loading Error");
            });

            return deferred.promise();
        }
    });

    var storeProducts = new DevExpress.data.CustomStore({
        key: "productNo",
        method: "Get",
        loadMode: "raw", // omit in the DataGrid, TreeList, PivotGrid, and Scheduler
        load: function () {
            var deferred = $.Deferred();

            SparksXService.GetProducts().success(function (data) {
                deferred.resolve(data);
            }).error(function () {
                deferred.reject("Data Loading Error");
            });

            return deferred.promise();
        }
    });

    var storeUnits = new DevExpress.data.CustomStore({
        key: "ediCode",
        method: "Get",
        loadMode: "raw", // omit in the DataGrid, TreeList, PivotGrid, and Scheduler
        load: function () {
            var deferred = $.Deferred();

            SparksXService.GetUnits().success(function (data) {
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
            $modalDetail = $('#modalDetail').on({
                "hidden.bs.modal": function () {
                    $scope.object = {};
                    $scope.$apply();

                    $gridDetail.option("dataSource", []);
                    $gridDetail.cancelEditData();

                    DeletedChepStokGirisDetayIdList = [];

                    $modalDetail.find('form .form-group').removeClass('has-success').removeClass('has-error');
                    $modalDetail.find('form .form-control').removeClass('ng-valid').removeClass('ng-invalid');
                    $modalDetail.find('form .input-icon .fa').removeClass('fa-check').removeClass('fa-warning');
                },
                "shown.bs.modal": function () {
                    SparksXService.GetNextReferenceNumber('Giris').success(function (data) {
                        $scope.object.referansNo = data;
                    });
                }
            }).modal({
                show: false,
                keyboard: false
            });

            if ($gridDetail == null) {
                $gridDetail = $("#gridDetail").dxDataGrid({
                    keyExpr: "stokGirisDetayId",
                    dataSource: [],
                    columns: [
                        {
                            dataField: "tpsSiraNo", caption: "TPS Sıra No", dataType: "number", width: 100,
                            format: { type: "fixedPoint", precision: 0 }, allowEditing: false,
                        },
                        {
                            dataField: "tpsCikisSiraNo", caption: "TPS Çıkış Sıra No", dataType: "number", width: 120,
                        },
                        {
                            dataField: "beyannameKalemNo", caption: "Beyanname Kalem No", dataType: "number", width: 150,
                        },
                        { dataField: "tpsBeyan", caption: "TPS Beyan", width: 100, },
                        {
                            dataField: "urunKod", caption: "Ürün Kodu", width: 170,
                            lookup: {
                                dataSource: storeProducts, // Edit aşamasında kolonda SelectBox oluşturulur ve tanımlanan kaynaktan ajax get veri alır.
                                displayExpr: "productNo", // Dönen veride basılacak metin
                                valueExpr: "productNo", // Dönen veride basılacak value
                            },
                        },
                        { dataField: "esyaGtip", caption: "Eşya GTİP", width: 120, },
                        { dataField: "esyaCinsi", caption: "Eşya Cinsi", width: 120, },
                        { dataField: "faturaNo", caption: "Fatura No", width: 100, },
                        { dataField: "faturaTarih", caption: "Fatura Tarihi", width: 100, dataType: "date", formatType: "shortDate" },
                        {
                            dataField: "faturaTutar", caption: "Fatura Tutar", width: 100, dataType: "number",
                            format: { type: "fixedPoint", precision: 2 },
                        },
                        {
                            dataField: "faturaDovizKod", caption: "Fatura Döviz Kod", width: 140,
                            lookup: {
                                dataSource: storeCurrencyTypes, // Edit aşamasında kolonda SelectBox oluşturulur ve tanımlanan kaynaktan ajax get veri alır.
                                displayExpr: "ediCode", // Dönen veride basılacak metin
                                valueExpr: "ediCode", // Dönen veride basılacak value
                            },
                        },
                        {
                            dataField: "miktar", caption: "Miktar", dataType: "number", width: 100,
                            format: { type: "fixedPoint", precision: 0 },
                        },
                        {
                            dataField: "olcuBirimi", caption: "Ölçü Birimi", width: 200,
                            lookup: {
                                dataSource: storeUnits, // Edit aşamasında kolonda SelectBox oluşturulur ve tanımlanan kaynaktan ajax get veri alır.
                                displayExpr: "unitsName", // Dönen veride basılacak metin
                                valueExpr: "ediCode", // Dönen veride basılacak value
                            },
                        },
                        { dataField: "rejim", caption: "Rejim", width: 90, },
                        { dataField: "cikisRejimi", caption: "Çıkış Rejimi", width: 100, },
                        {
                            dataField: "gidecegiUlke",
                            caption: "Gideceği Ülke",
                            width: 200,
                            lookup: {
                                dataSource: storeCountries, // Edit aşamasında kolonda SelectBox oluşturulur ve tanımlanan kaynaktan ajax get veri alır.
                                displayExpr: function (data) {
                                    return data.ediCode + " - " + data.countryName;
                                }, // Dönen veride basılacak metin
                                valueExpr: "ediCode", // Dönen veride basılacak value
                            },
                        },
                        {
                            dataField: "menseUlke",
                            caption: "Menşei Ülke",
                            width: 200,
                            lookup: {
                                dataSource: storeCountries, // Edit aşamasında kolonda SelectBox oluşturulur ve tanımlanan kaynaktan ajax get veri alır.
                                displayExpr: function (data) {
                                    return data.ediCode + " - " + data.countryName;
                                }, // Dönen veride basılacak metin
                                valueExpr: "ediCode", // Dönen veride basılacak value
                            },
                        },
                        {
                            dataField: "sozlesmeUlke",
                            caption: "Sözleşme Ülke",
                            width: 200,
                            lookup: {
                                dataSource: storeCountries, // Edit aşamasında kolonda SelectBox oluşturulur ve tanımlanan kaynaktan ajax get veri alır.
                                displayExpr: function (data) {
                                    return data.ediCode + " - " + data.countryName;
                                }, // Dönen veride basılacak metin
                                valueExpr: "ediCode", // Dönen veride basılacak value
                            },
                        },
                        { dataField: "marka", caption: "Marka", width: 100, },
                        { dataField: "model", caption: "Model", width: 100, },
                        { dataField: "poNo", caption: "PO", width: 100, },
                    ],
                    onContextMenuPreparing: function (e) {
                        if (e.target == "content") {
                            if (!e.items) e.items = [];

                            if ($rootScope.user.userPermissions.stokGirisAdd) {
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
                    onInitNewRow: function (e) {
                        var ds = e.component.getDataSource();

                        if (ds._items.length) {
                            e.data.tpsSiraNo = $(ds._items).last()[0].tpsSiraNo + 1;
                        } else {
                            e.data.tpsSiraNo = 1;
                        }

                        if (ds._items.length) {
                            e.data.tpsCikisSiraNo = $(ds._items).last()[0].tpsCikisSiraNo + 1;
                        } else {
                            e.data.tpsCikisSiraNo = 2;
                        }
                    },
                    onEditorPreparing: function (e) {//her değişiklikte her satıra bakar 
                        //Stok Giriş Detay Kısmında Ürün kodu Seçildiğinde Eşya Gitp,Eşya cinsi otomatik Doldurulma İşlemi
                        if (e.dataField == "urunKod") {//eğer satır urunKod ise
                            e.editorOptions.onValueChanged = function (args) {
                                e.setValue(args.value);

                                var ds = args.element.dxSelectBox("instance").getDataSource(),
                                    items = ds._items,
                                    selectedData = $(items).filter(function (i, el) {
                                        return el.productNo == args.value;
                                    });

                                e.component.cellValue(e.row.rowIndex, "esyaGtip", selectedData[0].hsCode);
                                e.component.cellValue(e.row.rowIndex, "esyaCinsi", selectedData[0].productNameTr);

                            }
                        } 
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
        $scope.filter = {};

        if ($gridContainer == null) {
            $gridContainer = $("#gridContainer").dxDataGrid({
                keyExpr: "stokGirisId",
                dataSource: [],
                columns: [
                    { dataField: "referansNo", caption: "Referans No", width: 120 },
                    { dataField: "tpsNo", caption: "TPS No", width: 200 },
                    { dataField: "tpsDurum", caption: "TPS Durum", width: 200 },
                    { dataField: "basvuruTarihi", caption: "Basvuru Tarihi", dataType: "date", formatType: "shortDate" },
                    { dataField: "sureSonuTarihi", caption: "Süre Sonu Tarihi", dataType: "date", formatType: "shortDate" },
                    { dataField: "gumrukKod", caption: "Gumruk Kodu", },
                    { dataField: "beyannameNo", caption: "Beyanname No", },
                    { dataField: "beyannameTarihi", caption: "Beyanname Tarihi", dataType: "date", formatType: "shortDate" },
                    { dataField: "belgeAd", caption: "Belge Ad", },
                    { dataField: "belgeSart", caption: "Belge Şart", },
                    { dataField: "tpsAciklama", caption: "TPS Açıklama", },
                    { dataField: "ithalatciFirmaName", caption: "İthalatçı Firma", width: 200 },
                    { dataField: "ihracatciFirmaName", caption: "İhracatçı Firma", width: 200 },
                    { dataField: "kapAdet", caption: "Kap Adet", },
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

                        if ($rootScope.user.userPermissions.stokGirisAdd) {
                            e.items.push({
                                text: "Ekle",
                                icon: "add",
                                onItemClick: function () {
                                    $scope.object.stokGirisId = 0;
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
                    SparksXService.GetStokGiris(e.key).success(function (data) {
                        $scope.object = data;

                        $gridDetail.option("dataSource", data.chepStokGirisDetayList);

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

        //$gridContainer.beginCustomLoading();

        //ListData();
    }

    $scope.ModalImport = function () {
        if ($modalImport == null) {
            $modalImport = $('#modalImport').on({
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

        SparksXService.ListStokGirises(referansNo, beyannameNo, tpsNo).success(function (data) {
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
    };

    $scope.Action = function (obj) {
        if (obj.basvuruTarihi == "") {
            obj.basvuruTarihi = null;
        }

        if (obj.beyannameTarihi == "") {
            obj.beyannameTarihi = null;
        }

        if (obj.sureSonuTarihi == "") {
            obj.sureSonuTarihi = null;
        }

        obj.referansNo = parseInt(obj.referansNo);

        App.startPageLoading();

        var ds = $gridDetail.getDataSource();

        for (var i = 0; i < ds._items.length; i++) {
            if (!Number.isInteger(ds._items[i].stokGirisDetayId)) {
                ds._items[i].stokGirisDetayId = 0;
            }
        }
        obj.chepStokGirisDetayList = ds._items;
        obj.deletedChepStokGirisDetayIdList = DeletedChepStokGirisDetayIdList;

        $gridDetail
            .getController('validating')
            .validate()
            .then(function (validGrid) {
                if (validGrid) {
                    console.log(obj);
                    if (obj.chepStokGirisDetayList.length > 0) {
                        if (obj.stokGirisId == 0) {
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
                    else {
                        swal({
                            icon: "warning",
                            title: "Detay Alanı Giriniz!",
                            text: "Detay alanına satır eklemeden kayıt yapılamaz!",
                        });
                    }


                }
            });
    };

    $scope.UploadFile = function (obj) {
        Upload.upload({
            url: $rootScope.settings.serverPath + '/api/StokGiris/Import/?userId=' + $rootScope.user.userId,
            data: {},
            file: obj.ExcelFile
        }).success(function (data) {
            swal({
                icon: "success",
                title: "Başarılı!",
                text: "Excel'den veri yükleme işlemi başarılı. " + data,
            });

        }).then(function (response) {
            $timeout(function () {
                $state.go('stokgiris/list');
            });
        });
    }

    //// UploadFile
    //$scope.UploadFile = function (obj) {
    //    $scope.HasMessage = false;
    //    $scope.ResultMessage = undefined;
    //    //console.log($scope.object.ExcelFile);-- obj.ExcelFile Aynı şey
    //    if (obj.ExcelFile == null) {
    //        return false;
    //    }

    //    var reader = new FileReader();
    //    reader.readAsBinaryString(obj.ExcelFile);
    //    reader.onload = function () {
    //        var files = btoa(reader.result);

    //        SparksXService.ImportStokGiris(files).success(function (data) {
    //            if (data.Message != null && data.Message != undefined && data.Message.length > 0) {
    //                $scope.ResultMessage = data.Message;
    //                $scope.HasMessage = true;

    //                var content = '<button class="close" data-close="alert"></button>';
    //                content += data.Message;

    //                $('#divMessage').html(content).removeClass('display-none');
    //            } else {
    //                $state.go('stokgiris/list');
    //            }
    //        }).error(function () {
    //            $scope.ResultMessage = "HATA";
    //            $scope.HasMessage = true;
    //        });
    //    };
    //    reader.onerror = function () {
    //        console.log("error");
    //        return false;
    //    };
    //};

}]);