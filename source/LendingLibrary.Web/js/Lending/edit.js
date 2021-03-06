﻿var LendingLibrary = LendingLibrary || {};
LendingLibrary.Web = LendingLibrary.Web || {};
LendingLibrary.Web.Lending = LendingLibrary.Web.Lending || {};
LendingLibrary.Web.Lending.Edit = LendingLibrary.Web.Lending.Edit || {};

(function (ns) {
    ns.editLendingEntry = function (lendingId, editUrl, redirectUrl) {
        $.get(editUrl, { id: lendingId })
            .then(function (response) {
                $.redirect(redirectUrl, response);
            })
            .fail(function (result) {
                sweetAlert({
                    title: "Error: " + result.statusText,
                    text: "",
                    type: "error",
                    confirmButtonColor: "#7CB43F"
                });
            });
    };
})(LendingLibrary.Web.Lending.Edit);