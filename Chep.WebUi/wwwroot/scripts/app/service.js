SparksXApp.service("SparksXService", function ($rootScope, $http) {

    this.EditHelper = function (obj) {

        var userId = $rootScope.user.EmployeeId;

        if (obj.RecordStatusId == 1) {
            obj.ModifiedBy = userId;
        }
        else {
            obj.DeletedBy = userId;
        }

        return obj;
    }

    this.AddHelper = function (obj) {

        obj.CreatedBy = $rootScope.user.EmployeeId;

        return obj;
    };

    // ====================== Helpers ======================

    function GetDefinitionRequestJson(definitionName) {
        return GetBaseRequestJson('Definition', definitionName);
    }

    function GetDefinitionRequestJsonById(definitionName, id) {
        return GetBaseRequestJson('Definition', definitionName, id);
    }

    function GetBaseRequestJson(baseName, definitionName, id, queryString) {
        var additionalString = '';

        if (queryString != undefined) {
            additionalString += "?";
            if (id != undefined) {
                additionalString += "id=" + id + "&";
            }
            additionalString += queryString;
        } else if (id != undefined) {
            additionalString = '/' + id;
        }

        return $http({
            url: $rootScope.settings.serverPath + '/api/' + baseName + '/' + definitionName + additionalString,
            method: 'GET',
            contentType: "application/json"
        });
    }

    function GetBaseRequestPostJson(baseName, definitionName, obj) {
        return $http({
            url: $rootScope.settings.serverPath + '/api/' + baseName + '/' + definitionName,
            method: "POST",
            data: obj,
            dataType: "json",
        });
    }


    //============================ Definitions ============================>

    this.GetRecordStatuses = function () {
        return GetDefinitionRequestJson('GetRecordStatuses');
    };

    this.GetCustomers = function () {
        return GetDefinitionRequestJson('GetCustomers');
    };

    this.GetEmployees = function () {
        return GetDefinitionRequestJson('GetEmployees');
    };

    this.GetPeriodTypes = function () {
        return GetDefinitionRequestJson('GetPeriodTypes');
    };

    this.GetMailDefinitions = function () {
        return GetDefinitionRequestJson('GetMailDefinitions');
    };

    this.GetUserTypes = function () {
        return GetDefinitionRequestJson('GetUserTypes');
    };

    this.GetUsers = function () {
        return GetDefinitionRequestJson('GetUsers');
    };

    this.GetCurrencyTypes = function () {
        return GetDefinitionRequestJson('GetCurrencyTypes');
    };

    this.GetDeliveryTerms = function () {
        return GetDefinitionRequestJson('GetDeliveryTerms');
    };

    this.GetPaymentMethods = function () {
        return GetDefinitionRequestJson('GetPaymentMethods');
    };

    this.GetCustoms = function () {
        return GetDefinitionRequestJson('GetCustoms');
    };

    this.GetParameterTypes = function () {
        return GetDefinitionRequestJson('GetParameterTypes');
    };

    this.GetCountries = function () {
        return GetDefinitionRequestJson('GetCountries');
    };

    this.GetProducts = function () {
        return GetDefinitionRequestJson('GetProducts');
    };

    this.GetUnits = function () {
        return GetDefinitionRequestJson('GetUnits');
    };

    this.GetNextReferenceNumber = function (id) {
        return GetDefinitionRequestJsonById('GetNextReferenceNumber', id);
    };

    //Login========================>

    this.Authenticate = function (user) {

        var response = $http({
            url: $rootScope.settings.serverPath + '/api/Account/Authenticate',
            method: 'post',
            data: user,
        });

        return response;
    }

    this.IsAuthenticated = function () {
        return GetBaseRequestJson("Account", "IsAuthenticated");
    };

    //User======================>

    this.ListUsers = function () {
        return GetBaseRequestJson("Account", "List");
    };

    this.GetUser = function (id) {
        return GetBaseRequestJson("Account", "GetUser", id);
    };

    this.AddUser = function (obj) {
        return GetBaseRequestPostJson("Account", "Add", obj);
    };

    this.Edit = function (obj) {
        return GetBaseRequestPostJson("Account", "Edit", obj);
    };

    //Company======================>

    this.GetCompany = function (id) {
        return GetBaseRequestJson("Company", "Get", id);
    };

    this.EditCompany = function (obj) {
        return GetBaseRequestPostJson("Company", "Edit", obj);
    };

    //Dashboard========================>  


    this.GetDashboardDetails = function (id) {
        return GetDefinitionRequestJsonById("GetDashboardDetails", id);
    };

    //MailDefinition======================>

    this.ListMailDefinitions = function () {
        return GetBaseRequestJson("MailReport", "GetMailDefinitions");
    };

    this.AddMailDefinition = function (obj) {
        return GetBaseRequestPostJson("MailReport", "AddMailDefinition", obj);
    };

    //Customer======================>    

    this.ListCustomers = function () {
        return GetBaseRequestJson("Customer", "List");
    };

    this.GetCustomer = function (id) {
        return GetBaseRequestJson("Customer", "Get", id);
    };

    this.AddCustomer = function (obj) {
        return GetBaseRequestPostJson("Customer", "Add", obj);
    };

    this.EditCustomer = function (obj) {
        return GetBaseRequestPostJson("Customer", "Edit", obj);
    };

    //Product======================>

    this.ListProducts = function (id) {
        return GetBaseRequestJson("Product", "List", id);
    };

    this.GetProduct = function (id) {
        return GetBaseRequestJson("Product", "Get", id);
    };

    this.AddProduct = function (obj) {
        return GetBaseRequestPostJson("Product", "Add", obj);
    };

    this.EditProduct = function (obj) {
        return GetBaseRequestPostJson("Product", "Edit", obj);
    };

    this.PostExcel = function (obj) {
        return GetBaseRequestPostJson("Product", "PostExcel", obj);
    };

    //MailReport======================>    

    this.ListMailReports = function (id) {
        return GetBaseRequestJson("MailReport", "List", id);
    };

    this.GetMailReport = function (id) {
        return GetBaseRequestJson("MailReport", "Get", id);
    };

    this.AddMailReport = function (obj) {
        console.log(obj);
        return GetBaseRequestPostJson("MailReport", "Add", obj);
    };

    this.EditMailReport = function (obj) {
        return GetBaseRequestPostJson("MailReport", "Edit", obj);
    };

    this.GetMailResultSet = function (id, userId) {
        return GetBaseRequestJson("MailReport", "GetMailResultSet", id, 'userId=' + userId);
    };

    //GenericReport======================>    

    this.ListGenericReports = function (id) {
        return GetBaseRequestJson("GenericReport", "List", id);
    };

    this.GetGenericReport = function (id) {
        return GetBaseRequestJson("GenericReport", "Get", id);
    };

    this.CreateExcel = function (obj) {
        return GetBaseRequestPostJson("GenericReport", "CreateExcel", obj);
    };

    this.GetResultSet = function (obj) {
        return GetBaseRequestPostJson("GenericReport", "GetResultSet", obj);
    };

    this.AddGenericReport = function (obj) {
        return GetBaseRequestPostJson("GenericReport", "Add", obj);
    };

    this.EditGenericReport = function (obj) {
        return GetBaseRequestPostJson("GenericReport", "Edit", obj);
    };

    //Stok Giris======================>

    this.ListStokGirises = function (ReferansNo, BeyannameNo, TPSNo) {
        return GetBaseRequestJson("StokGiris", "List", undefined, 'referansNo=' + ReferansNo + '&beyannameNo=' + BeyannameNo + '&tpsNo=' + TPSNo);
    };

    this.ListStokGirisDetails = function (id) {
        return GetBaseRequestJson("StokGiris", "ListDetails");
    };

    this.GetStokGiris = function (id) {
        return GetBaseRequestJson("StokGiris", "Get", id);
    };

    this.AddStokGiris = function (obj) {
        return GetBaseRequestPostJson("StokGiris", "Add", obj);
    };

    this.EditStokGiris = function (obj) {
        return GetBaseRequestPostJson("StokGiris", "Edit", obj);
    };

    this.ImportStokGiris = function (obj) {
        return GetBaseRequestPostJson("StokGiris", "Import", obj);
    };

    //Stok Cikis======================>

    this.ListStokCikises = function (ReferansNo, BeyannameNo, TPSNo) {
        return GetBaseRequestJson("StokCikis", "List", undefined, 'referansNo=' + ReferansNo + '&beyannameNo=' + BeyannameNo + '&tpsNo=' + TPSNo);
    };

    this.GetStokCikis = function (id) {
        return GetBaseRequestJson("StokCikis", "Get", id);
    };
    
    this.SetWorkOrderService = function (id) {
        return GetBaseRequestJson("StokCikis", "SetWorkOrderService", id);
    };

    this.AddStokCikis = function (obj) {
        return GetBaseRequestPostJson("StokCikis", "Add", obj);
    };

    this.EditStokCikis = function (obj) {
        return GetBaseRequestPostJson("StokCikis", "Edit", obj);
    };

    this.GetStokDusumListe = function (itemNo, cikisAdet) {
        return GetBaseRequestJson("StokCikis", "GetStokDusumListe", undefined, 'itemNo=' + itemNo + "&cikisAdet=" + cikisAdet);
    };

    this.GetStokDusumListeAdd = function (obj) {
        return GetBaseRequestPostJson("StokCikis", "StokDusumListeAdd", obj);
    };

    this.InsertStokCikisFromStokDusumListe = function (stokCikisId, itemNo, cikisAdet) {
        return GetBaseRequestJson("StokCikis", "InsertStokCikisFromStokDusumListe", undefined, 'stokCikisId=' + stokCikisId + '&itemNo=' + itemNo + "&cikisAdet=" + cikisAdet);
    };

});